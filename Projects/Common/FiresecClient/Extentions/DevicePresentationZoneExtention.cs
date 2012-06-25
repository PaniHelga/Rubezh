﻿using System.Linq;
using FiresecAPI.Models;

namespace FiresecClient
{
	public static class DevicePresentationZoneExtention
	{
		public static string GetPersentationZone(this Device device)
		{
			if (device.Driver.IsZoneDevice)
			{
				var zone = FiresecManager.DeviceConfiguration.Zones.FirstOrDefault(x => x.No == device.ZoneNo);
				if (zone != null)
					return zone.PresentationName;
				return "";
			}

			if (device.Driver.IsZoneLogicDevice && device.ZoneLogic != null)
				return device.ZoneLogic.ToString();
			if (device.Driver.IsIndicatorDevice && device.IndicatorLogic != null)
				return device.IndicatorLogic.ToString();
			if ((device.Driver.DriverType == DriverType.PDUDirection) && (device.PDUGroupLogic != null))
				return device.PDUGroupLogic.ToString();

			return "";
		}
	}
}