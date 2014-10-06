﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FiresecAPI.Automation
{
	[DataContract]
	public class RandomArguments
	{
		public RandomArguments()
		{
			MaxValueParameter = new Argument();
			ResultParameter = new Argument();
		}

		[DataMember]
		public Argument MaxValueParameter { get; set; }

		[DataMember]
		public Argument ResultParameter { get; set; }
	}
}