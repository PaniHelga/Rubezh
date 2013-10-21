﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecClient;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using XFiresecAPI;

namespace GKModule.ViewModels
{
	public class NewDeviceViewModel : NewDeviceViewModelBase
	{
		public NewDeviceViewModel(DeviceViewModel deviceViewModel)
			: base(deviceViewModel)
		{
			var sortedDrivers = SortDrivers();
			foreach (var driver in sortedDrivers)
			{
				if (driver.DriverType == XDriverType.AMP_1)
					continue;
				if (ParentDevice.Driver.Children.Contains(driver.DriverType))
					Drivers.Add(driver);
			}

			var driverType = deviceViewModel.Driver.DriverType;
			if (driverType == XDriverType.MPT || driverType == XDriverType.MRO_2)
			{
				Drivers = new ObservableCollection<XDriver>(
					from XDriver driver in sortedDrivers
					where driver.DriverType == driverType
					select driver);
			}

			var parentShleif = ParentDevice;
			if (ParentDevice.Driver.DriverType == XDriverType.MPT || ParentDevice.Driver.DriverType == XDriverType.MRO_2)
				parentShleif = ParentDevice.Parent;
			if (parentShleif.Driver.DriverType == XDriverType.KAU_Shleif || parentShleif.Driver.DriverType == XDriverType.RSR2_KAU_Shleif)
			{
				SelectedShleif = parentShleif.IntAddress;
			}
			else
			{
				SelectedShleif = 1;
			}

			SelectedDriver = Drivers.FirstOrDefault();
		}

		byte SelectedShleif;

		XDriver _selectedDriver;
		public XDriver SelectedDriver
		{
			get { return _selectedDriver; }
			set
			{
				_selectedDriver = value;
				OnPropertyChanged("SelectedDriver");
				UpdateAddressRange();
			}
		}

		int _startAddress;
		public int StartAddress
		{
			get { return _startAddress; }
			set
			{
				if (_startAddress != value)
				{
					_startAddress = value;
					OnPropertyChanged("StartAddress");
				}
			}
		}

		void UpdateAddressRange()
		{
			int maxAddress = NewDeviceHelper.GetMinAddress(SelectedDriver, ParentDevice, SelectedShleif);
			StartAddress = (byte)(maxAddress % 256);
		}

		bool CreateDevices()
		{
			var step = Math.Max(SelectedDriver.GroupDeviceChildrenCount, (byte)1);

			for (int i = StartAddress; i < StartAddress + Count * step; i++)
			{
				if (ParentDevice.Children.Any(x => x.IntAddress == i && x.ShleifNo == SelectedShleif))
				{
					MessageBoxService.ShowWarning("В заданном диапазоне уже существуют устройства");
					return false;
				}
			}

			if (ParentDevice.Driver.IsGroupDevice)
			{
				Count = Math.Min(Count, ParentDevice.Driver.GroupDeviceChildrenCount);
			}

			for (int i = 0; i < Count; i++)
			{
				var address = StartAddress + i * step;
				if (address + SelectedDriver.GroupDeviceChildrenCount >= 256)
				{
					return true;
				}

				XDevice device = XManager.AddChild(ParentDevice, SelectedDriver, SelectedShleif, (byte)address);
				AddedDevice = NewDeviceHelper.AddDevice(device, ParentDeviceViewModel);
			}
			return true;
		}

		protected override bool CanSave()
		{
			return (SelectedDriver != null);
		}

		protected override bool Save()
		{
			var result = CreateDevices();
			if (result)
			{
				ParentDeviceViewModel.Update();
				XManager.DeviceConfiguration.Update();
			}
			return result;
		}
	}
}