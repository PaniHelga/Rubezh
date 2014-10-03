﻿using System.Collections.Generic;
using FiresecAPI.GK;

namespace GKProcessor
{
	public abstract class CommonDatabase
	{
		ushort currentChildNo = 1;
		protected ushort NextDescriptorNo
		{
			get { return currentChildNo++; }
		}

		public DatabaseType DatabaseType { get; protected set; }
		public GKDevice RootDevice { get; protected set; }
		public List<BaseDescriptor> Descriptors { get; set; }

		public CommonDatabase()
		{
			Descriptors = new List<BaseDescriptor>();
		}

		public abstract void BuildObjects();
	}
}