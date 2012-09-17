﻿using System.Collections.Generic;
using System.Linq;

namespace FiresecAPI.Models
{
	public class AMP4Helper
	{
		public static void Create(List<Driver> drivers)
		{
			var driver = drivers.FirstOrDefault(x => x.DriverType == DriverType.AMP_4);
            driver.HasConfigurationProperties = true;

			AddAM(driver);
		}

		private static void AddAM(Driver driver)
		{
			var property1 = new DriverProperty()
			{
				IsAUParameter = true,
				No = 0x81,
				Name = "Тип шлейфа модуля",
				Caption = "Тип шлейфа модуля",
				Default = "0",
				BitOffset = 4,
				UseMask = true
			};
			var property1Parameter1 = new DriverPropertyParameter()
			{
				Name = "0 Шлейф дымовых датчиков с определением двойной сработки",
				Value = "0"
			};
			var property1Parameter2 = new DriverPropertyParameter()
			{
				Name = "1 Комбинированный шлейф дымовых и тепловых датчиков без определения двойной сработки тепловых датчиков и с определением двойной сработки дымовых",
				Value = "1"
			};
			var property1Parameter3 = new DriverPropertyParameter()
			{
				Name = "2 Шлейф тепловых датчиков с определением двойной сработки",
				Value = "2"
			};
			var property1Parameter4 = new DriverPropertyParameter()
			{
				Name = "3 Комбинированный шлейф дымовых и тепловых датчиков без определения двойной сработки и без контроля короткого замыкания ШС",
				Value = "3"
			};
			property1.Parameters.Add(property1Parameter1);
			property1.Parameters.Add(property1Parameter2);
			property1.Parameters.Add(property1Parameter3);
			property1.Parameters.Add(property1Parameter4);
			driver.Properties.Add(property1);

			var property2 = new DriverProperty()
			{
				IsAUParameter = true,
				No = 0x81,
				Name = "Тип включения выхода при пожаре модуля",
				Caption = "Тип включения выхода при пожаре модуля",
				Default = "2",
				MaxBit=3,
				UseMask=true
			};
			var property2Parameter1 = new DriverPropertyParameter()
			{
				Name = "0 Выключено",
				Value = "0"
			};
			var property2Parameter2 = new DriverPropertyParameter()
			{
				Name = "1 Мерцает",
				Value = "1"
			};
			var property2Parameter3 = new DriverPropertyParameter()
			{
				Name = "2 Включено",
				Value = "2"
			};
			property2.Parameters.Add(property2Parameter1);
			property2.Parameters.Add(property2Parameter2);
			property2.Parameters.Add(property2Parameter3);
			driver.Properties.Add(property2);
		}
	}
}