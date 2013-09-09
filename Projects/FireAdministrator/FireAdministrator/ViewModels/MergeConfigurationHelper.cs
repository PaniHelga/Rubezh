﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Infrastructure;
using FiresecClient;
using Infrastructure.Common.Windows;
using Infrastructure.Events;
using System.IO;
using Infrastructure.Common;
using Common;
using FiresecAPI.Models;
using Ionic.Zip;
using System.Diagnostics;
using XFiresecAPI;

namespace FireAdministrator.ViewModels
{
	public class MergeConfigurationHelper
	{
		public PlansConfiguration PlansConfiguration;
		public XDeviceConfiguration XDeviceConfiguration;

		public void Merge()
		{
			var openDialog = new OpenFileDialog()
			{
				Filter = "firesec2 files|*.fscp",
				DefaultExt = "firesec2 files|*.fscp"
			};
			if (openDialog.ShowDialog().Value)
			{
				ServiceFactory.Events.GetEvent<ConfigurationClosedEvent>().Publish(null);
				ZipConfigActualizeHelper.Actualize(openDialog.FileName, false);
				var folderName = AppDataFolderHelper.GetLocalFolder("Administrator/MergeConfiguration");
				var configFileName = Path.Combine(folderName, "Config.fscp");
				if (Directory.Exists(folderName))
					Directory.Delete(folderName, true);
				Directory.CreateDirectory(folderName);
				File.Copy(openDialog.FileName, configFileName);
				LoadFromZipFile(configFileName);
				ServiceFactory.ContentService.Invalidate();

				FiresecManager.UpdateConfiguration();

				ServiceFactory.Events.GetEvent<ConfigurationChangedEvent>().Publish(null);
				ServiceFactory.Layout.Close();
				if (ApplicationService.Modules.Any(x => x.Name == "Устройства, Зоны, Направления"))
					ServiceFactory.Events.GetEvent<ShowDeviceEvent>().Publish(Guid.Empty);
				else if (ApplicationService.Modules.Any(x => x.Name == "Групповой контроллер"))
					ServiceFactory.Events.GetEvent<ShowXDeviceEvent>().Publish(Guid.Empty);

				ServiceFactory.SaveService.FSChanged = true;
				ServiceFactory.SaveService.PlansChanged = true;
				ServiceFactory.SaveService.GKChanged = true;
				ServiceFactory.Layout.ShowFooter(null);
			}
		}

		public void LoadFromZipFile(string fileName)
		{
			var zipFile = ZipFile.Read(fileName, new ReadOptions { Encoding = Encoding.GetEncoding("cp866") });
			var fileInfo = new FileInfo(fileName);
			var unzipFolderPath = Path.Combine(fileInfo.Directory.FullName, "Unzip");
			zipFile.ExtractAll(unzipFolderPath);

			var zipConfigurationItemsCollectionFileName = Path.Combine(unzipFolderPath, "ZipConfigurationItemsCollection.xml");
			if (!File.Exists(zipConfigurationItemsCollectionFileName))
			{
				Logger.Error("FiresecManager.LoadFromZipFile zipConfigurationItemsCollectionFileName file not found");
				return;
			}
			var zipConfigurationItemsCollection = ZipSerializeHelper.DeSerialize<ZipConfigurationItemsCollection>(zipConfigurationItemsCollectionFileName);
			if (zipConfigurationItemsCollection == null)
			{
				Logger.Error("FiresecManager.LoadFromZipFile zipConfigurationItemsCollection == null");
				return;
			}

			foreach (var zipConfigurationItem in zipConfigurationItemsCollection.GetWellKnownZipConfigurationItems)
			{
				var configurationFileName = Path.Combine(unzipFolderPath, zipConfigurationItem.Name);
				if (File.Exists(configurationFileName))
				{
					switch (zipConfigurationItem.Name)
					{
						//case "DeviceConfiguration.xml":
						//    var deviceConfiguration = ZipSerializeHelper.DeSerialize<DeviceConfiguration>(configurationFileName);
						//    MergeDeviceConfiguration(deviceConfiguration);
						//    break;

						case "XDeviceConfiguration.xml":
							XDeviceConfiguration = ZipSerializeHelper.DeSerialize<XDeviceConfiguration>(configurationFileName);
							break;

						case "PlansConfiguration.xml":
							PlansConfiguration = ZipSerializeHelper.DeSerialize<PlansConfiguration>(configurationFileName);
							break;
					}
				}
			}

			var destinationImagesDirectory = AppDataFolderHelper.GetLocalFolder("Administrator/Configuration/Unzip/Content");
			var sourceImagesDirectoryInfo = new DirectoryInfo(Path.Combine(unzipFolderPath, "Content"));
			foreach (var imageFileInfo in sourceImagesDirectoryInfo.GetFiles())
			{
				imageFileInfo.CopyTo(Path.Combine(destinationImagesDirectory, imageFileInfo.Name), true);
			}

			zipFile.Dispose();

			MergeXDeviceConfiguration();
		}

		void MergeXDeviceConfiguration()
		{
			XDeviceConfiguration.Update();
			PlansConfiguration.Update();
			CreateNewUIDs();
			XDeviceConfiguration.Update();
			PlansConfiguration.Update();

			foreach (var device in XDeviceConfiguration.RootDevice.Children)
			{
				XManager.DeviceConfiguration.RootDevice.Children.Add(device);
			}
			foreach (var zone in XDeviceConfiguration.Zones)
			{
				XManager.Zones.Add(zone);
			}
			foreach (var direction in XDeviceConfiguration.Directions)
			{
				XManager.Directions.Add(direction);
			}

			foreach (var plan in PlansConfiguration.Plans)
			{
				FiresecManager.PlansConfiguration.Plans.Add(plan);
			}

			XManager.UpdateConfiguration();
			FiresecManager.UpdateConfiguration();
		}

		void CreateNewUIDs()
		{
			foreach (var device in XDeviceConfiguration.Devices)
			{
				var uid = Guid.NewGuid();
				DeviceUIDs.Add(device.UID, uid);
				device.UID = uid;
			}
			foreach (var zone in XDeviceConfiguration.Zones)
			{
				var uid = Guid.NewGuid();
				ZoneUIDs.Add(zone.UID, uid);
				zone.UID = uid;
			}
			foreach (var direction in XDeviceConfiguration.Directions)
			{
				var uid = Guid.NewGuid();
				DirectionUIDs.Add(direction.UID, uid);
				direction.UID = uid;
			}

			foreach (var device in XDeviceConfiguration.Devices)
			{
				for (int i = 0; i < device.ZoneUIDs.Count; i++)
				{
					var zoneUID = device.ZoneUIDs[i];
					device.ZoneUIDs[i] = ZoneUIDs[zoneUID];
				}
				foreach (var clause in device.DeviceLogic.Clauses)
				{
					for (int i = 0; i < clause.ZoneUIDs.Count; i++)
					{
						var zoneUID = clause.ZoneUIDs[i];
						clause.ZoneUIDs[i] = ZoneUIDs[zoneUID];
					}
					for (int i = 0; i < clause.DeviceUIDs.Count; i++)
					{
						var deviceUID = clause.DeviceUIDs[i];
						clause.ZoneUIDs[i] = DeviceUIDs[deviceUID];
					}
					for (int i = 0; i < clause.DirectionUIDs.Count; i++)
					{
						var directionUID = clause.DirectionUIDs[i];
						clause.ZoneUIDs[i] = DirectionUIDs[directionUID];
					}
				}
			}

			foreach (var direction in XDeviceConfiguration.Directions)
			{
				foreach (var directionZone in direction.DirectionZones)
				{
					var zoneUID = directionZone.ZoneUID;
					directionZone.ZoneUID = ZoneUIDs[zoneUID];
				}
				foreach (var directionDevice in direction.DirectionDevices)
				{
					var deviceUID = directionDevice.DeviceUID;
					directionDevice.DeviceUID = DeviceUIDs[deviceUID];
				}
			}

			foreach (var plan in PlansConfiguration.AllPlans)
			{
				foreach (var element in plan.ElementXDevices)
				{
					element.XDeviceUID = DeviceUIDs[element.XDeviceUID];
					var uid = Guid.NewGuid();
					PlenElementUIDs.Add(element.UID, uid);
					element.UID = uid;
				}
				foreach (var element in plan.ElementRectangleXZones)
				{
					element.ZoneUID = ZoneUIDs[element.ZoneUID];
					var uid = Guid.NewGuid();
					PlenElementUIDs.Add(element.UID, uid);
					element.UID = uid;
				}
				foreach (var element in plan.ElementPolygonXZones)
				{
					element.ZoneUID = ZoneUIDs[element.ZoneUID];
					var uid = Guid.NewGuid();
					PlenElementUIDs.Add(element.UID, uid);
					element.UID = uid;
				}
				foreach (var element in plan.ElementRectangleXDirections)
				{
					element.DirectionUID = DirectionUIDs[element.DirectionUID];
					var uid = Guid.NewGuid();
					PlenElementUIDs.Add(element.UID, uid);
					element.UID = uid;
				}
				foreach (var element in plan.ElementPolygonXDirections)
				{
					element.DirectionUID = DirectionUIDs[element.DirectionUID];
					var uid = Guid.NewGuid();
					PlenElementUIDs.Add(element.UID, uid);
					element.UID = uid;
				}
			}

			foreach (var device in XDeviceConfiguration.Devices)
			{
				var uids = new List<Guid>();
				foreach (var planElementUID in device.PlanElementUIDs)
				{
					uids.Add(PlenElementUIDs[planElementUID]);
				}
				device.PlanElementUIDs = uids;
			}
			//foreach (var zone in XDeviceConfiguration.Zones)
			//{
			//    var uids = new List<Guid>();
			//    foreach (var planElementUID in zone.PlanElementUIDs)
			//    {
			//        uids.Add(PlenElementUIDs[planElementUID]);
			//    }
			//    zone.PlanElementUIDs = uids;
			//}
			//foreach (var direction in XDeviceConfiguration.Directions)
			//{
			//    var uids = new List<Guid>();
			//    foreach (var planElementUID in direction.PlanElementUIDs)
			//    {
			//        uids.Add(PlenElementUIDs[planElementUID]);
			//    }
			//    direction.PlanElementUIDs = uids;
			//}

			foreach (var plan in PlansConfiguration.AllPlans)
			{
				plan.UID = Guid.NewGuid();
			}
		}

		Dictionary<Guid, Guid> DeviceUIDs = new Dictionary<Guid, Guid>();
		Dictionary<Guid, Guid> ZoneUIDs = new Dictionary<Guid, Guid>();
		Dictionary<Guid, Guid> DirectionUIDs = new Dictionary<Guid, Guid>();
		Dictionary<Guid, Guid> PlenElementUIDs = new Dictionary<Guid, Guid>();

		//void MergePlansConfiguration(PlansConfiguration plansConfiguration)
		//{
		//    plansConfiguration.Update();
		//    foreach (var plan in plansConfiguration.Plans)
		//    {
		//        FiresecManager.PlansConfiguration.Plans.Add(plan);
		//    }
		//}

		//void MergeDeviceConfiguration(DeviceConfiguration deviceConfiguration)
		//{
		//    deviceConfiguration.Update();
		//    foreach (var device in deviceConfiguration.Devices)
		//        device.Driver = FiresecManager.Drivers.FirstOrDefault(x => x.UID == device.DriverUID);
		//    foreach (var device in deviceConfiguration.Devices)
		//        if (device.Driver != null && (device.Driver.DriverType == DriverType.MS_1 || device.Driver.DriverType == DriverType.MS_2))
		//            AddDeviceTree(device);
		//    foreach (var device in deviceConfiguration.Devices)
		//        if (device.Driver != null && device.Driver.IsPanel)
		//            AddDeviceTree(device);
		//    var maxZoneNo = 0;
		//    if (FiresecManager.Zones.Count > 0)
		//        maxZoneNo = FiresecManager.Zones.Max(x => x.No);
		//    var zoneMap = new List<Guid>();
		//    FiresecManager.Zones.ForEach(zone => zoneMap.Add(zone.UID));
		//    foreach (var zone in deviceConfiguration.Zones)
		//        if (!zoneMap.Contains(zone.UID))
		//        {
		//            zone.No += maxZoneNo;
		//            FiresecManager.Zones.Add(zone);
		//        }
		//}

		//void AddDeviceTree(Device device)
		//{
		//    var hasDevice = FiresecManager.Devices.Any(x => x.GetId() == device.GetId());
		//    if (!hasDevice)
		//    {
		//        var existingParent = FiresecManager.Devices.FirstOrDefault(x => x.GetId() == device.Parent.GetId());
		//        if (existingParent != null)
		//        {
		//            existingParent.Children.Add(device);
		//        }
		//    }
		//}

	}
}