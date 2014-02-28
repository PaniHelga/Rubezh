﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FiresecAPI
{
	[DataContract]
	public class EmployeeShedulePart
	{
		public EmployeeShedulePart()
		{
			UID = Guid.NewGuid();
		}

		[DataMember]
		public Guid UID { get; set; }

		[DataMember]
		public Guid ZoneUID { get; set; }

		[DataMember]
		public bool IsControl { get; set; }
	}
}