﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common;
using Infrastructure.Client.Login.ViewModels;
using Infrastructure.Common.MessageBox;
using System.Configuration;

namespace Infrastructure.Client.Login
{
	public class LoginService
	{
		private IUserDialogService _userDialogService;
		private string _clientType;
		private string _title;

		public LoginService(IUserDialogService userDialogService, string clientType, string title = null)
		{
			_userDialogService = userDialogService;
			_clientType = clientType;
			_title = title ?? "Авторизация";
		}

		public bool ExecuteConnect()
		{
			return Execute(LoginViewModel.PasswordViewType.Connect);
		}
		public bool ExecuteReconnect()
		{
			return Execute(LoginViewModel.PasswordViewType.Reconnect);
		}
		public bool ExecuteValidate()
		{
			return Execute(LoginViewModel.PasswordViewType.Validate);
		}

		private bool Execute(LoginViewModel.PasswordViewType passwordViewType)
		{
			var loginViewModel = new LoginViewModel(_clientType, passwordViewType) { Title = _title };
			bool isAutoconnect = GetIsAutoConnect() && passwordViewType == LoginViewModel.PasswordViewType.Connect;
			while (!loginViewModel.IsConnected && !loginViewModel.IsCanceled)
			{
				if (isAutoconnect)
					loginViewModel.ConnectCommand.Execute();
				else
					_userDialogService.ShowModalWindow(loginViewModel);
				if (!string.IsNullOrEmpty(loginViewModel.Message))
					MessageBoxService.Show(loginViewModel.Message);
				isAutoconnect = false;
			}
			return loginViewModel.IsConnected;
		}
		private bool GetIsAutoConnect()
		{
			string setting = ConfigurationManager.AppSettings["AutoConnect"];
			bool res;
			return setting == null || !bool.TryParse(setting, out res) ? false : res;
		}
	}
}