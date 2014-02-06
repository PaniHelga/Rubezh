﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKDDriver.DataAccess;

namespace SKDDriver
{
	static partial class Translator
	{
		public static Position TranslateBack(FiresecAPI.Position position)
		{
			if (position == null)
				return null;
			return new Position
			{
				Uid = position.Uid,
				Name = position.Name,
				Description = position.Description
			};
		}

		public static Department TranslateBack(FiresecAPI.Department department)
		{
			if (department == null)
				return null;
			return new Department
			{
				Uid = department.Uid,
				Name = department.Name,
				Description = department.Description,
				ParentDepartmentUid = department.ParentDepartmentUid,
				ContactEmployeeUid = department.ContactEmployeeUid,
				AttendantUid = department.AttendantEmployeeUId,
			};
		}

		public static Employee TranslateBack(FiresecAPI.Employee employee)
		{
			if (employee == null)
				return null;
			return new Employee
			{
				Uid = employee.Uid,
				FirstName = employee.FirstName,
				SecondName = employee.SecondName,
				LastName = employee.LastName,
				Appointed = employee.Appointed,
				Dismissed = employee.Dismissed,
				PositionUid = employee.PositionUid,
				DepartmentUid = employee.DepartmentUid,
				ScheduleUid = employee.ScheduleUid,
			};
		}
		
		public static Journal TranslateBack(FiresecAPI.SKDJournalItem journalItem)
		{
			if (journalItem == null)
				return null;
			return new Journal
			{
				Uid = journalItem.Uid,
				CardNo = journalItem.CardNo,
				Description = journalItem.Description,
				DeviceDate = journalItem.DeviceDateTime,
				DeviceNo = journalItem.DeviceJournalRecordNo,
				IpPort = journalItem.IpAddress,
				Name = journalItem.Name,
				SysemDate = journalItem.SystemDateTime
			};
		}

		public static Frame TranslateBack(FiresecAPI.Frame frame)
		{
			if (frame == null)
				return null;
			return new Frame
			{
				Uid = frame.Uid,
				CameraUid = frame.CameraUid,
				DateTime = frame.DateTime,
				FrameData = frame.FrameData,
				JournalItemUid = frame.JournalItemUid
			};
		}

		public static Card TranslateBack(FiresecAPI.SKDCard card)
		{
			if (card == null)
				return null;
			return new Card
			{
				EmployeeUid = card.EmployeeUid,
				Number = card.Number,
				Series = card.Series,
				Uid = card.Uid,
				ValidFrom = card.ValidFrom,
				ValidTo = card.ValidTo
			};
		}
		
		public static CardZoneLink TranslateBack(FiresecAPI.CardZoneLink cardZoneLink)
		{
			if (cardZoneLink == null)
				return null;
			return new CardZoneLink
			{
				Uid = cardZoneLink.Uid,
				AccessType = cardZoneLink.AccessType.ToString(),
				IsAntipass = cardZoneLink.IsAntipass,
				ZoneUid = cardZoneLink.ZoneUid,
				CardUid = cardZoneLink.CardUid,
				TimeCriteriaUid = cardZoneLink.TimeCriteriaUid
			};
		}
	}
}
