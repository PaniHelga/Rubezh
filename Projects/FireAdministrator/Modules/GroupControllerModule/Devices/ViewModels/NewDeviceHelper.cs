﻿using System;
using System.Linq;
using FiresecAPI.GK;
using FiresecClient;

namespace GKModule.ViewModels
{
	public static class NewDeviceHelper
	{
		public static byte GetMinAddress(GKDriver driver, GKDevice device)
		{
			GKDevice parentDevice = device;
			if (parentDevice.DriverType == GKDriverType.MPT || parentDevice.DriverType == GKDriverType.MRO_2)
				parentDevice = parentDevice.Parent;

			byte maxAddress = 0;

			if (driver.IsRangeEnabled)
			{
				maxAddress = driver.MinAddress;
			}
			else
			{
				if (parentDevice.Driver.IsGroupDevice)
				{
					maxAddress = (byte)parentDevice.IntAddress;
				}
			}

			foreach (var child in parentDevice.Children)
			{
				if (child.Driver.IsAutoCreate)
					continue;

				if (driver.IsRangeEnabled)
				{
					if ((child.IntAddress < driver.MinAddress) && (child.IntAddress > driver.MaxAddress))
						continue;
				}

				if (child.Driver.IsGroupDevice)
				{
					if (child.IntAddress + child.Driver.GroupDeviceChildrenCount - 1 > maxAddress)
						maxAddress = (byte)Math.Min(255, child.IntAddress + child.Driver.GroupDeviceChildrenCount - 1);
				}

				if (child.IntAddress > maxAddress)
					maxAddress = (byte)child.IntAddress;

				if (child.DriverType == GKDriverType.MPT || child.DriverType == GKDriverType.MRO_2)
				{
					foreach (var child2 in child.Children)
					{
						if (child2.IntAddress > maxAddress)
							maxAddress = (byte)child2.IntAddress;
					}
				}
			}

			if (driver.IsRangeEnabled)
			{
				if (parentDevice.Children.Where(x => x.Driver.IsAutoCreate == false).Count() > 0)
					if (maxAddress + 1 <= driver.MaxAddress)
						maxAddress = (byte)(maxAddress + 1);
			}
			else
			{
				if (parentDevice.Driver.IsGroupDevice)
				{
					if (parentDevice.Children.Count > 0)
						if (maxAddress + 1 <= parentDevice.IntAddress + driver.GroupDeviceChildrenCount - 1)
							maxAddress = (byte)(maxAddress + 1);
				}
				else
				{
					if (parentDevice.Children.Where(x => x.Driver.IsAutoCreate == false).ToList().Count > 0)
						if (((maxAddress + 1) % 256) != 0)
							maxAddress = (byte)(maxAddress + 1);
				}
			}

			return Math.Max((byte)1, maxAddress);
		}

		public static DeviceViewModel AddDevice(GKDevice device, DeviceViewModel parentDeviceViewModel, bool addAutoCreate = true)
		{
			var deviceViewModel = new DeviceViewModel(device);
			parentDeviceViewModel.AddChild(deviceViewModel);

			foreach (var childDevice in device.Children)
			{
				AddDevice(childDevice, deviceViewModel, addAutoCreate);
			}

			if (addAutoCreate)
			{
				if (device.Driver.IsGroupDevice)
				{
					var driver = GKManager.Drivers.FirstOrDefault(x => x.DriverType == device.Driver.GroupDeviceChildType);

					for (byte i = 0; i < device.Driver.GroupDeviceChildrenCount; i++)
					{
						var autoDevice = GKManager.AddChild(device, null, driver, (byte)(device.IntAddress + i));
						AddDevice(autoDevice, deviceViewModel, addAutoCreate);
					}
				}
			}
			return deviceViewModel;
		}

		public static DeviceViewModel InsertDevice(GKDevice device, DeviceViewModel parentDeviceViewModel)
		{
			var deviceViewModel = new DeviceViewModel(device);
			parentDeviceViewModel.InsertChild(deviceViewModel);

			foreach (var childDevice in device.Children)
			{
				AddDevice(childDevice, deviceViewModel);
			}

			if (device.Driver.IsGroupDevice && device.Children.Count == 0)
			{
				var driver = GKManager.Drivers.FirstOrDefault(x => x.DriverType == device.Driver.GroupDeviceChildType);

				for (byte i = 0; i < device.Driver.GroupDeviceChildrenCount; i++)
				{
					var autoDevice = GKManager.AddChild(device, null, driver, (byte)(device.IntAddress + i));
					AddDevice(autoDevice, deviceViewModel);
				}
			}
			return deviceViewModel;
		}
	}
}