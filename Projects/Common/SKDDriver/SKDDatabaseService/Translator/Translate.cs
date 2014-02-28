﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKDDriver.DataAccess;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Data.Linq;

namespace SKDDriver
{
	static partial class Translator
	{
		public static FiresecAPI.Department Translate(Department item)
		{
			if (item == null)
				return null;
			var phoneUids = new List<Guid>();
			item.Phone.ToList().ForEach(x => phoneUids.Add(x.Uid));
			var childDepartmentUids = new List<Guid>();
			item.Department2.ToList().ForEach(x => childDepartmentUids.Add(x.Uid));
			var result = new FiresecAPI.Department
			{
				Name = item.Name,
				Description = item.Description,
				ParentDepartmentUid = item.ParentDepartmentUid,
				ChildDepartmentUids = childDepartmentUids,
				ContactEmployeeUid = item.ContactEmployeeUid,
				AttendantEmployeeUId = item.AttendantUid,
				PhoneUids = phoneUids
			};
			TranslateOrganizationElement(item.Uid, item.IsDeleted, item.RemovalDate, item.OrganizationUid, result);
			return result;
		}

		public static FiresecAPI.Employee Translate(Employee item)
		{
			if (item == null)
				return null;
			var additionalColumnUids = new List<Guid>();
			item.AdditionalColumn.ToList().ForEach(x => additionalColumnUids.Add(x.Uid));
			Guid? replacementUid = null;
			if (item.EmployeeReplacement != null)
				replacementUid = item.EmployeeReplacement.Uid;
			var cardUids = new List<Guid>();
			foreach (var card in item.Card)
			{
				cardUids.Add(card.Uid);
			}

			var result = new FiresecAPI.Employee
			{
				FirstName = item.FirstName,
				SecondName = item.SecondName,
				LastName = item.LastName,
				Appointed = item.Appointed,
				Dismissed = item.Dismissed,
				PositionUid = item.PositionUid,
				ReplacementUid = replacementUid,
				DepartmentUid = item.DepartmentUid,
				ScheduleUid = item.ScheduleUid,
				AdditionalColumnUids = additionalColumnUids,
				Type = (FiresecAPI.PersonType)item.Type,
				CardUids = cardUids
			};
			TranslateOrganizationElement(item.Uid, item.IsDeleted, item.RemovalDate, item.OrganizationUid, result);
			return result;
		}

		public static FiresecAPI.Holiday Translate(Holiday item)
		{
			if (item == null)
				return null;
			var result = new FiresecAPI.Holiday
			{
				Name = item.Name,
				Date = item.Date,
				TransferDate = item.TransferDate,
				Reduction = item.Reduction,
				Type = (FiresecAPI.HolidayType)item.Type
			};
			TranslateOrganizationElement(item.Uid, item.IsDeleted, item.RemovalDate, item.OrganizationUid, result);
			return result;
		}

		public static FiresecAPI.SKDJournalItem Translate(Journal item)
		{
			if (item == null)
				return null;
			var result = new FiresecAPI.SKDJournalItem
			{
				Name = item.Name,
				Description = item.Description,
				SystemDateTime = item.SysemDate,
				DeviceDateTime = item.DeviceDate,
				CardNo = item.CardNo,
				CardSeries = item.CardSeries,
				CardUid = item.CardUid,
				DeviceJournalRecordNo = item.DeviceNo,
				IpAddress = item.IpPort
			};
			TranslateBase(item.Uid, item.IsDeleted, item.RemovalDate, result);
			return result;
		}

		public static FiresecAPI.Frame Translate(Frame item)
		{
			if (item == null)
				return null;
			var result = new FiresecAPI.Frame
			{
				CameraUid = item.CameraUid,
				DateTime = item.DateTime,
				FrameData = item.FrameData.ToArray(),
				JournalItemUid = item.JournalItemUid
			};
			TranslateBase(item.Uid, item.IsDeleted, item.RemovalDate, result);
			return result;
		}

		public static FiresecAPI.Organization Translate(Organization item)
		{
			if (item == null)
				return null;
			var result = new FiresecAPI.Organization
			{
				Name = item.Name,
				Description = item.Description
			};
			TranslateBase(item.Uid, item.IsDeleted, item.RemovalDate, result);
			return result;
		}

		static void TranslateBase<T>(Guid uid, bool isDeleted, DateTime removalDate, T apiItem)
			where T : FiresecAPI.SKDModelBase
		{
			apiItem.UID = uid;
			apiItem.IsDeleted = isDeleted;
			apiItem.RemovalDate = removalDate;
		}

		static void TranslateOrganizationElement<T>(Guid uid, bool isDeleted, DateTime removalDate, Guid? organizationUid, T apiItem)
			where T : FiresecAPI.OrganizationElementBase
		{
			TranslateBase(uid, isDeleted, removalDate, apiItem);
			apiItem.OrganizationUid = organizationUid;
		}
	}
}