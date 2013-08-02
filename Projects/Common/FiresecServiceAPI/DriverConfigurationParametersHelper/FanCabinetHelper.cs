﻿using System.Collections.Generic;
using System.Linq;

namespace FiresecAPI.Models
{
	public class FanCabinetHelper
	{
		public static void Create(List<Driver> drivers)
		{
			var driver = drivers.FirstOrDefault(x => x.DriverType == DriverType.FanCabinet);
			if (driver != null)
			{
				driver.HasConfigurationProperties = true;
				ConfigurationDriverHelper.AddPlainEnumProprety(driver, 0x82, "Внешний сигнал", 0, "Сигнал с кнопок управления ПУСК и СТОП", "Сигнал с датчика выхода на режим");
				ConfigurationDriverHelper.AddIntProprety(driver, 0x83, "Время удержания, мин", "Время удержания, с", 0, 0, 0, 255);
				ConfigurationDriverHelper.AddIntProprety(driver, 0x84, "Задержка включения, с", "Задержка включения, с", 0, 0, 0, 255);
				ConfigurationDriverHelper.AddIntProprety(driver, 0x85, "Ожидание выхода на режим, с", "Ожидание выхода на режим, с", 0, 0, 0, 255);
			}
		}
	}
}