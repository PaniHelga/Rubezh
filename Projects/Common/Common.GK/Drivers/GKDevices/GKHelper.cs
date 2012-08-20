﻿using System;
using XFiresecAPI;

namespace Common.GK
{
	public static class GKHelper
	{
		public static XDriver Create()
		{
			var driver = new XDriver()
			{
				DriverTypeNo = 0x102,
				DriverType = XDriverType.GK,
				UID = new Guid("C052395D-043F-4590-A0B8-BC49867ADC6A"),
				Name = "Групповой контроллер",
				ShortName = "ГК",
				IsDeviceOnShleif = false,
				CanEditAddress = false,
				HasAddress = false,
				IsChildAddressReservedRange = false
			};
			driver.AutoCreateChildren.Add(XDriverType.GKIndicator);
			driver.AutoCreateChildren.Add(XDriverType.GKLine);
			driver.AutoCreateChildren.Add(XDriverType.GKRele);
			driver.Children.Add(XDriverType.KAU);

			driver.Properties.Add(
				new XDriverProperty()
				{
					Name = "IPAddress",
					Caption = "IP адрес",
					ToolTip = "IP адрес",
					StringDefault = "172.16.7.102",
					DriverPropertyType = XDriverPropertyTypeEnum.StringType,
					IsInternalDeviceParameter = false
				}
				);

			driver.Properties.Add(
				new XDriverProperty()
				{
					No = 0,
					Name = "Неответы",
					Caption = "Неответы",
					ToolTip = "Неответы",
					Default = 1,
					DriverPropertyType = XDriverPropertyTypeEnum.IntType,
					IsInternalDeviceParameter = true
				}
				);

			driver.Properties.Add(
				new XDriverProperty()
				{
					No = 1,
					Name = "Засыпание, мин",
					Caption = "Засыпание, мин",
					ToolTip = "Засыпание, мин",
					Default = 1,
					DriverPropertyType = XDriverPropertyTypeEnum.IntType,
					IsInternalDeviceParameter = true
				}
				);

			driver.Properties.Add(
				new XDriverProperty()
				{
					No = 2,
					Name = "Яркость, %",
					Caption = "Яркость, %",
					ToolTip = "Яркость, %",
					Default = 50,
					Min = 0,
					Max = 100,
					DriverPropertyType = XDriverPropertyTypeEnum.IntType,
					IsInternalDeviceParameter = true
				}
				);

			return driver;
		}
	}
}