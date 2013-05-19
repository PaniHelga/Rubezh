﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFiresecAPI
{
	[DataContract]
	public class XDirection : XBinaryBase
	{
		public XDirection()
		{
            UID = Guid.NewGuid();
            DirectionZones = new List<XDirectionZone>();
            DirectionDevices = new List<XDirectionDevice>();
			Regime = 1;
			InputZones = new List<XZone>();
			InputDevices = new List<XDevice>();
            OutputDevices = new List<XDevice>();
			PlanElementUIDs = new List<Guid>();
		}

		public XDirectionState DirectionState { get; set; }
		public override XBaseState GetXBaseState() { return DirectionState; }
		public List<XZone> InputZones { get; set; }
		public List<XDevice> InputDevices { get; set; }
        public List<XDevice> OutputDevices { get; set; }

		[DataMember]
		public Guid UID { get; set; }

		[DataMember]
		public ushort No { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public ushort Delay { get; set; }

		[DataMember]
		public ushort Hold { get; set; }

		[DataMember]
		public ushort Regime { get; set; }

		[DataMember]
        public List<XDirectionZone> DirectionZones { get; set; }

		[DataMember]
        public List<XDirectionDevice> DirectionDevices { get; set; }

		public string PresentationName
		{
			get { return Name + " - " + No.ToString(); }
		}

		public override XBinaryInfo BinaryInfo
		{
			get
			{
				return new XBinaryInfo()
				{
					Type = "Направление",
					Name = Name,
					Address = No.ToString()
				};
			}
		}

		public override string GetBinaryDescription()
		{
			return Name + " - " + No.ToString();
		}

		public void OnChanged()
		{
			if (Changed != null)
				Changed();
		}
		public event Action Changed;
		public List<Guid> PlanElementUIDs { get; set; }
	}
}