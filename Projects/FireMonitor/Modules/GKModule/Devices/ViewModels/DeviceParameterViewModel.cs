﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.GK;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Common.Windows;

namespace GKModule.ViewModels
{
	public class DeviceParameterViewModel : BaseViewModel
	{
		public DeviceViewModel DeviceViewModel { get; private set; }
		public GKDevice Device { get; private set; }

		public DeviceParameterViewModel(GKDevice device)
		{
			Device = device;
			DeviceViewModel = new DeviceViewModel(device);

			Smokiness = " - ";
			Temperature = " - ";
			Dustinness = " - ";
			LastServiceTime = " - ";
			Resistance = " - ";
			AUParameterValues = new ObservableCollection<MeasureParameterViewModel>();

			device.State.MeasureParametersChanged += new Action(OnMeasureParametersChanged);
		}

		void OnMeasureParametersChanged()
		{
		}

		bool _isCurrent;
		public bool IsCurrent
		{
			get { return _isCurrent; }
			set
			{
				_isCurrent = value;
				OnPropertyChanged(() => IsCurrent);
			}
		}

		string _temperature;
		public string Temperature
		{
			get { return _temperature; }
			set
			{
				_temperature = value;
				OnPropertyChanged(() => Temperature);
			}
		}

		string _smokiness;
		public string Smokiness
		{
			get { return _smokiness; }
			set
			{
				_smokiness = value;
				OnPropertyChanged(() => Smokiness);
			}
		}

		string _dustinness;
		public string Dustinness
		{
			get { return _dustinness; }
			set
			{
				_dustinness = value;
				OnPropertyChanged(() => Dustinness);
			}
		}

		string _lastServiceTime;
		public string LastServiceTime
		{
			get { return _lastServiceTime; }
			set
			{
				_lastServiceTime = value;
				OnPropertyChanged(() => LastServiceTime);
			}
		}

		string _resistance;
		public string Resistance
		{
			get { return _resistance; }
			set
			{
				_resistance = value;
				OnPropertyChanged(() => Resistance);
			}
		}

		public void OnNewAUParameterValue(MeasureParameterViewModel value)
		{
			ApplicationService.BeginInvoke(() =>
			{
				var auParameterValue = AUParameterValues.FirstOrDefault(x => x.Name == value.Name);
				if (auParameterValue == null)
				{
					auParameterValue = value;
					AUParameterValues.Add(auParameterValue);
				}
				auParameterValue.StringValue = value.StringValue;
				OnPropertyChanged(() => AUParameterValues);
			});
		}

		ObservableCollection<MeasureParameterViewModel> _auParameterValues;
		public ObservableCollection<MeasureParameterViewModel> AUParameterValues
		{
			get { return _auParameterValues; }
			set
			{
				_auParameterValues = value;
				OnPropertyChanged(() => AUParameterValues);
			}
		}
	}
}