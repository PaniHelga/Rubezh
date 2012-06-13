﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.Models;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using System.ServiceModel;

namespace FiresecService.ViewModels
{
	public class MainViewModel : ApplicationViewModel
	{
		public static MainViewModel Current { get; private set; }
		public string HostStatus { get; private set; }
		public string ComServersStatus { get; private set; }

		public MainViewModel()
		{
			Current = this;
			ShowOperationHistoryCommand = new RelayCommand(OnShowOperationHistory);
			ShowImitatorCommand = new RelayCommand(OnShowImitator);
			Clients = new ObservableCollection<ClientViewModel>();
			Title = "Сервер ОПС FireSec-2";
			ComServersStatus = "устанавливается";
			HostStatus = "устанавливается";
		}

		public RelayCommand ShowImitatorCommand { get; private set; }
		void OnShowImitator()
		{
			foreach (var connection in Clients)
			{
				if (connection.ClientType == ClientType.Itv)
				{
					var imitatorViewModel = new ImitatorViewModel(connection.FiresecService);
					DialogService.ShowModalWindow(imitatorViewModel);
					break;
				}
			}
		}

		public RelayCommand ShowOperationHistoryCommand { get; private set; }
		void OnShowOperationHistory()
		{
			if (SelectedClient != null)
			{
				var operationHistoryViewModel = new OperationHistoryViewModel(SelectedClient);
				DialogService.ShowModalWindow(operationHistoryViewModel);
			}
		}

		private string _status;
		public string Satus
		{
			get { return _status; }
			set
			{
				_status = value;
				OnPropertyChanged("Status");
			}
		}

		public bool IsDebug
		{
			get { return AppSettings.IsDebug; }
		}

		public ObservableCollection<ClientViewModel> Clients { get; private set; }

		ClientViewModel _selectedClient;
		public ClientViewModel SelectedClient
		{
			get { return _selectedClient; }
			set
			{
				_selectedClient = value;
				OnPropertyChanged("SelectedClient");
			}
		}

		public void AddClient(FiresecService.Service.FiresecService firesecService)
		{
			Dispatcher.Invoke(new Action(
			delegate()
			{
				var endpointAddress = new EndpointAddress(new Uri(firesecService.ClientCredentials.ClientCallbackAddress));
				var port = endpointAddress.Uri.Port;
				var connectionViewModel = new ClientViewModel()
				{
					FiresecService = firesecService,
					UID = firesecService.UID,
					UserName = firesecService.ClientCredentials.UserName,
					ClientType = firesecService.ClientCredentials.ClientType,
					IpAddress = firesecService.ClientIpAddressAndPort,
					CallbackPort = port,
					ConnectionDate = DateTime.Now
				};
				Clients.Add(connectionViewModel);
			}
			));
		}
		public void RemoveClient(Guid uid)
		{
			Dispatcher.Invoke(new Action(
			delegate()
			{
				var connectionViewModel = MainViewModel.Current.Clients.FirstOrDefault(x => x.UID == uid);
				Clients.Remove(connectionViewModel);
			}
			));
		}
		public void EditClient(Guid uid, string userName)
		{
			Dispatcher.Invoke(new Action(
			delegate()
			{
				var connectionViewModel = MainViewModel.Current.Clients.FirstOrDefault(x => x.UID == uid);
				connectionViewModel.ConnectionDate = DateTime.Now;
				connectionViewModel.UserName = userName;
			}
			));
		}

		public void BeginAddOperation(Guid uid, OperationDirection operationDirection, string operationName)
		{
			Dispatcher.Invoke(new Action(
			delegate()
			{
				var connectionViewModel = MainViewModel.Current.Clients.FirstOrDefault(x => x.UID == uid);
				if (connectionViewModel != null)
				{
					connectionViewModel.BeginAddOperation(operationDirection, operationName);
				}
			}
			));
		}

		public void EndAddOperation(Guid uid, OperationDirection operationDirection)
		{
			Dispatcher.Invoke(new Action(
			delegate()
			{
				var connectionViewModel = MainViewModel.Current.Clients.FirstOrDefault(x => x.UID == uid);
				if (connectionViewModel != null)
				{
					connectionViewModel.EndAddOperation(operationDirection);
				}
			}
			));
		}

		public void UpdateClientState(Guid uid, string state)
		{
			Dispatcher.Invoke(new Action(
			delegate()
			{
				var connectionViewModel = MainViewModel.Current.Clients.FirstOrDefault(x => x.UID == uid);
				if (connectionViewModel != null)
				{
					connectionViewModel.State = state;
				}
			}
			));
		}

		public void UpdateStatus(string hostStatus, string comServersStatus)
		{
			Dispatcher.Invoke(new Action(
			delegate()
			{
				HostStatus = hostStatus;
				ComServersStatus = comServersStatus;
				OnPropertyChanged("HostStatus");
				OnPropertyChanged("ComServersStatus");
			}
			));
		}

		public override bool OnClosing(bool isCanceled)
		{
			Minimize();
			return true;
		}
	}
}