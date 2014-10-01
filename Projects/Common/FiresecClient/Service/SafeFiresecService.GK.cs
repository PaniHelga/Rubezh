﻿using System;
using System.Collections.Generic;
using FiresecAPI;
using FiresecAPI.GK;
using FiresecAPI.Journal;
using GKProcessor;
using Infrastructure.Common;

namespace FiresecClient
{
	public partial class SafeFiresecService
	{
		static bool IsGKAsAService = GlobalSettingsHelper.GlobalSettings.IsGKAsAService;

		public void CancelGKProgress(Guid progressCallbackUID, string userName)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => FiresecService.CancelGKProgress(progressCallbackUID, userName), "CancelGKProgress");
			}
			else
			{
				GKProcessorManager.CancelGKProgress(progressCallbackUID, userName);
			}
		}

		public OperationResult<bool> GKWriteConfiguration(XDevice device)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => FiresecService.GKWriteConfiguration(device.UID), "GKWriteConfiguration");
			}
			else
			{
				var result = GKProcessorManager.GKWriteConfiguration(device, FiresecManager.CurrentUser.Name);
				if (!result.HasError)
					FiresecManager.FiresecService.NotifyClientsOnConfigurationChanged();
				return result;
			}
		}

		public OperationResult<XDeviceConfiguration> GKReadConfiguration(XDevice device)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => FiresecService.GKReadConfiguration(device.UID), "GKReadConfiguration");
			}
			else
			{
				return GKProcessorManager.GKReadConfiguration(device, FiresecManager.CurrentUser.Name);
			}
		}

		public OperationResult<XDeviceConfiguration> GKReadConfigurationFromGKFile(XDevice device)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => FiresecService.GKReadConfigurationFromGKFile(device.UID), "GKReadConfigurationFromGKFile");
			}
			else
			{
				return GKProcessorManager.GKReadConfigurationFromGKFile(device, FiresecManager.CurrentUser.Name);
			}
		}

		public OperationResult<bool> GKUpdateFirmware(XDevice device, string fileName)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => FiresecService.GKUpdateFirmware(device.UID, fileName), "GKUpdateFirmware");
			}
			else
			{
				return GKProcessorManager.GKUpdateFirmware(device, fileName, FiresecManager.CurrentUser.Name);
			}
		}

		public OperationResult<bool> GKUpdateFirmwareFSCS(HexFileCollectionInfo hxcFileInfo, List<XDevice> devices)
		{
			if (IsGKAsAService)
			{
				var deviceUIDs = new List<Guid>();
				foreach (var device in devices)
				{
					deviceUIDs.Add(device.UID);
				}
				var result = SafeOperationCall(() => FiresecService.GKUpdateFirmwareFSCS(hxcFileInfo, FiresecManager.CurrentUser.Name, deviceUIDs), "GKUpdateFirmwareFSCS");
				return result;
			}
			else
			{
				return GKProcessorManager.GKUpdateFirmwareFSCS(hxcFileInfo, FiresecManager.CurrentUser.Name, devices);
			}
		}

		public OperationResult<bool> GKSyncronyseTime(XDevice device)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => { return FiresecService.GKSyncronyseTime(device.UID); }, "GKSyncronyseTime");
			}
			else
			{
				return new OperationResult<bool>() { Result = GKProcessorManager.GKSyncronyseTime(device, FiresecManager.CurrentUser.Name) };
			}
		}

		public OperationResult<string> GKGetDeviceInfo(XDevice device)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => { return FiresecService.GKGetDeviceInfo(device.UID); }, "GKGetDeviceInfo");
			}
			else
			{
				return new OperationResult<string>() { Result = GKProcessorManager.GKGetDeviceInfo(device, FiresecManager.CurrentUser.Name) };
			}
		}

		public OperationResult<int> GKGetJournalItemsCount(XDevice device)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => { return FiresecService.GKGetJournalItemsCount(device.UID); }, "GKGetJournalItemsCount");
			}
			else
			{
				return GKProcessorManager.GKGetJournalItemsCount(device);
			}
		}

		public OperationResult<XJournalItem> GKReadJournalItem(XDevice device, int no)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => { return FiresecService.GKReadJournalItem(device.UID, no); }, "GKReadJournalItem");
			}
			else
			{
				return GKProcessorManager.GKReadJournalItem(device, no);
			}
		}

		public OperationResult<bool> GKSetSingleParameter(XBase xBase, List<byte> parameterBytes)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall<bool>(() => { return FiresecService.GKSetSingleParameter(xBase.UID, parameterBytes); }, "SetSingleParameter");
			}
			else
			{
				return GKProcessorManager.GKSetSingleParameter(xBase, parameterBytes);
			}
		}

		public OperationResult<List<XProperty>> GKGetSingleParameter(XBase xBase)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall<List<XProperty>>(() => { return FiresecService.GKGetSingleParameter(xBase.UID); }, "GetSingleParameter");
			}
			else
			{
				return GKProcessorManager.GKGetSingleParameter(xBase);
			}
		}

		public OperationResult<List<byte>> GKGKHash(XDevice device)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall<List<byte>>(() => { return FiresecService.GKGKHash(device.UID); }, "GKGKHash");
			}
			else
			{
				return GKProcessorManager.GKGKHash(device);
			}
		}

		public GKStates GKGetStates()
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall<GKStates>(() => { return FiresecService.GKGetStates(); }, "GKGetStates");
			}
			else
			{
				return GKProcessorManager.GKGetStates();
			}
		}

		public void GKExecuteDeviceCommand(XDevice device, XStateBit stateBit)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKExecuteDeviceCommand(device.UID, stateBit); }, "GKExecuteDeviceCommand");
			}
			else
			{
				GKProcessorManager.GKExecuteDeviceCommand(device, stateBit, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKReset(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKReset(xBase.UID, xBase.ObjectType); }, "GKReset");
			}
			else
			{
				GKProcessorManager.GKReset(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKResetFire1(XZone zone)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKResetFire1(zone.UID); }, "GKResetFire1");
			}
			else
			{
				GKProcessorManager.GKResetFire1(zone, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKResetFire2(XZone zone)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKResetFire2(zone.UID); }, "GKResetFire2");
			}
			else
			{
				GKProcessorManager.GKResetFire2(zone, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKSetAutomaticRegime(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKSetAutomaticRegime(xBase.UID, xBase.ObjectType); }, "GKSetAutomaticRegime");
			}
			else
			{
				GKProcessorManager.GKSetAutomaticRegime(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKSetManualRegime(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKSetManualRegime(xBase.UID, xBase.ObjectType); }, "GKSetManualRegime");
			}
			else
			{
				GKProcessorManager.GKSetManualRegime(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKSetIgnoreRegime(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKSetIgnoreRegime(xBase.UID, xBase.ObjectType); }, "GKTurnOn");
			}
			else
			{
				GKProcessorManager.GKSetIgnoreRegime(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKTurnOn(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKTurnOn(xBase.UID, xBase.ObjectType); }, "GKTurnOn");
			}
			else
			{
				GKProcessorManager.GKTurnOn(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKTurnOnNow(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKTurnOnNow(xBase.UID, xBase.ObjectType); }, "GKTurnOnNow");
			}
			else
			{
				GKProcessorManager.GKTurnOnNow(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKTurnOff(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKTurnOff(xBase.UID, xBase.ObjectType); }, "GKTurnOff");
			}
			else
			{
				GKProcessorManager.GKTurnOff(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKTurnOffNow(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKTurnOffNow(xBase.UID, xBase.ObjectType); }, "GKTurnOffNow");
			}
			else
			{
				GKProcessorManager.GKTurnOffNow(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKStop(XBase xBase)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKStop(xBase.UID, xBase.ObjectType); }, "GKStop");
			}
			else
			{
				GKProcessorManager.GKStop(xBase, FiresecManager.CurrentUser.Name);
			}
		}

		public void GKStartMeasureMonitoring(XDevice device)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKStartMeasureMonitoring(device.UID); }, "GKStartMeasureMonitoring");
			}
			else
			{
				GKProcessorManager.GKStartMeasureMonitoring(device);
			}
		}

		public void GKStopMeasureMonitoring(XDevice device)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => { FiresecService.GKStopMeasureMonitoring(device.UID); }, "GKStopMeasureMonitoring");
			}
			else
			{
				GKProcessorManager.GKStopMeasureMonitoring(device);
			}
		}

		public void GKAddMessage(JournalEventNameType journalEventNameType, string description)
		{
			if (IsGKAsAService)
			{
			}
			else
			{
				//GKProcessorManager.AddGKMessage(journalEventNameType, description, null, FiresecManager.CurrentUser.Name, true);
			}
		}

		#region Journal
		public List<XJournalItem> GetGKTopLastJournalItems(int count)
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => FiresecService.GetGKTopLastJournalItems(count), "GetGKTopLastJournalItems");
			}
			else
			{
				return GKDBHelper.GetGKTopLastJournalItems(count);
			}
		}

		public void BeginGetGKFilteredArchive(XArchiveFilter archiveFilter, Guid archivePortionUID)
		{
			if (IsGKAsAService)
			{
				SafeOperationCall(() => FiresecService.BeginGetGKFilteredArchive(archiveFilter, archivePortionUID), "BeginGetGKFilteredArchive");
			}
			else
			{
				SafeOperationCall(() => FiresecService.BeginGetGKFilteredArchive(archiveFilter, archivePortionUID), "BeginGetGKFilteredArchive");
			}
		}

		public List<string> GetGkEventNames()
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => FiresecService.GetDistinctGKJournalNames(), "GetGkEventNames");
			}
			else
			{
				return GKDBHelper.EventNames;
			}
		}

		public List<string> GetGkEventDescriptions()
		{
			if (IsGKAsAService)
			{
				return SafeOperationCall(() => FiresecService.GetDistinctGKJournalDescriptions(), "GetGkEventDescriptions");
			}
			else
			{
				return GKDBHelper.EventDescriptions;
			}
		}
		#endregion
	}
}