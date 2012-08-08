﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace XFiresecAPI
{
	[DataContract]
	public class XDirection : XBinaryBase
	{
		public XDirection()
		{
			Zones = new List<int>();
			DirectionDevices = new List<DirectionDevice>();
			XZones = new List<XZone>();
			Regime = 1;
		}

		public List<XZone> XZones { get; set; }

		[DataMember]
		public short No { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public short Delay { get; set; }

		[DataMember]
		public short Hold { get; set; }

		[DataMember]
		public short Regime { get; set; }

		[DataMember]
		public List<int> Zones { get; set; }

		[DataMember]
		public List<DirectionDevice> DirectionDevices { get; set; }

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
	}
}