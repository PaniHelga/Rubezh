﻿using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common.Windows;
using System.Collections.Generic;

namespace DevicesModule.Guard
{
	public class FS2DeviceGetGuardUserListHelper
	{
		static Device Device;
		static OperationResult<List<GuardUser>> OperationResult;
		public static List<GuardUser> Result { get; private set; }

		public static void Run(Device device)
		{
			Device = device;
			ServiceFactory.ProgressService.Run(OnPropgress, OnCompleted, Device.PresentationAddressAndName + ". Чтение списка пользователей");
		}

		static void OnPropgress()
		{
			OperationResult = FiresecManager.FS2ClientContract.DeviceGetGuardUsers(Device.UID);
		}

		static void OnCompleted()
		{
			if (OperationResult.HasError)
			{
				MessageBoxService.ShowError(OperationResult.Error, "Ошибка при выполнении операции");
				return;
			}
			Result = OperationResult.Result;
		}
	}
}