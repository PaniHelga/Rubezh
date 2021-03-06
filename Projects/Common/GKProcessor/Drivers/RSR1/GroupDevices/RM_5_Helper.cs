﻿using System;
using FiresecAPI.GK;

namespace GKProcessor
{
	public static class RM_5_Helper
	{
		public static GKDriver Create()
		{
			var driver = new GKDriver()
			{
				DriverType = GKDriverType.RM_5,
				UID = new Guid("a7c09ba8-dd00-484c-8bea-245f2920dfbb"),
				Name = "РМ-5",
				ShortName = "РМ-5",
				IsGroupDevice = true,
				GroupDeviceChildType = GKDriverType.RM_1,
				GroupDeviceChildrenCount = 5,
				IsIgnored = true,
			};
			return driver;
		}
	}
}