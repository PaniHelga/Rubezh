﻿using System.Collections.Generic;
using XFiresecAPI;

namespace Commom.GK
{
	public class GkDatabase : CommonDatabase
	{
		public GkDatabase(XDevice gkDevice)
		{
			DatabaseType = DatabaseType.Gk;
			RootDevice = gkDevice;

			AddDevice(gkDevice);

			foreach (var device in gkDevice.Children)
			{
				if (device.Driver.DriverType == XDriverType.GKIndicator)
				{
					AddDevice(device);
				}
			}

			foreach (var device in gkDevice.Children)
			{
				if (device.Driver.DriverType == XDriverType.GKLine)
				{
					AddDevice(device);
				}
			}

			foreach (var device in gkDevice.Children)
			{
				if (device.Driver.DriverType == XDriverType.GKRele)
				{
					AddDevice(device);
				}
			}

			//foreach (var device in gkDevice.Children)
			//{
			//    if (device.Driver.DriverType == XDriverType.KAU)
			//    {
			//        AddDevice(device);
			//    }
			//}
		}

		public List<byte> GetBytes()
		{
			var bytes = new List<byte>();
			foreach (var binaryObject in BinaryObjects)
			{
				bytes.AddRange(binaryObject.AllBytes);
			}
			return bytes;
		}
	}
}