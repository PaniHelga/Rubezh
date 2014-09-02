﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FiresecAPI.GK
{
	[DataContract]
	public class XGuardZoneDevice
	{
		public XGuardZoneDevice()
		{
			CodeReaderSettings = new XCodeReaderSettings();
		}

		public XDevice Device { get; set; }

		[DataMember]
		public Guid DeviceUID { get; set; }

		[DataMember]
		public XGuardZoneDeviceActionType ActionType { get; set; }

		[DataMember]
		public XCodeReaderSettings CodeReaderSettings { get; set; }
	}

	public enum XGuardZoneDeviceActionType
	{
		[Description("Поставить на охрану")]
		SetGuard,

		[Description("Снять с охраны")]
		ResetGuard,

		[Description("Вызвать тревогу")]
		SetAlarm
	}
}