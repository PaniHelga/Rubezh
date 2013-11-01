﻿using System.Collections.Generic;
using Common;
using Common.GK;
using Infrastructure;
using Infrastructure.Common.Windows;
using GKProcessor.Events;
using XFiresecAPI;
using System.Threading;
using System;
using System.Linq;
using System.Diagnostics;
using FiresecClient;
using Infrastructure.Common.Services;

namespace GKProcessor
{
	public partial class Watcher
	{
		bool IsAnyDBMissmatch = false;

		void GetAllStates(bool showProgress)
		{
			Trace.WriteLine("GetAllStates");
			IsAnyDBMissmatch = false;

			if (showProgress)
				StartProgress("Опрос объектов ГК", GkDatabase.BinaryObjects.Count, false);
			foreach (var descriptor in GkDatabase.Descriptors)
			{
				LastUpdateTime = DateTime.Now;
				var result = GetState(descriptor.XBase);
				if (!result)
				{
					if (descriptor.Device != null && descriptor.Device.Driver.DriverType == XDriverType.GK)
					{
						descriptor.Device.DeviceState.IsConnectionLost = true;
						break;
					}
				}
				if (showProgress)
					DoProgress(descriptor.XBase.DescriptorInfo);
			}
			foreach (var device in XManager.Devices)
			{
				if (device.Driver.DriverType == XDriverType.KAU_Shleif || device.Driver.DriverType == XDriverType.RSR2_KAU_Shleif)
				{
					device.DeviceState.OnStateChanged();
				}
			}
			if (showProgress)
				StopProgress();

			if (IsAnyDBMissmatch)
			{
				foreach (var descriptor in GkDatabase.Descriptors)
				{
					var baseState = descriptor.XBase.GetXBaseState();
					baseState.StateBits = new List<XStateBit>() { XStateBit.Norm };
					baseState.IsGKMissmatch = true;
				}
			}
			else
			{
				foreach (var descriptor in GkDatabase.Descriptors)
				{
					var baseState = descriptor.XBase.GetXBaseState();
					baseState.IsGKMissmatch = false;
				}
			}
			CheckTechnologicalRegime();

			ApplicationService.Invoke(() => { ServiceFactoryBase.Events.GetEvent<GKObjectsStateChangedEvent>().Publish(null); });
		}

		bool GetState(XBase xBase)
		{
			var no = xBase.GKDescriptorNo;
			var sendResult = SendManager.Send(xBase.GkDatabaseParent, 2, 12, 68, BytesHelper.ShortToBytes(no));
			if (sendResult.HasError)
			{
				ConnectionChanged(false);
				return false;
			}
			if (sendResult.Bytes.Count != 68)
			{
				IsAnyDBMissmatch = true;
				ApplicationService.Invoke(() => { xBase.GetXBaseState().IsGKMissmatch = true; });
				return false;
			}
			ConnectionChanged(true);
			var binaryObjectStateHelper = new BinaryObjectStateHelper();
            binaryObjectStateHelper.Parse(sendResult.Bytes);
			CheckDBMissmatch(xBase, binaryObjectStateHelper);
			ApplicationService.Invoke(() =>
			{
				var binaryState = xBase.GetXBaseState();
				binaryState.AdditionalStates = binaryObjectStateHelper.AdditionalStates;
				binaryState.AdditionalStateProperties = binaryObjectStateHelper.AdditionalStateProperties;
				binaryState.OnDelay = binaryObjectStateHelper.OnDelay;
				binaryState.HoldDelay = binaryObjectStateHelper.HoldDelay;
				binaryState.OffDelay = binaryObjectStateHelper.OffDelay;
				binaryState.LastDateTime = DateTime.Now;
				binaryState.StateBits = binaryObjectStateHelper.StateBits; // OnStateChanged();
			});

			return true;
		}

		void CheckAdditionalStates(BaseDescriptor descriptor)
		{
			if (descriptor is DeviceDescriptor)
			{
				var deviceDescriptor = descriptor as DeviceDescriptor;
				if (deviceDescriptor.Device.Driver.DriverType == XDriverType.GK || deviceDescriptor.Device.Driver.DriverType == XDriverType.KAU)
				{
					GetState(descriptor.XBase);
				}
			}
		}

		void CheckDBMissmatch(XBase xBase, BinaryObjectStateHelper binaryObjectState)
		{
			bool isMissmatch = false;
			if (xBase is XDevice)
			{
				var device = xBase as XDevice;
				if (device.Driver.DriverTypeNo != binaryObjectState.TypeNo)
					isMissmatch = true;

				ushort physicalAddress = device.IntAddress;
				if (device.Driver.IsDeviceOnShleif)
					physicalAddress = (ushort)((device.ShleifNoNew - 1) * 256 + device.IntAddress);
				if (device.Driver.DriverType != XDriverType.GK && device.Driver.DriverType != XDriverType.KAU && device.Driver.DriverType != XDriverType.RSR2_KAU
					&& device.Driver.HasAddress && physicalAddress != binaryObjectState.PhysicalAddress)
					isMissmatch = true;

				if (device.GetNearestDatabaseNo() != binaryObjectState.AddressOnController)
					isMissmatch = true;
			}
			if (xBase is XZone)
			{
				var zone = xBase as XZone;
				if (binaryObjectState.TypeNo != 0x100)
					isMissmatch = true;
			}
			if (xBase is XDirection)
			{
				var direction = xBase as XDirection;
				if (binaryObjectState.TypeNo != 0x106)
					isMissmatch = true;
			}
			if (xBase is XDelay)
			{
				var delay = xBase as XDelay;
				if (binaryObjectState.TypeNo != 0x101)
					isMissmatch = true;
			}

            var description = xBase.GetDescriptorName();
            if (xBase.GetDescriptorName().TrimEnd(' ') != binaryObjectState.Description)
				isMissmatch = true;

			xBase.GetXBaseState().IsRealMissmatch = isMissmatch;
			if (isMissmatch)
			{
				IsAnyDBMissmatch = true;
			}
		}

		void CheckServiceRequired(XBase xBase, JournalItem journalItem)
		{
			if (journalItem.Name != "Запыленность")
				return;

			if (xBase is XDevice)
			{
				var device = xBase as XDevice;
				bool isDusted = journalItem.YesNo == JournalYesNoType.Yes;
				ApplicationService.Invoke(() => { device.DeviceState.IsService = isDusted; });
			}
		}

		#region TechnologicalRegime
		void CheckTechnologicalRegime()
		{
			var isInTechnologicalRegime = IsInTechnologicalRegime(GkDatabase.RootDevice);
			foreach (var descriptor in GkDatabase.Descriptors)
			{
				var baseState = descriptor.XBase.GetXBaseState();
				baseState.IsInTechnologicalRegime = isInTechnologicalRegime;
			}

			if (!isInTechnologicalRegime)
			{
				foreach (var kauDatabase in GkDatabase.KauDatabases)
				{
					isInTechnologicalRegime = IsInTechnologicalRegime(kauDatabase.RootDevice);
					var allChildren = XManager.GetAllDeviceChildren(kauDatabase.RootDevice);
					allChildren.Add(kauDatabase.RootDevice);
					foreach (var device in allChildren)
					{
						var baseState = device.GetXBaseState();
						baseState.IsInTechnologicalRegime = isInTechnologicalRegime;
					}
				}
			}
		}

		bool IsInTechnologicalRegime(XDevice device)
		{
			var sendResult = SendManager.Send(device, 0, 1, 1, null, true, false, 2000);
			if (!sendResult.HasError)
			{
				if (sendResult.Bytes.Count > 0)
				{
					var version = sendResult.Bytes[0];
					if (version > 127)
					{
						return true;
					}
				}
			}
			return false;
		}
		#endregion
	}
}