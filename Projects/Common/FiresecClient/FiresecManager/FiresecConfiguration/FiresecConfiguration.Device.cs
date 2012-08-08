﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiresecAPI.Models;

namespace FiresecClient
{
	public partial class FiresecConfiguration
	{
		public Device CopyDevice(Device device, bool fullCopy)
		{
			var newDevice = new Device()
			{
				DriverUID = device.DriverUID,
				Driver = device.Driver,
				IntAddress = device.IntAddress,
				Description = device.Description,
				ZoneNo = device.ZoneNo
			};

			if (fullCopy)
			{
				newDevice.UID = device.UID;
				newDevice.DatabaseId = device.DatabaseId;
			}

			if (device.ZoneLogic != null)
			{
				newDevice.ZoneLogic = new ZoneLogic();
				newDevice.ZoneLogic.JoinOperator = device.ZoneLogic.JoinOperator;
				foreach (var clause in device.ZoneLogic.Clauses)
				{
					newDevice.ZoneLogic.Clauses.Add(new Clause()
					{
						State = clause.State,
						Operation = clause.Operation,
						Zones = clause.Zones.ToList()
					});
				}
			}

			newDevice.Properties = new List<Property>();
			foreach (var property in device.Properties)
			{
				newDevice.Properties.Add(new Property()
				{
					Name = property.Name,
					Value = property.Value
				});
			}

			newDevice.Children = new List<Device>();
			foreach (var childDevice in device.Children)
			{
				var newChildDevice = CopyDevice(childDevice, fullCopy);
				newChildDevice.Parent = newDevice;
				newDevice.Children.Add(newChildDevice);
			}

			return newDevice;
		}

		public Device AddChild(Device parentDevice, Driver newDriver, int newAddress)
		{
			var device = new Device()
			{
				DriverUID = newDriver.UID,
				Driver = newDriver,
				IntAddress = newAddress,
				Parent = parentDevice
			};
			parentDevice.Children.Add(device);
			AddAutoCreateChildren(device);

			return device;
		}

		void AddAutoCreateChildren(Device device)
		{
			foreach (var autoCreateDriverId in device.Driver.AutoCreateChildren)
			{
				var autoCreateDriver = Drivers.FirstOrDefault(x => x.UID == autoCreateDriverId);

				for (int i = autoCreateDriver.MinAutoCreateAddress; i <= autoCreateDriver.MaxAutoCreateAddress; i++)
				{
					AddChild(device, autoCreateDriver, i);
				}
			}
		}

		public void SynchronizeChildern(Device device)
		{
			for (int i = device.Children.Count(); i > 0; i--)
			{
				var childDevice = device.Children[i - 1];

				if (device.Driver.AvaliableChildren.Contains(childDevice.Driver.UID) == false)
				{
					device.Children.RemoveAt(i - 1);
				}
			}

			foreach (var autoCreateChildUID in device.Driver.AutoCreateChildren)
			{
				var autoCreateDriver = Drivers.FirstOrDefault(x => x.UID == autoCreateChildUID);

				for (int i = autoCreateDriver.MinAutoCreateAddress; i <= autoCreateDriver.MaxAutoCreateAddress; i++)
				{
					var newDevice = new Device()
					{
						DriverUID = autoCreateDriver.UID,
						Driver = autoCreateDriver,
						IntAddress = i
					};
					if (device.Children.Any(x => x.Driver.DriverType == newDevice.Driver.DriverType && x.IntAddress == newDevice.IntAddress) == false)
					{
						device.Children.Add(newDevice);
						newDevice.Parent = device;
					}
				}
			}
		}
	}
}