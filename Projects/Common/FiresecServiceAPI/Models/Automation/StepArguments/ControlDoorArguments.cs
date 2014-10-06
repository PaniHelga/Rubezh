﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FiresecAPI.Automation
{
	[DataContract]
	public class ControlDoorArguments
	{
		public ControlDoorArguments()
		{
			DoorParameter = new Argument();
		}

		[DataMember]
		public Argument DoorParameter { get; set; }

		[DataMember]
		public DoorCommandType DoorCommandType { get; set; }
	}

	public enum DoorCommandType
	{
		[Description("Открыть дверь")]
		Open,

		[Description("Закрыть дверь")]
		Close,

		[Description("Установить режим ОТКРЫТО")]
		OpenForever,

		[Description("Установить режим ЗАКРЫТО")]
		CloseForever
	}
}