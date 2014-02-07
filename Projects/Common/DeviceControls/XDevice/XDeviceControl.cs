﻿using System.Collections.Generic;
using System.Linq;
using Common;
using FiresecClient;
using Infrustructure.Plans.Devices;
using XFiresecAPI;

namespace DeviceControls.XDevice
{
	public class XDeviceControl : BaseDeviceControl<LibraryXFrame, XStateClass>
	{
		public XStateClass StateClass { get; set; }

		protected override IEnumerable<ILibraryState<LibraryXFrame, XStateClass>> GetStates()
		{
			var libraryXDevice = XManager.DeviceLibraryConfiguration.XDevices.FirstOrDefault(x => x.XDriverId == DriverId);
			if (libraryXDevice == null)
			{
				Logger.Error("XDeviceControl.Update libraryXDevice = null " + DriverId.ToString());
				return null;
			}

			var libraryState = libraryXDevice.XStates.FirstOrDefault(x => x.Code == null && x.XStateClass == StateClass);
			if (libraryState == null)
			{
				libraryState = libraryXDevice.XStates.FirstOrDefault(x => x.Code == null && x.XStateClass == XStateClass.No);
				if (libraryState == null)
				{
					Logger.Error("XDeviceControl.Update libraryState = null " + DriverId.ToString());
					return null;
				}
			}

			var resultLibraryStates = new List<ILibraryState<LibraryXFrame, XStateClass>>();
			if (libraryState != null)
				resultLibraryStates.Add(libraryState);
			return resultLibraryStates;
		}
	}
}