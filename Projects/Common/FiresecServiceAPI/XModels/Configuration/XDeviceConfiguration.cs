﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FiresecAPI;

namespace XFiresecAPI
{
    [DataContract]
    public class XDeviceConfiguration : VersionedConfiguration
    {
        public XDeviceConfiguration()
        {
            Devices = new List<XDevice>();
            Zones = new List<XZone>();
            Directions = new List<XDirection>();
            JournalFilters = new List<XJournalFilter>();
            Instructions = new List<XInstruction>();
        }

        public List<XDevice> Devices { get; set; }

        [DataMember]
        public XDevice RootDevice { get; set; }

        [DataMember]
        public List<XZone> Zones { get; set; }

        [DataMember]
        public List<XDirection> Directions { get; set; }

        [DataMember]
        public List<XJournalFilter> JournalFilters { get; set; }

        [DataMember]
        public List<XInstruction> Instructions { get; set; }

        public void Update()
        {
            Devices = new List<XDevice>();
            if (RootDevice != null)
            {
                RootDevice.Parent = null;
                Devices.Add(RootDevice);
                AddChild(RootDevice);
            }
        }

        void AddChild(XDevice parentDevice)
        {
            foreach (var device in parentDevice.Children)
            {
                device.Parent = parentDevice;
                Devices.Add(device);
                AddChild(device);
            }
        }

		public List<XZone> SortedZones
		{
			get
			{
				return(
				from XZone zone in Zones
				orderby zone.No
				select zone).ToList();
			}
		}

		public void Reorder()
		{
			foreach (var device in Devices)
			{
				device.SynchronizeChildern();
			}
		}

        public override bool ValidateVersion()
        {
			bool result = true;

			if (RootDevice == null)
			{
				var device = new XDevice();
				device.DriverUID = new Guid("938947C5-4624-4A1A-939C-60AEEBF7B65C");
				RootDevice = device;
				result = false;
			}

            Update();

			if (JournalFilters == null)
			{
				JournalFilters = new List<XJournalFilter>();
				result = false;
			}
            foreach (var journalFilter in JournalFilters)
            {
                if(journalFilter.EventNames == null)
                {
                    journalFilter.EventNames = new List<string>();
                    result = false;
                }
            }

			if (Devices == null)
			{
				Devices = new List<XDevice>();
				result = false;
			}
			if (Zones == null)
			{
				Zones = new List<XZone>();
				result = false;
			}
			if (Directions == null)
			{
				Directions = new List<XDirection>();
				result = false;
			}

			foreach (var device in Devices)
			{
				if (device.ZoneUIDs == null)
				{
					device.ZoneUIDs = new List<Guid>();
					result = false;
				}

				if (device.DeviceLogic == null)
				{
					device.DeviceLogic = new XDeviceLogic();
					result = false;
				}
				if (device.DeviceLogic.Clauses == null)
				{
					device.DeviceLogic.Clauses = new List<XClause>();
					result = false;
				}
				foreach (var clause in device.DeviceLogic.Clauses)
				{
					if (clause.ZoneUIDs == null)
					{
						clause.ZoneUIDs = new List<Guid>();
						result = false;
					}

					if (clause.DeviceUIDs == null)
					{
						clause.DeviceUIDs = new List<Guid>();
						result = false;
					}

					if (clause.DirectionUIDs == null)
					{
						clause.DirectionUIDs = new List<Guid>();
						result = false;
					}
				}
			}
			foreach (var zone in Zones)
			{
			}
			foreach (var direction in Directions)
			{
				if (direction.DirectionZones == null)
				{
                    direction.DirectionZones = new List<XDirectionZone>();
					result = false;
				}

				if (direction.DirectionDevices == null)
				{
                    direction.DirectionDevices = new List<XDirectionDevice>();
					result = false;
				}
			}

            foreach (var journalFilter in JournalFilters)
            {
                if(journalFilter.StateClasses == null)
                {
                    journalFilter.StateClasses = new List<XStateClass>();
                    result = false;
                }
            }

            if (Instructions == null)
            {
                Instructions = new List<XInstruction>();
                result = false;
            }

			return result;
        }
    }
}