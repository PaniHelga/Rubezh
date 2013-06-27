﻿using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common.Windows;
using System.Collections.Generic;

namespace DevicesModule.Guard
{
	public class FS2DeviceSetGuardUsersListHelper
	{
		static Device Device;
		static OperationResult OperationResult;
		static List<GuardUser> GuardUsers;

		public static void Run(Device device, List<GuardUser> guardUser)
		{
			Device = device;
			GuardUsers = guardUser;
			ServiceFactory.ProgressService.Run(OnPropgress, OnCompleted, Device.PresentationAddressAndName + ". Запись списка пользователей");
		}

		static void OnPropgress()
		{
			OperationResult = FiresecManager.FS2ClientContract.DeviceSetGuardUsers(Device.UID, GuardUsers);
		}

		static void OnCompleted()
		{
			if (OperationResult.HasError)
			{
				MessageBoxService.ShowError(OperationResult.Error, "Ошибка при выполнении операции");
			}
			MessageBoxService.Show("Операция завершилась успешно");
		}
	}
}