﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecClient;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using XFiresecAPI;
using System.Collections.ObjectModel;

namespace GKModule.ViewModels
{
    public class NewDeviceViewModel : SaveCancelDialogViewModel
    {
        DeviceViewModel _parentDeviceViewModel;
        XDevice ParentDevice;

        public NewDeviceViewModel(DeviceViewModel parent)
        {
            Title = "Новое устройство";
            _parentDeviceViewModel = parent;
            ParentDevice = _parentDeviceViewModel.Device;

			Drivers = new ObservableCollection<XDriver>();
			foreach (var driver in XManager.DriversConfiguration.XDrivers)
			{
				if (driver.DriverType == XDriverType.AM1_O || driver.DriverType == XDriverType.AMP_1)
					continue;
				if (ParentDevice.Driver.Children.Contains(driver.DriverType))
					Drivers.Add(driver);

			}

			if(parent.Driver.DriverType == XDriverType.MPT)
				Drivers = new ObservableCollection<XDriver>(
					from XDriver driver in XManager.DriversConfiguration.XDrivers
					where driver.DriverType == XDriverType.MPT
					select driver);

			SelectedDriver = Drivers.FirstOrDefault();
            Count = 1;
        }

        public ObservableCollection<XDriver> Drivers { get; private set; }

        XDriver _selectedDriver;
        public XDriver SelectedDriver
        {
            get { return _selectedDriver; }
            set
            {
                _selectedDriver = value;
                UpdateAddressRange();
                OnPropertyChanged("SelectedDriver");
            }
        }


        XDevice _startDevice;
        public XDevice StartDevice
        {
            get { return _startDevice; }
            set
            {
                _startDevice = value;
                OnPropertyChanged("StartDevice");
            }
        }

        string _startAddress;
        public string StartAddress
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

        int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        void UpdateAddressRange()
        {
            int maxAddress = NewDeviceHelper.GetMinAddress(SelectedDriver, ParentDevice);

            StartDevice = new XDevice()
            {
                Driver = SelectedDriver,
                ShleifNo = (byte)(maxAddress / 256 + 1),
                IntAddress = (byte)(maxAddress % 256),
                Parent = ParentDevice
            };
            StartAddress = StartDevice.Address;
        }

        void CreateDevices()
        {
            var step = Math.Max(SelectedDriver.GroupDeviceChildrenCount, (byte)1);
            for (int i = StartDevice.IntAddress; i <= StartDevice.IntAddress + Count * step; i++)
            {
                if (ParentDevice.Children.Any(x => x.IntAddress == i && x.ShleifNo == StartDevice.ShleifNo))
                {
                    MessageBoxService.ShowWarning("В заданном диапазоне уже существуют устройства");
                    return;
                }
            }

            if (ParentDevice.Driver.IsGroupDevice)
            {
                Count = Math.Min(Count, ParentDevice.Driver.GroupDeviceChildrenCount);
            }

            byte shleifNo = StartDevice.ShleifNo;
            for (int i = 0; i < Count; i++)
            {
                var address = StartDevice.IntAddress + i * step;
                if (address + SelectedDriver.GroupDeviceChildrenCount >= 256)
                {
                    return;
                }

                XDevice device = XManager.AddChild(ParentDevice, SelectedDriver, shleifNo, (byte)address);
                NewDeviceHelper.AddDevice(device, _parentDeviceViewModel);
            }
        }

        protected override bool CanSave()
        {
            return (SelectedDriver != null);
        }

		protected override bool Save()
		{
            CreateDevices();
            _parentDeviceViewModel.Update();
            XManager.DeviceConfiguration.Update();
			return base.Save();
		}
    }
}