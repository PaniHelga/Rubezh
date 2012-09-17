﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using FiresecAPI.Models;

namespace FiresecClient
{
	public partial class FiresecConfiguration
	{
		public DriversConfiguration DriversConfiguration
		{
			get { return ConfigurationCash.DriversConfiguration; }
			set { ConfigurationCash.DriversConfiguration = value; }
		}
		public DeviceConfiguration DeviceConfiguration
		{
			get { return ConfigurationCash.DeviceConfiguration; }
			set { ConfigurationCash.DeviceConfiguration = value; }
		}

		public void UpdateConfiguration()
		{
			if (DeviceConfiguration == null)
			{
				Logger.Error("FiresecConfiguration.UpdateConfiguration DeviceConfiguration = null");
				return;
			}

			DeviceConfiguration.Update();
            DeviceConfiguration.Reorder();

            foreach (var device in DeviceConfiguration.Devices)
            {
                device.Driver = DriversConfiguration.Drivers.FirstOrDefault(x => x.UID == device.DriverUID);
                if (device.Driver == null)
                {
                    Logger.Error("FiresecConfiguration.UpdateConfiguration device.Driver = null");
                    continue;
                }
            }

            InvalidateConfiguration();
			UpateGuardZoneSecPanelUID();

            foreach (var device in DeviceConfiguration.Devices)
            {
                device.HasExternalDevices = device.HaveExternalDevices();
            }
		}

        public void InvalidateConfiguration()
        {
            DeviceConfiguration.InvalidateConfiguration();
            DeviceConfiguration.UpdateCrossReferences();
        }

		public void CreateStates()
		{
			foreach (var device in DeviceConfiguration.Devices)
			{
				var deviceState = new DeviceState()
				{
					Device = device
				};
				foreach (var parameter in device.Driver.Parameters)
					deviceState.Parameters.Add(parameter.Copy());
				device.DeviceState = deviceState;
			}

			foreach (var zone in DeviceConfiguration.Zones)
			{
				var zoneState = new ZoneState()
				{
					Zone = zone,
				};
				zone.ZoneState = zoneState;
			}
		}

		public List<Zone> GetChannelZones(Device device)
		{
            DeviceConfiguration.UpdateCrossReferences();
			var zones = new List<Zone>();
			var channelDevice = device.ParentChannel;

			foreach (var zone in from zone in DeviceConfiguration.Zones orderby zone.No select zone)
			{
				if (channelDevice != null)
				{
					if (zone.DevicesInZone.All(x => ((x.ParentChannel != null) && (x.ParentChannel.UID == channelDevice.UID))))
						zones.Add(zone);
				}
				else
				{
					if (zone.DevicesInZone.All(x => (x.Parent.UID == device.UID)))
						zones.Add(zone);
				}
			}

			return zones;
		}

		public List<Zone> GetPanelZones(Device device)
		{
            DeviceConfiguration.UpdateCrossReferences();
			var zones = new List<Zone>();
			var channelDevice = device.ParentPanel;

			foreach (var zone in from zone in DeviceConfiguration.Zones orderby zone.No select zone)
			{
				if (channelDevice != null)
				{
					if (zone.DevicesInZone.All(x => ((x.ParentPanel != null) && (x.ParentPanel.UID == channelDevice.UID))))
						zones.Add(zone);
				}
				else
				{
					if (zone.DevicesInZone.All(x => (x.Parent.UID == device.UID)))
						zones.Add(zone);
				}
			}

			return zones;
		}

        public bool HasExternalDevices(Device device)
        {
            return device.HaveExternalDevices();
        }

		List<Device> allChildren;
		public List<Device> GetAllChildrenForDevice(Device device)
		{
			allChildren = new List<Device>();
			AddChild(device);
			return allChildren;
		}
		void AddChild(Device device)
		{
			foreach (var child in device.Children)
			{
				allChildren.Add(child);
				if (child.Driver.DriverType == DriverType.MPT)
				{
					AddChild(child);
				}
			}
		}

		public bool IsChildMPT(Device device)
		{
			if (device.Parent == null)
				return false;
			return ((device.Driver.DriverType == DriverType.MPT) && (device.Parent.Driver.DriverType == DriverType.MPT));
		}

		public void UpateGuardZoneSecPanelUID()
		{
			foreach (var zone in DeviceConfiguration.Zones)
			{
				zone.SecPanelUID = Guid.Empty;
				if (zone.ZoneType == ZoneType.Guard)
				{
					foreach (var device in zone.DevicesInZone)
					{
						if (device.Driver.DriverType == DriverType.AM1_O)
						{
							zone.SecPanelUID = device.Parent.UID;
							break;
						}
					}
				}
			}
		}

		public int GetZoneLocalSecNo(Zone zone)
		{
			if (zone.SecPanelUID != Guid.Empty)
			{
				var guardZones = (from guardZone in DeviceConfiguration.Zones
								  orderby guardZone.No
								  where guardZone.SecPanelUID == zone.SecPanelUID
								  select guardZone).ToList();
				return guardZones.IndexOf(zone) + 1;
			}
			return -1;
		}

		List<Zone> GetPanelLocalZones(Device device)
		{
			var zones = new List<Zone>();
			foreach (var child in GetPanelChildren(device))
			{
				if (child.Driver.IsZoneDevice)
				{
					if (child.ZoneNo.HasValue)
					{
						var zone = DeviceConfiguration.Zones.FirstOrDefault(x => x.No == child.ZoneNo);
						if (zone != null)
						{
							zones.Add(zone);
						}
					}
				}
			}
			return zones;
		}

		List<Device> panelChildren;
		List<Device> GetPanelChildren(Device device)
		{
			panelChildren = new List<Device>();
			AddPanelChildren(device);
			return panelChildren;
		}
		void AddPanelChildren(Device device)
		{
			foreach (var child in device.Children)
			{
				panelChildren.Add(child);
			}
		}

		public void SetEmptyConfiguration()
		{
			DeviceConfiguration = new DeviceConfiguration();

			var computerDriver = DriversConfiguration.Drivers.FirstOrDefault(x => x.DriverType == DriverType.Computer);
			if (computerDriver != null)
			{
				DeviceConfiguration.RootDevice = new Device()
				{
					DriverUID = computerDriver.UID,
					Driver = computerDriver
				};
				DeviceConfiguration.Update();
			}
			else
			{
				Logger.Error("FiresecConfiguration.SetEmptyConfiguration computerDriver = null");
			}
		}
	}
}