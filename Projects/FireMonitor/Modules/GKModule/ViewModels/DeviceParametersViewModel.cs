﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using System.Collections.ObjectModel;
using FiresecClient;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace GKModule.ViewModels
{
	public class DeviceParametersViewModel : ViewPartViewModel
	{
		BackgroundWorker BackgroundWorker;
		bool CancelBackgroundWorker = false;

		public DeviceParametersViewModel()
		{
			ParameterUpdateHelper.NewAUParameterValue -= new Action<AUParameterValue>(ParameterUpdateHelper_NewAUParameterValue);
			ParameterUpdateHelper.NewAUParameterValue += new Action<AUParameterValue>(ParameterUpdateHelper_NewAUParameterValue);
		}

		public void Initialize()
		{
			Devices = new ObservableCollection<DeviceParameterViewModel>();
			foreach (var device in XManager.DeviceConfiguration.Devices)
			{
				if (device.Driver.AUParameters.Where(x => !x.IsDelay).Count() > 0)
				{
					var deviceParameterViewModel = new DeviceParameterViewModel(device);
					Devices.Add(deviceParameterViewModel);
				}
			}
			SelectedDevice = Devices.FirstOrDefault();
		}

		public ObservableCollection<DeviceParameterViewModel> Devices { get; private set; }

		DeviceParameterViewModel _selectedDevice;
		public DeviceParameterViewModel SelectedDevice
		{
			get { return _selectedDevice; }
			set
			{
				_selectedDevice = value;
				OnPropertyChanged("SelectedDevice");
			}
		}

		public override void OnShow()
		{
			BackgroundWorker = new BackgroundWorker();
			BackgroundWorker.DoWork += new DoWorkEventHandler(UpdateAuParameters);
			BackgroundWorker.RunWorkerAsync();
		}

		public override void OnHide()
		{
			CancelBackgroundWorker = true;
		}

		void UpdateAuParameters(object sender, DoWorkEventArgs e)
		{
			while (true)
			{
				if (CancelBackgroundWorker)
					break;
				foreach (var deviceParameterViewModel in Devices)
				{
					if (deviceParameterViewModel.Device.Driver.AUParameters.Any(x => x.InternalName == "Temperature"))
						deviceParameterViewModel.Temperature = "опрос";
					if (deviceParameterViewModel.Device.Driver.AUParameters.Any(x => x.InternalName == "Dustinness"))
						deviceParameterViewModel.Dustinness = "опрос";
					if (deviceParameterViewModel.Device.Driver.AUParameters.Any(x => x.InternalName == "LastServiceTime"))
						deviceParameterViewModel.LastServiceTime = "опрос";
					if (deviceParameterViewModel.Device.Driver.AUParameters.Any(x => x.InternalName == "Resistance"))
						deviceParameterViewModel.Resistance = "опрос";

					deviceParameterViewModel.IsCurrent = true;
					ParameterUpdateHelper.UpdateDevice(deviceParameterViewModel.Device);
					deviceParameterViewModel.IsCurrent = false;
				}
			}
		}

		void ParameterUpdateHelper_NewAUParameterValue(AUParameterValue auParameterValue)
		{
			var deviceParameterViewModel = Devices.FirstOrDefault(x => x.Device.UID == auParameterValue.Device.UID);
			if (deviceParameterViewModel != null)
			{
				switch (auParameterValue.DriverParameter.InternalName)
				{
					case "Temperature":
						deviceParameterViewModel.Temperature = auParameterValue.StringValue;
						break;

					case "Dustinness":
						deviceParameterViewModel.Dustinness = auParameterValue.StringValue;
						break;

					case "LastServiceTime":
						deviceParameterViewModel.LastServiceTime = auParameterValue.StringValue;
						break;

					case "Resistance":
						deviceParameterViewModel.Resistance = auParameterValue.StringValue;
						break;
				}
			}
			//Trace.WriteLine("AUParameterValue " + auParameterValue.Device.ShortNameAndDottedAddress + " - " + auParameterValue.Name + " - " + auParameterValue.Value);
		}
	}
}