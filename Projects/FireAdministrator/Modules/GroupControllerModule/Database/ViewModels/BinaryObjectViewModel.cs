﻿using GKModule.Converter;
using Infrastructure.Common;
using XFiresecAPI;
using GKModule.Database;
using FiresecClient;
using System.Linq;

namespace GKModule.ViewModels
{
	public class BinaryObjectViewModel : BaseViewModel
	{
		public BinaryObjectViewModel(BinaryObjectBase binaryObject)
		{
			BinaryObject = binaryObject;
			if (binaryObject.Device != null)
			{
				Name = binaryObject.Device.Driver.ShortName;
				Address = binaryObject.Device.Address;
				ImageSource = binaryObject.Device.Driver.ImageSource;
			}
			if (binaryObject.Zone != null)
			{
				Name = binaryObject.Zone.Name;
				Address = binaryObject.Zone.No.ToString();
				ImageSource = XManager.DriversConfiguration.Drivers.FirstOrDefault(x => x.DriverType == XDriverType.System).ImageSource;
			}
		}

		public BinaryObjectBase BinaryObject { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string ImageSource { get; set; }
	}
}