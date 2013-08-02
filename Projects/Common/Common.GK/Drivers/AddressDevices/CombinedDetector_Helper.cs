﻿using System;
using XFiresecAPI;

namespace Common.GK
{
	public class CombinedDetector_Helper
	{
		public static XDriver Create()
		{
			var driver = new XDriver()
			{
				DriverTypeNo = 0x60,
				DriverType = XDriverType.CombinedDetector,
				UID = new Guid("37f13667-bc77-4742-829b-1c43fa404c1f"),
				Name = "Пожарный комбинированный извещатель ИП212/101-64-А2R1",
				ShortName = "ИП-64К",
				HasZone = true,
                IsPlaceable = true
			};

			GKDriversHelper.AddAvailableStates(driver, XStateType.Test);
			GKDriversHelper.AddAvailableStates(driver, XStateType.Fire1);
            GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Info);
            GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Fire1);
            GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Service);

			GKDriversHelper.AddIntProprety(driver, 0x84, "Порог срабатывания по дыму, 0.01*дБ/м", 0, 18, 5, 20);
			GKDriversHelper.AddIntProprety(driver, 0x8B, "Порог срабатывания по температуре, C", 0, 70, 54, 85);

			driver.AUParameters.Add(new XAUParameter() { No = 0x82, Name = "Дым", InternalName = "Smokiness" });
			driver.AUParameters.Add(new XAUParameter() { No = 0x83, Name = "Температура", InternalName = "Temperature" });

			driver.AUParameters.Add(new XAUParameter() { No = 0x86, Name = "Текущая запыленность", InternalName = "Dustinness" });
			driver.AUParameters.Add(new XAUParameter() { No = 0x87, Name = "Порог запыленности предварительный" });
			driver.AUParameters.Add(new XAUParameter() { No = 0x8A, Name = "Порог запыленности критический" });

			return driver;
		}
	}
}