﻿using System;
using System.Linq;
using System.Windows.Media;
using FiresecAPI.Models;
using FiresecClient;
using Infrustructure.Plans.Elements;
using XFiresecAPI;
using System.Collections.Generic;

namespace GKModule.Plans.Designer
{
	internal static class Helper
	{
		private static Dictionary<Guid, XZone> _xzoneMap;
		private static Dictionary<Guid, XDevice> _xdeviceMap;
		private static Dictionary<Guid, XDirection> _xdirectionMap;
		public static void BuildMap()
		{
			_xzoneMap = new Dictionary<Guid, XZone>();
			XManager.DeviceConfiguration.Zones.ForEach(item => _xzoneMap.Add(item.UID, item));
			_xdeviceMap = new Dictionary<Guid, XDevice>();
			XManager.DeviceConfiguration.Devices.ForEach(item => _xdeviceMap.Add(item.UID, item));
			_xdirectionMap = new Dictionary<Guid, XDirection>();
			XManager.DeviceConfiguration.Directions.ForEach(item => _xdirectionMap.Add(item.UID, item));
		}
		
		public static string GetXZoneTitle(IElementZone element)
		{
			XZone xzone = GetXZone(element);
			return xzone == null ? "Несвязанная зона" : xzone.PresentationName;
		}
		public static XZone GetXZone(IElementZone element)
		{
			return GetXZone(element.ZoneUID);
		}
		public static XZone GetXZone(Guid xzoneUID)
		{
			return xzoneUID != Guid.Empty && _xzoneMap.ContainsKey(xzoneUID) ? _xzoneMap[xzoneUID] : null;
		}
		public static void SetXZone(IElementZone element)
		{
			XZone zone = GetXZone(element);
			element.BackgroundColor = GetXZoneColor(zone);
		}
		public static void SetXZone(IElementZone element, XZone xzone)
		{
			element.ZoneUID = xzone == null ? Guid.Empty : xzone.UID;
			element.BackgroundColor = GetXZoneColor(xzone);
		}
		public static Color GetXZoneColor(XZone zone)
		{
			Color color = Colors.Black;
			if (zone != null)
				color = Colors.Green;
			return color;
		}

		public static string GetXDirectionTitle(ElementXDirection element)
		{
			var xdirection = GetXDirection(element);
			return xdirection == null ? "Несвязанная зона" : xdirection.PresentationName;
		}
		public static XDirection GetXDirection(ElementXDirection element)
		{
			return GetXDirection(element.DirectionUID);
		}
		public static XDirection GetXDirection(Guid xdirectionUID)
		{
			return xdirectionUID != Guid.Empty && _xdirectionMap.ContainsKey(xdirectionUID) ? _xdirectionMap[xdirectionUID] : null;
		}
		public static void SetXDirection(ElementXDirection element)
		{
			var xdirection = GetXDirection(element);
			element.BackgroundColor = GetXDirectionColor(xdirection);
		}
		public static void SetXDirection(ElementXDirection element, XDirection xdirection)
		{
			element.DirectionUID = xdirection == null ? Guid.Empty : xdirection.UID;
			element.BackgroundColor = GetXDirectionColor(xdirection);
		}
		public static Color GetXDirectionColor(XDirection xdirection)
		{
			Color color = Colors.Black;
			if (xdirection != null)
				color = Colors.LightBlue;
			return color;
		}

		public static XDevice GetXDevice(ElementXDevice element)
		{
			return element.XDeviceUID != Guid.Empty && _xdeviceMap.ContainsKey(element.XDeviceUID) ? _xdeviceMap[element.XDeviceUID] : null;
		}
		public static string GetXDeviceTitle(ElementXDevice element)
		{
			var device = GetXDevice(element);
			return device == null ? "Неизвестное устройство" : device.DottedAddress + " " + device.Driver.ShortName;
		}
		public static XDevice SetXDevice(ElementXDevice element)
		{
			XDevice device = GetXDevice(element);
			if (device != null)
				device.PlanElementUIDs.Add(element.UID);
			return device;
		}
		public static void SetXDevice(ElementXDevice element, XDevice device)
		{
			ResetXDevice(element);
			element.XDeviceUID = device == null ? Guid.Empty : device.UID;
			if (device != null)
				device.PlanElementUIDs.Add(element.UID);
		}
		public static void ResetXDevice(ElementXDevice element)
		{
			XDevice device = GetXDevice(element);
			if (device != null)
				device.PlanElementUIDs.Remove(element.UID);
		}
	}
}