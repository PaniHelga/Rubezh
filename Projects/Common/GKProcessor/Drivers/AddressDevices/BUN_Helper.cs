﻿using System;
using XFiresecAPI;

namespace GKProcessor
{
	public static class BUN_Helper
	{
		public static XDriver Create()
		{
			var driver = new XDriver()
			{
				DriverTypeNo = 0x70,
				DriverType = XDriverType.Pump,
				UID = new Guid("8bff7596-aef4-4bee-9d67-1ae3dc63ca94"),
				Name = "Шкаф управления насосом",
				ShortName = "ШУН",
				IsControlDevice = true,
				HasLogic = false,
				IsPlaceable = true,
				MaxAddressOnShleif = 15
			};

			GKDriversHelper.AddControlAvailableStates(driver);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.AutoOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.On);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOn);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.TurningOff);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Off);


			driver.AvailableCommandBits.Add(XStateBit.TurnOn_InManual);
			driver.AvailableCommandBits.Add(XStateBit.TurnOnNow_InManual);
			driver.AvailableCommandBits.Add(XStateBit.TurnOff_InManual);

			GKDriversHelper.AddIntProprety(driver, 0x8c, "Время разновременного пуска, с", 0, 1, 0, 255);
			GKDriversHelper.AddIntProprety(driver, 0x84, "Время ожидания выхода насоса на режим, с", 0, 3, 3, 30);

			var property3 = new XDriverProperty()
			{
				No = 0x8d,
				Name = "ЭКМ на выходе насоса НЗ",
				Caption = "ЭКМ на выходе насоса НЗ",
				DriverPropertyType = XDriverPropertyTypeEnum.BoolType,
				IsLowByte = true,
				Mask = 1
			};
			driver.Properties.Add(property3);

			var property4 = new XDriverProperty()
			{
				No = 0x8d,
				Name = "УЗН Старт НЗ",
				Caption = "УЗН Старт НЗ",
				DriverPropertyType = XDriverPropertyTypeEnum.BoolType,
				IsLowByte = true,
				Mask = 2
			};
			driver.Properties.Add(property4);

			var property5 = new XDriverProperty()
			{
				No = 0x8d,
				Name = "УЗН Стоп НЗ",
				Caption = "УЗН Стоп НЗ",
				DriverPropertyType = XDriverPropertyTypeEnum.BoolType,
				IsLowByte = true,
				Mask = 4
			};
			driver.Properties.Add(property5);

			var property6 = new XDriverProperty()
			{
				No = 0x8d,
				Name = "Функция УЗН Вкл",
				Caption = "Функция УЗН Вкл",
				DriverPropertyType = XDriverPropertyTypeEnum.BoolType,
				IsLowByte = true,
				Mask = 8
			};
			driver.Properties.Add(property6);

			driver.AUParameters.Add(new XAUParameter() { No = 0x80, Name = "Режим работы" });

			return driver;
		}
	}
}