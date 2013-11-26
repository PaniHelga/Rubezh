﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common;
//using Infrastructure.Common.Windows;
using XFiresecAPI;

namespace FiresecClient
{
	public partial class XManager
	{
		public static XDeviceConfiguration DeviceConfiguration { get; set; }
		public static XDriversConfiguration DriversConfiguration { get; set; }
        public static XDeviceLibraryConfiguration DeviceLibraryConfiguration { get; set; }

		static XManager()
		{
			DeviceConfiguration = new XDeviceConfiguration();
			DriversConfiguration = new XDriversConfiguration();
		}

		public static List<XDevice> Devices
		{
			get { return DeviceConfiguration.Devices; }
		}
		public static List<XZone> Zones
		{
			get { return DeviceConfiguration.Zones; }
		}
		public static List<XDirection> Directions
		{
			get { return DeviceConfiguration.Directions; }
		}
		public static List<XDriver> Drivers
		{
			get { return DriversConfiguration.XDrivers; }
		}
		public static List<XParameterTemplate> ParameterTemplates
		{
			get { return XManager.DeviceConfiguration.ParameterTemplates; }
		}

		public static void SetEmptyConfiguration()
		{
			DeviceConfiguration = new XDeviceConfiguration();
			UpdateConfiguration();
		}

		public static void UpdateConfiguration()
		{
			if (DeviceConfiguration == null)
			{
				DeviceConfiguration = new XDeviceConfiguration();
			}
			if (DeviceConfiguration.RootDevice == null)
			{
				var systemDriver = Drivers.FirstOrDefault(x => x.DriverType == XDriverType.System);
				if (systemDriver != null)
				{
					DeviceConfiguration.RootDevice = new XDevice()
					{
						DriverUID = systemDriver.UID,
						Driver = systemDriver
					};
				}
				else
				{
					Logger.Error("XManager.SetEmptyConfiguration systemDriver = null");
				}
			}
			DeviceConfiguration.ValidateVersion();

			DeviceConfiguration.Update();
			foreach (var device in DeviceConfiguration.Devices)
			{
				device.Driver = Drivers.FirstOrDefault(x => x.UID == device.DriverUID);
				if (device.Driver == null)
				{
					//MessageBoxService.Show("Ошибка при сопоставлении драйвера устройств ГК");
				}
			}
			DeviceConfiguration.Reorder();

			InitializeProperties();
            Invalidate();
		}

		public static void InitializeProperties()
		{
			foreach (var device in Devices)
			{
				foreach (var property in device.Properties)
				{
					property.DriverProperty = device.Driver.Properties.FirstOrDefault(x => x.Name == property.Name);
				}
				foreach (var property in device.DeviceProperties)
				{
					property.DriverProperty = device.Driver.Properties.FirstOrDefault(x => x.Name == property.Name);
				}
				device.InitializeDefaultProperties();
			}
		}

		public static ushort GetKauLine(XDevice device)
		{
			if (!device.Driver.IsKauOrRSR2Kau)
			{
				throw new Exception("В XManager.GetKauLine передан неверный тип устройства");
			}

			ushort lineNo = 0;
			var modeProperty = device.Properties.FirstOrDefault(x => x.Name == "Mode");
			if (modeProperty != null)
			{
				return modeProperty.Value;
			}
			return lineNo;
		}

		public static string GetIpAddress(XDevice device)
		{
			XDevice gkDevice = null;
            switch (device.DriverType)
            {
                case XDriverType.GK:
                    gkDevice = device;
                    break;

                case XDriverType.KAU:
				case XDriverType.RSR2_KAU:
                    gkDevice = device.Parent;
                    break;

                default:
                    {
                        Logger.Error("XManager.GetIpAddress Получить IP адрес можно только у ГК или в КАУ");
                        throw new Exception("Получить IP адрес можно только у ГК или в КАУ");
                    }
            }
			var ipAddress = gkDevice.GetGKIpAddress();
			return ipAddress;
		}

		public static bool IsManyGK()
		{
			return DeviceConfiguration.Devices.Where(x => x.DriverType == XDriverType.GK).Count() > 1;
		}
	}
}