﻿using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using Common;
using FiresecAPI;
using FiresecAPI.Models;
using XFiresecAPI;
using System.Linq;

namespace FiresecService.Service
{
	[ServiceBehavior(MaxItemsInObjectGraph = Int32.MaxValue, UseSynchronizationContext = false,
	InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class SafeFiresecService : IFiresecService
	{
		public FiresecService FiresecService { get; set; }

		public SafeFiresecService()
		{
			FiresecService = new FiresecService();
		}

		public void BeginOperation(string operationName)
		{
		}

		public void EndOperation()
		{
		}

		public OperationResult<T> CreateEmptyOperationResult<T>(string message)
		{
			var operationResult = new OperationResult<T>
			{
				Result = default(T),
				HasError = true,
				Error = "Ошибка при выполнении операции на сервере" + "\n\r" + message
			};
			return operationResult;
		}

		OperationResult<T> SafeOperationCall<T>(Func<OperationResult<T>> func, string operationName)
		{
			try
			{
				BeginOperation(operationName);
				var result = func();
				EndOperation();
				return result;
			}
			catch (Exception e)
			{
				Logger.Error(e, "Исключение при вызове SafeFiresecService.SafeOperationCall. operationName = " + operationName);
				return CreateEmptyOperationResult<T>(e.Message + "\n" + e.StackTrace);
			}
		}

		T SafeOperationCall<T>(Func<T> func, string operationName)
		{
			try
			{
				BeginOperation(operationName);
				var result = func();
				EndOperation();
				return result;
			}
			catch (Exception e)
			{
				Logger.Error(e, "Исключение при вызове SafeFiresecService.SafeOperationCall. operationName = " + operationName);
			}
			return default(T);
		}

		void SafeOperationCall(Action action, string operationName)
		{
			try
			{
				BeginOperation(operationName);
				action();
				EndOperation();
			}
			catch (Exception e)
			{
				Logger.Error(e, "Исключение при вызове SafeFiresecService.SafeOperationCall. operationName = " + operationName);
			}
		}

		public OperationResult<bool> Connect(Guid uid, ClientCredentials clientCredentials, bool isNew)
		{
			return SafeOperationCall(() => { return FiresecService.Connect(uid, clientCredentials, isNew); }, "Connect");
		}

		public OperationResult<bool> Reconnect(Guid uid, string userName, string password)
		{
			return SafeOperationCall(() => { return FiresecService.Reconnect(uid, userName, password); }, "Reconnect");
		}

		public void Disconnect(Guid uid)
		{
			SafeOperationCall(() => { FiresecService.Disconnect(uid); }, "Disconnect");
		}

		public string Ping()
		{
			return SafeOperationCall(() => { return FiresecService.Ping(); }, "Ping");
		}

		public List<CallbackResult> Poll(Guid uid)
		{
			return SafeContext.Execute<List<CallbackResult>>(() => FiresecService.Poll(uid));
		}

		public void NotifyClientsOnConfigurationChanged()
		{
			SafeOperationCall(() => { FiresecService.NotifyClientsOnConfigurationChanged(); }, "NotifyClientsOnConfigurationChanged");
		}

		public SecurityConfiguration GetSecurityConfiguration()
		{
			return SafeOperationCall(() => { return FiresecService.GetSecurityConfiguration(); }, "GetSecurityConfiguration");
		}

		public OperationResult<int> GetJournalLastId()
		{
			return SafeOperationCall(() => { return FiresecService.GetJournalLastId(); }, "GetJournalLastId");
		}

		public OperationResult<List<FiresecAPI.Models.JournalRecord>> GetFilteredJournal(FiresecAPI.Models.JournalFilter journalFilter)
		{
			return SafeOperationCall(() => { return FiresecService.GetFilteredJournal(journalFilter); }, "GetFilteredJournal");
		}

		public OperationResult<List<JournalRecord>> GetFilteredArchive(ArchiveFilter archiveFilter)
		{
			return SafeOperationCall(() => { return FiresecService.GetFilteredArchive(archiveFilter); }, "GetFilteredArchive");
		}

		public void BeginGetFilteredArchive(FiresecAPI.Models.ArchiveFilter archiveFilter)
		{
			SafeOperationCall(() => { FiresecService.BeginGetFilteredArchive(archiveFilter); }, "BeginGetFilteredArchive");
		}

		public OperationResult<List<FiresecAPI.Models.JournalDescriptionItem>> GetDistinctDescriptions()
		{
			return SafeOperationCall(() => { return FiresecService.GetDistinctDescriptions(); }, "GetDistinctDescriptions");
		}

		public OperationResult<DateTime> GetArchiveStartDate()
		{
			return SafeOperationCall(() => { return FiresecService.GetArchiveStartDate(); }, "GetArchiveStartDate");
		}

		public void AddJournalRecords(List<JournalRecord> journalRecords)
		{
			SafeOperationCall(() => { FiresecService.AddJournalRecords(journalRecords); }, "AddJournalRecords");
		}

		public List<string> GetFileNamesList(string directory)
		{
			return SafeOperationCall(() => { return FiresecService.GetFileNamesList(directory); }, "GetFileNamesList");
		}

		public Dictionary<string, string> GetDirectoryHash(string directory)
		{
			return SafeOperationCall(() => { return FiresecService.GetDirectoryHash(directory); }, "GetDirectoryHash");
		}

		public System.IO.Stream GetFile(string dirAndFileName)
		{
			return SafeOperationCall(() => { return FiresecService.GetFile(dirAndFileName); }, "GetFile");
		}

		public Stream GetConfig()
		{
			return SafeOperationCall(() => { return FiresecService.GetConfig(); }, "GetConfig");
		}

		public void SetConfig(Stream stream)
		{
			SafeOperationCall(() => { FiresecService.SetConfig(stream); }, "SetConfig");
		}

		public void SetJournal(List<JournalRecord> journalRecords)
		{
			SafeOperationCall(() => { FiresecService.SetJournal(journalRecords); }, "ConvertJournal");
		}

		public string Test(string arg)
		{
			return SafeOperationCall(() => { return FiresecService.Test(arg); }, "Test");
		}

		#region SKD
		#region Get
		public OperationResult<IEnumerable<Employee>> GetEmployees(EmployeeFilter filter)
		{
			return SafeContext.Execute < OperationResult<IEnumerable<Employee>>>(() => FiresecService.GetEmployees(filter));
		}
		public OperationResult<IEnumerable<Position>> GetPositions(PositionFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<Position>>>(() => FiresecService.GetPositions(filter));
		}
		public OperationResult<IEnumerable<Department>> GetDepartments(DepartmentFilter filter)
		{
			return SafeContext.Execute < OperationResult<IEnumerable<Department>>>(() => FiresecService.GetDepartments(filter));
		}
		public OperationResult<IEnumerable<SKDJournalItem>> GetSKDJournalItems(SKDJournalFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<SKDJournalItem>>>(() => FiresecService.GetSKDJournalItems(filter));
		}
		public OperationResult<IEnumerable<SKDCard>> GetCards(CardFilter filter)
		{
			return SafeContext.Execute < OperationResult < IEnumerable < SKDCard >>> (() => FiresecService.GetCards(filter));
		}
		public OperationResult<IEnumerable<CardZone>> GetCardZones(CardZoneFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<CardZone>>>(() => FiresecService.GetCardZones(filter));
		}
		public OperationResult<IEnumerable<Organization>> GetOrganizations(OrganizationFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<Organization>>>(() => FiresecService.GetOrganizations(filter));
		}
		public OperationResult<IEnumerable<Document>> GetDocuments(DocumentFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<Document>>>(() => FiresecService.GetDocuments(filter));
		}
		public OperationResult<IEnumerable<AccessTemplate>> GetGUDs(AccessTemplateFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<AccessTemplate>>>(() => FiresecService.GetGUDs(filter));
		}
		public OperationResult<IEnumerable<AdditionalColumnType>> GetAdditionalColumnTypes(AdditionalColumnTypeFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<AdditionalColumnType>>>(() => FiresecService.GetAdditionalColumnTypes(filter));
		}
		public OperationResult<IEnumerable<AdditionalColumn>> GetAdditionalColumns(AdditionalColumnFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<AdditionalColumn>>>(() => FiresecService.GetAdditionalColumns(filter));
		}
		public OperationResult<IEnumerable<Photo>> GetPhotos(PhotoFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<Photo>>>(() => FiresecService.GetPhotos(filter));
		}
		public OperationResult<IEnumerable<EmployeeReplacement>> GetEmployeeReplacements(EmployeeReplacementFilter filter)
		{
			return SafeContext.Execute<OperationResult<IEnumerable<EmployeeReplacement>>>(() => FiresecService.GetEmployeeReplacements(filter));
		}
		#endregion

		#region Save
		public OperationResult SaveEmployees(IEnumerable<Employee> Employees)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveEmployees(Employees));
		}
		public OperationResult SaveDepartments(IEnumerable<Department> Departments)
		{
			return SafeContext.Execute < OperationResult>(() => FiresecService.SaveDepartments(Departments));
		}
		public OperationResult SavePositions(IEnumerable<Position> Positions)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SavePositions(Positions));
		}
		public OperationResult SaveSKDJournalItems(IEnumerable<SKDJournalItem> SKDJournalItems)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveSKDJournalItems(SKDJournalItems));
		}
		public OperationResult SaveCards(IEnumerable<SKDCard> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveCards(items));
		}
		public OperationResult SaveCardZones(IEnumerable<CardZone> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveCardZones(items));
		}
		public OperationResult SaveOrganizations(IEnumerable<Organization> Organizations)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveOrganizations(Organizations));
		}
		public OperationResult SaveOrganizationZones(Organization organization)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveOrganizationZones(organization));
		}
		public OperationResult SaveDocuments(IEnumerable<Document> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveDocuments(items));
		}
		public OperationResult SaveGUDs(IEnumerable<AccessTemplate> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveGUDs(items));
		}
		public OperationResult SaveAdditionalColumnTypes(IEnumerable<AdditionalColumnType> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveAdditionalColumnTypes(items));
		}
		public OperationResult SaveAdditionalColumns(IEnumerable<AdditionalColumn> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveAdditionalColumns(items));
		}
		public OperationResult SavePhotos(IEnumerable<Photo> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SavePhotos(items));
		}
		public OperationResult SaveEmployeeReplacements(IEnumerable<EmployeeReplacement> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.SaveEmployeeReplacements(items));
		}
		#endregion

		#region MarkDeleted
		public OperationResult MarkDeletedEmployees(IEnumerable<Employee> Employees)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedEmployees(Employees));
		}
		public OperationResult MarkDeletedDepartments(IEnumerable<Department> Departments)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedDepartments(Departments));
		}
		public OperationResult MarkDeletedPositions(IEnumerable<Position> Positions)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedPositions(Positions));
		}
		public OperationResult MarkDeletedCards(IEnumerable<SKDCard> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedCards(items));
		}
		public OperationResult MarkDeletedCardZones(IEnumerable<CardZone> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedCardZones(items));
		}
		public OperationResult MarkDeletedOrganizations(IEnumerable<Organization> Organizations)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedOrganizations(Organizations));
		}
		public OperationResult MarkDeletedDocuments(IEnumerable<Document> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedDocuments(items));
		}
		public OperationResult MarkDeletedGUDs(IEnumerable<AccessTemplate> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedGUDs(items));
		}
		public OperationResult MarkDeletedAdditionalColumnTypes(IEnumerable<AdditionalColumnType> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedAdditionalColumnTypes(items));
		}
		public OperationResult MarkDeletedAdditionalColumns(IEnumerable<AdditionalColumn> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedAdditionalColumns(items));
		}
		public OperationResult MarkDeletedPhotos(IEnumerable<Photo> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedPhotos(items));
		}
		public OperationResult MarkDeletedEmployeeReplacements(IEnumerable<EmployeeReplacement> items)
		{
			return SafeContext.Execute<OperationResult>(() => FiresecService.MarkDeletedEmployeeReplacements(items));
		}
		#endregion

		public OperationResult<string> SKDGetDeviceInfo(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.SKDGetDeviceInfo(deviceUID); }, "SKDGetDeviceInfo");
		}
		public OperationResult<bool> SKDSyncronyseTime(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.SKDSyncronyseTime(deviceUID); }, "SKDSyncronyseTime");
		}
		public OperationResult<bool> SKDWriteConfiguration(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.SKDWriteConfiguration(deviceUID); }, "SKDWriteConfiguration");
		}
		public OperationResult<bool> SKDUpdateFirmware(Guid deviceUID, string fileName)
		{
			return SafeOperationCall(() => { return FiresecService.SKDUpdateFirmware(deviceUID, fileName); }, "SKDUpdateFirmware");
		}
		public void SKDSetRegimeOpen(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDSetRegimeOpen(deviceUID); }, "SKDSetIgnoreRegime");
		}
		public void SKDSetRegimeClose(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDSetRegimeClose(deviceUID); }, "SKDSetIgnoreRegime");
		}
		public void SKDSetRegimeControl(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDSetRegimeControl(deviceUID); }, "SKDSetRegimeControl");
		}
		public void SKDSetRegimeConversation(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDSetRegimeConversation(deviceUID); }, "SKDSetRegimeConversation");
		}
		public void SKDOpenDevice(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDOpenDevice(deviceUID); }, "SKDOpenDevice");
		}
		public void SKDCloseDevice(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDCloseDevice(deviceUID); }, "SKDCloseDevice");
		}
		public void SKDAllowReader(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDAllowReader(deviceUID); }, "SKDAllowReader");
		}

		public void SKDDenyReader(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.SKDDenyReader(deviceUID); }, "SKDDenyReader");
		}
		#endregion

		#region GK

		public void CancelGKProgress(Guid progressCallbackUID, string userName)
		{
			SafeOperationCall(() => { FiresecService.CancelGKProgress(progressCallbackUID, userName); }, "CancelGKProgress");
		}

		public OperationResult<bool> GKWriteConfiguration(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKWriteConfiguration(deviceUID); }, "GKWriteConfiguration");
		}

		public OperationResult<XDeviceConfiguration> GKReadConfiguration(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKReadConfiguration(deviceUID); }, "GKReadConfiguration");
		}

		public OperationResult<XDeviceConfiguration> GKReadConfigurationFromGKFile(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKReadConfigurationFromGKFile(deviceUID); }, "GKReadConfigurationFromGKFile");
		}

		public OperationResult<bool> GKUpdateFirmware(Guid deviceUID, string fileName)
		{
			return SafeOperationCall(() => { return FiresecService.GKUpdateFirmware(deviceUID, fileName); }, "GKUpdateFirmware");
		}

		public OperationResult<bool> GKUpdateFirmwareFSCS(HexFileCollectionInfo hxcFileInfo, string userName, List<Guid> deviceUIDs)
		{
			var result = SafeOperationCall(() => { return FiresecService.GKUpdateFirmwareFSCS(hxcFileInfo, userName, deviceUIDs); }, "GKUpdateFirmwareFSCS");
			return result;
		}

		public OperationResult<bool> GKSyncronyseTime(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKSyncronyseTime(deviceUID); }, "GKSyncronyseTime");
		}

		public OperationResult<string> GKGetDeviceInfo(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKGetDeviceInfo(deviceUID); }, "GKGetDeviceInfo");
		}

		public OperationResult<int> GKGetJournalItemsCount(Guid deviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKGetJournalItemsCount(deviceUID); }, "GKGetJournalItemsCount");
		}

		public OperationResult<JournalItem> GKReadJournalItem(Guid deviceUID, int no)
		{
			return SafeOperationCall(() => { return FiresecService.GKReadJournalItem(deviceUID, no); }, "GKReadJournalItem");
		}

		public OperationResult<bool> GKSetSingleParameter(Guid objectUID, List<byte> parameterBytes)
		{
			return SafeOperationCall(() => { return FiresecService.GKSetSingleParameter(objectUID, parameterBytes); }, "GKSetSingleParameter");
		}

		public OperationResult<List<XProperty>> GKGetSingleParameter(Guid objectUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKGetSingleParameter(objectUID); }, "GKGetSingleParameter");
		}

		public OperationResult<List<byte>> GKGKHash(Guid gkDeviceUID)
		{
			return SafeOperationCall(() => { return FiresecService.GKGKHash(gkDeviceUID); }, "GKGKHash");
		}

		public GKStates GKGetStates()
		{
			return SafeOperationCall(() => { return FiresecService.GKGetStates(); }, "GKGetStates");
		}
		public void GKExecuteDeviceCommand(Guid deviceUID, XStateBit stateBit)
		{
			SafeOperationCall(() => { FiresecService.GKExecuteDeviceCommand(deviceUID, stateBit); }, "GKExecuteDeviceCommand");
		}

		public void GKReset(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKReset(uid, objectType); }, "GKReset");
		}

		public void GKResetFire1(Guid zoneUID)
		{
			SafeOperationCall(() => { FiresecService.GKResetFire1(zoneUID); }, "GKResetFire1");
		}

		public void GKResetFire2(Guid zoneUID)
		{
			SafeOperationCall(() => { FiresecService.GKResetFire2(zoneUID); }, "GKResetFire2");
		}

		public void GKSetAutomaticRegime(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKSetAutomaticRegime(uid, objectType); }, "GKSetAutomaticRegime");
		}

		public void GKSetManualRegime(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKSetManualRegime(uid, objectType); }, "GKSetManualRegime");
		}

		public void GKSetIgnoreRegime(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKSetIgnoreRegime(uid, objectType); }, "GKSetIgnoreRegime");
		}

		public void GKTurnOn(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKTurnOn(uid, objectType); }, "GKTurnOn");
		}

		public void GKTurnOnNow(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKTurnOnNow(uid, objectType); }, "GKTurnOnNow");
		}

		public void GKTurnOff(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKTurnOff(uid, objectType); }, "GKTurnOff");
		}

		public void GKTurnOffNow(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKTurnOffNow(uid, objectType); }, "GKTurnOffNow");
		}

		public void GKStop(Guid uid, XBaseObjectType objectType)
		{
			SafeOperationCall(() => { FiresecService.GKStop(uid, objectType); }, "GKStop");
		}

		public void GKStartMeasureMonitoring(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.GKStartMeasureMonitoring(deviceUID); }, "GKStartMeasureMonitoring");
		}

		public void GKStopMeasureMonitoring(Guid deviceUID)
		{
			SafeOperationCall(() => { FiresecService.GKStopMeasureMonitoring(deviceUID); }, "GKStopMeasureMonitoring");
		}


		public void AddJournalItem(JournalItem journalItem)
		{
			SafeOperationCall(() => { FiresecService.AddJournalItem(journalItem); }, "AddJournalItem");
		}

		public List<JournalItem> GetGKTopLastJournalItems(int count)
		{
			return SafeOperationCall(() => { return FiresecService.GetGKTopLastJournalItems(count); }, "GetGKTopLastJournalItems");
		}

		public void BeginGetGKFilteredArchive(XArchiveFilter archiveFilter)
		{
			SafeOperationCall(() => { FiresecService.BeginGetGKFilteredArchive(archiveFilter); }, "BeginGetGKFilteredArchive");
		}

		public List<string> GetDistinctGKJournalNames()
		{
			return SafeOperationCall(() => { return FiresecService.GetDistinctGKJournalNames(); }, "GetDistinctGKJournalNames");
		}

		public List<string> GetDistinctGKJournalDescriptions()
		{
			return SafeOperationCall(() => { return FiresecService.GetDistinctGKJournalDescriptions(); }, "GetDistinctGKJournalDescriptions");
		}
		#endregion
	}
}