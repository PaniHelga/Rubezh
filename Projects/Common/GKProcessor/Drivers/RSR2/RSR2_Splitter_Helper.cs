﻿using System;
using XFiresecAPI;

namespace GKProcessor
{
	public static class RSR2_Splitter_Helper
	{
		public static XDriver Create()
		{
			var driver = new XDriver()
			{
				DriverTypeNo = 0xE5,
				DriverType = XDriverType.RSR2_Splitter,
				UID = new Guid("0B1BD00D-680B-4A80-AC9A-659FD7F85BB4"),
				Name = "Модуль ветвления и подпитки МВП RSR2",
				ShortName = "МВП RSR2",
				IsPlaceable = true
			};

			GKDriversHelper.AddAvailableStateBits(driver, XStateBit.Test);
			GKDriversHelper.AddAvailableStateClasses(driver, XStateClass.Test);
			
			GKDriversHelper.AddIntProprety(driver, 0, "Число АУ на АЛС3 МВП ", 0, 0, 250);
			GKDriversHelper.AddIntProprety(driver, 1, "Число АУ на АЛС4 МВП ", 0, 0, 250);
			GKDriversHelper.AddIntProprety(driver, 2, "Порог КЗ", 330, 300, 70000);
			//var property = new XDriverProperty()
			//{
			//    No = 2,
			//    Name = "Порог КЗ",
			//    Caption = "Порог КЗ",
			//    DriverPropertyType = XDriverPropertyTypeEnum.IntType,
			//    Default = (ushort)330,
			//    Min = (ushort)300,
			//    Max = (ushort)700,
			//    //Multiplier = 0.1
			//};
			//driver.Properties.Add(property);

			driver.AUParameters.Add(new XAUParameter() { No = 0, Name = "Напряжение на АЛC1", InternalName = "Voltage_1", Multiplier = 10 });
			driver.AUParameters.Add(new XAUParameter() { No = 1, Name = "Напряжение на АЛС2", InternalName = "Voltage_2", Multiplier = 10 });
			driver.AUParameters.Add(new XAUParameter() { No = 2, Name = "Напряжение на АЛС3", InternalName = "Voltage_3", Multiplier = 10 });
			driver.AUParameters.Add(new XAUParameter() { No = 3, Name = "Напряжение на АЛС4", InternalName = "Voltage_4", Multiplier = 10 });

			return driver;
		}
	}
}