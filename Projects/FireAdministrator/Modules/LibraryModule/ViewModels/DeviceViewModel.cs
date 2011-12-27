﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;

namespace LibraryModule.ViewModels
{
    public class DeviceViewModel : BaseViewModel
    {
        public static Driver UnknownDeviceDriver
        {
            get
            {
                return new Driver()
                {
                    ImageSource = FileHelper.GetIconFilePath("Unknown_Device") + ".ico",
                    UID = new Guid("00000000-0000-0000-0000-000000000000"),
                    Name = "Неизвестное устройство",
                    States = new List<DriverState>(),
                };
            }
        }
        readonly Driver _driver;

        public DeviceViewModel(LibraryDevice device)
        {
            Device = device;
            _driver = FiresecManager.Drivers.FirstOrDefault(x => x.UID == Device.DriverId);
            if (_driver == null)
            {
                _driver = UnknownDeviceDriver;
                Device.DriverId = _driver.UID;
                Device.States = null;
                StateViewModels = new ObservableCollection<StateViewModel>();
            }
            else
            {
                if (Device.States == null) SetDefaultStateTo(Device);

                StateViewModels = new ObservableCollection<StateViewModel>(
                    Device.States.Select(state => new StateViewModel(state, _driver))
                );
            }

            AddStateCommand = new RelayCommand(OnAddState);
            AddAdditionalStateCommand = new RelayCommand(OnAddAdditionalState);
            RemoveStateCommand = new RelayCommand(OnRemoveState, CanRemoveState);
        }

        public LibraryDevice Device { get; private set; }

        public string Name
        {
            get { return _driver.Name; }
        }

        public string ImageSource
        {
            get { return _driver.ImageSource; }
        }

        public Guid Id
        {
            get { return Device.DriverId; }
        }

        public ObservableCollection<StateViewModel> StateViewModels { get; private set; }

        StateViewModel _selectedStateViewModel;
        public StateViewModel SelectedStateViewModel
        {
            get { return _selectedStateViewModel; }
            set
            {
                _selectedStateViewModel = value;
                OnPropertyChanged("SelectedStateViewModel");
                OnPropertyChanged("DeviceControl");
            }
        }

        public DeviceControls.DeviceControl DeviceControl
        {
            get
            {
                if (SelectedStateViewModel == null)
                    return null;

                var deviceControl = new DeviceControls.DeviceControl();
                deviceControl.DriverId = Device.DriverId;

                var additionalStateCodes = new List<string>();
                if (SelectedStateViewModel.IsAdditional)
                {
                    additionalStateCodes.Add(SelectedStateViewModel.State.Code);
                    deviceControl.AdditionalStateCodes = additionalStateCodes;

                    return deviceControl;
                }

                deviceControl.StateType = SelectedStateViewModel.State.StateType;
                foreach (var stateViewModel in StateViewModels)
                {
                    if (stateViewModel.IsAdditional &&
                        stateViewModel.IsChecked &&
                        stateViewModel.State.StateType == SelectedStateViewModel.State.StateType
                    ) additionalStateCodes.Add(stateViewModel.State.Code);
                }
                deviceControl.AdditionalStateCodes = additionalStateCodes;

                return deviceControl;
            }
        }

        public static void SetDefaultStateTo(LibraryDevice device)
        {
            device.States = new List<LibraryState>();
            device.States.Add(StateViewModel.GetDefaultStateWith());
        }

        public static LibraryDevice GetDefaultDriverWith(Guid driverId)
        {
            var device = new LibraryDevice();
            device.DriverId = driverId;
            SetDefaultStateTo(device);

            return device;
        }

        public RelayCommand AddStateCommand { get; private set; }
        void OnAddState()
        {
            var addStateViewModel = new StateDetailsViewModel(Device);
            if (ServiceFactory.UserDialogs.ShowModalWindow(addStateViewModel))
            {
                Device.States.Add(addStateViewModel.SelectedItem.State);
                StateViewModels.Add(addStateViewModel.SelectedItem);
                LibraryModule.HasChanges = true;
            }
        }

        public RelayCommand AddAdditionalStateCommand { get; private set; }
        void OnAddAdditionalState()
        {
            var addAdditionalStateViewModel = new AdditionalStateDetailsViewModel(Device);
            if (ServiceFactory.UserDialogs.ShowModalWindow(addAdditionalStateViewModel))
            {
                Device.States.Add(addAdditionalStateViewModel.SelectedItem.State);
                StateViewModels.Add(addAdditionalStateViewModel.SelectedItem);
                LibraryModule.HasChanges = true;
            }
        }

        public RelayCommand RemoveStateCommand { get; private set; }
        void OnRemoveState()
        {
            var dialogResult = DialogBox.DialogBox.Show("Удалить выбранное состояние?",
                                                MessageBoxButton.OKCancel,
                                                MessageBoxImage.Question);

            if (dialogResult == MessageBoxResult.OK)
            {
                Device.States.Remove(SelectedStateViewModel.State);
                StateViewModels.Remove(SelectedStateViewModel);
                LibraryModule.HasChanges = true;
            }
        }

        bool CanRemoveState()
        {
            return SelectedStateViewModel != null && SelectedStateViewModel.State.StateType != StateViewModel.DefaultStateType;
        }
    }
}