﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.Common;
using System.Collections.ObjectModel;
using System.IO;
using MuliclientAPI;
using System.Runtime.Serialization;
using Common;
using Infrastructure.Common.Windows;

namespace MultiClient.ViewModels
{
	public class ShellViewModel : BaseViewModel
	{
		public ShellViewModel()
		{
			AddCommand = new RelayCommand(OnAdd);
			RemoveCommand = new RelayCommand(OnRemove, CanRemove);
			SaveCommand = new RelayCommand(OnSave, CanSave);
			AppItems = new ObservableCollection<AppItemViewModel>();
		}

		public void Initialize(string password)
		{
			var configuration = MulticlientConfigurationHelper.LoadConfiguration(password);
			foreach (var multiclientData in configuration.MulticlientDatas)
			{
				var appItemViewModel = new AppItemViewModel(multiclientData);
				AppItems.Add(appItemViewModel);
			}
			SelectedAppItem = AppItems.FirstOrDefault();
		}

		public ObservableCollection<AppItemViewModel> AppItems { get; private set; }

		AppItemViewModel _selectedAppItem;
		public AppItemViewModel SelectedAppItem
		{
			get { return _selectedAppItem; }
			set
			{
				_selectedAppItem = value;
				OnPropertyChanged("SelectedAppItem");
			}
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var appItemViewModel = new AppItemViewModel(new MulticlientData());
			AppItems.Add(appItemViewModel);
			SelectedAppItem = AppItems.LastOrDefault();
			HasChanges = true;
		}

		public RelayCommand RemoveCommand { get; private set; }
		void OnRemove()
		{
			AppItems.Remove(SelectedAppItem);
			SelectedAppItem = AppItems.FirstOrDefault();
			HasChanges = true;
		}
		bool CanRemove()
		{
			return SelectedAppItem != null;
		}

		public RelayCommand SaveCommand { get; private set; }
		void OnSave()
		{
			var passwordViewModel = new PasswordViewModel();
			DialogService.ShowModalWindow(passwordViewModel);
			var password = passwordViewModel.Password;
			if (string.IsNullOrEmpty(password))
				return;

			var configuration = new MulticlientConfiguration();
			foreach (var appItem in AppItems)
			{
				var multiclientData = new MulticlientData()
				{
					Name = appItem.Name,
					Address = appItem.Address,
					Port = appItem.Port,
					Login = appItem.Login,
					Password = appItem.Password
				};
				configuration.MulticlientDatas.Add(multiclientData);
			}
			MulticlientConfigurationHelper.SaveConfiguration(configuration, password);
			HasChanges = false;
		}
		bool CanSave()
		{
			return HasChanges;
		}

		public static bool HasChanges = false;

		public void SaveOnClose()
		{
			if (HasChanges)
			{
				if (MessageBoxService.ShowQuestion("Сохранить изменения") == System.Windows.MessageBoxResult.Yes)
					OnSave();
			}
		}
	}
}