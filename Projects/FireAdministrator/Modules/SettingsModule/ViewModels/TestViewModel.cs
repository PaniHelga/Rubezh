﻿using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Events;
using SettingsModule.Views;
using System;
using System.Linq;
using FiresecAPI.Models;

namespace SettingsModule.ViewModels
{
	public class TestViewModel : BaseViewModel
	{
		public TestViewModel()
		{
			ShowDriversCommand = new RelayCommand(OnShowDrivers);
			ShowTreeCommand = new RelayCommand(OnShowTree);
			Test1Command = new RelayCommand(OnTest1);
			Test2Command = new RelayCommand(OnTest2);
		}

		public bool IsDebug
		{
			get { return ServiceFactory.AppSettings.IsDebug; }
		}

		string _text;
		public string Text
		{
			get { return _text; }
			set
			{
				_text = value;
				OnPropertyChanged("Text");
			}
		}

		public RelayCommand ShowDriversCommand { get; private set; }
		void OnShowDrivers()
		{
			var driversView = new DriversView();
			driversView.ShowDialog();
		}

		public RelayCommand ShowTreeCommand { get; private set; }
		void OnShowTree()
		{
			var devicesTreeViewModel = new DevicesTreeViewModel();
			DialogService.ShowModalWindow(devicesTreeViewModel);
		}

		public RelayCommand Test1Command { get; private set; }
		void OnTest1()
		{
			var stringBuilder = new StringBuilder();

			var Rm1Driver = FiresecManager.Drivers.FirstOrDefault(x=>x.DriverType == DriverType.RM_1);
			foreach (var state in Rm1Driver.States)
			{
				stringBuilder.AppendLine("РМ-1: " + state.Id + " - " + state.Code + " - " + state.Name);
			}

			foreach (var driver in FiresecManager.Drivers)
			{
				foreach (var state in driver.States)
				{
					if (state.AffectParent)
					{
						stringBuilder.AppendLine("AffectParent - " + driver.Name + " - " + state.Name);
					}
				}
			}

			foreach (var driver in FiresecManager.Drivers)
			{
				foreach (var state in driver.States)
				{
					if (state.IsManualReset)
					{
						stringBuilder.AppendLine("IsManualReset - " + driver.Name + " - " + state.Name);
					}
				}
			}

			foreach (var driver in FiresecManager.Drivers)
			{
				foreach (var state in driver.States)
				{
					if (state.CanResetOnPanel)
					{
						stringBuilder.AppendLine("CanResetOnPanel - " + driver.Name + " - " + state.Name);
					}
				}
			}

			foreach (var driver in FiresecManager.Drivers)
			{
				foreach (var state in driver.States)
				{
					if (state.IsAutomatic && state.Name.Contains("AutoOff"))
					{
						stringBuilder.AppendLine("Automatic AutoOff - " + driver.Name + " - " + state.Name);
					}
					if (state.IsAutomatic && state.Name.Contains("Auto_Off"))
					{
						stringBuilder.AppendLine("Automatic Auto_Off - " + driver.Name + " - " + state.Name);
					}
				}
			}

			foreach (var driver in FiresecManager.Drivers)
			{
				foreach (var state in driver.States)
				{
					if (state.IsAutomatic)
					{
						stringBuilder.AppendLine("Automatic - " + driver.Name + " - " + state.Name + " - " + state.Code);
					}
				}
			}
			Text = stringBuilder.ToString();
		}

		public RelayCommand Test2Command { get; private set; }
		void OnTest2()
		{
			Guid uid1 = Guid.Parse("{584BC59A-28D5-430B-90BF-592E40E843A6}");
			Guid uid2 = Guid.Parse("28A7487A-BA32-486C-9955-E251AF2E9DD4");

			string stringUid1 = uid1.ToString("B");
			string stringUid2 = uid2.ToString("B");

			Text = stringUid1 + "\n" + stringUid2;
		}
	}
}