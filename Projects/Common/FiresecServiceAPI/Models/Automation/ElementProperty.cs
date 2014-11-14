﻿using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace FiresecAPI.Automation
{
	public class ElementProperty
	{
		public ElementProperty()
		{
			ElementUid = new Guid();
			ValueArgument = new Argument();
		}

		[DataMember]
		public Guid ElementUid { get; set; }

		[DataMember]
		public ExplicitType ExplicitType { get; set; }

		[DataMember]
		public ObjectType ObjectType { get; set; }

		[DataMember]
		public ElementPropertyType ElementPropertyType { get; set; }

		[DataMember]
		public Argument ValueArgument { get; set; }
	}

}