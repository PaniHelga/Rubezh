﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Infrastructure.Common;
using Device = FiresecAPI.Models.Device;

namespace ServerFS2
{
	public static class SetConfigurationOperationHelper
	{
		static List<byte> _crc;

		public static string UpdateFullFlash(Device device)
		{
			var firmwareFileName = Path.Combine(AppDataFolderHelper.GetFolder("Server"), "Сборка 2АМ\\sborka2AM.zip");
			
			var tempLoaderInfo = FirmwareUpdateOperationHelper.GetHexInfo(firmwareFileName, "R2AM_BUNS_update_rs_v3.10.hex");
			var avrInfo = FirmwareUpdateOperationHelper.GetHexInfo(firmwareFileName, "avr_buns_new_hard_m88_v1_20.hex");
			var loaderInfo = FirmwareUpdateOperationHelper.GetHexInfo(firmwareFileName, "R2AM_loader_v3.10_CRP.hex");
			var softWareInfo = FirmwareUpdateOperationHelper.GetHexInfo(firmwareFileName, "Rubezh_OPS.hex");

			WriteTempLoader(device, tempLoaderInfo); //Временный Ram загрузчика RS-485
			//WriteAvr(device, avrInfo); //ПО контроллера однопроводного интерфейса (AVR)
			WriteLoader(device, loaderInfo); //Загрузчик ROM
			WriteSoftWare(device, softWareInfo); //Пользовательское ПО (ARM)

			return "";
		}

		#region
		// Записать временный загрузчик
		private static void WriteTempLoader(Device device, HexInfo tempLoaderInfo)
		{
			// 01 01, 01 03, 37 02, 37 03, 37 01
			BeginUpdateFirmWare(device);
			ConfirmLongTermOperation(device);
			// 3D
			WriteRomConfiguration(device, tempLoaderInfo.Bytes, tempLoaderInfo.Offset);
			BeginUpdateRom(device);
			ConfirmLongTermOperation(device);
			// 3D, 01 01
		}
		private static void WriteAvr(Device device, HexInfo avrInfo)
		{
			WriteAVRConfiguration(device, avrInfo.Bytes, 0x7D000);
			ConfirmLongTermOperation(device);
			ClearAvrSector(device);
			ConfirmLongTermOperation(device);
		}
		private static void WriteLoader(Device device, HexInfo loaderInfo)
		{
			WriteRomConfiguration(device, loaderInfo.Bytes, loaderInfo.Offset); // Запись прошивки с 00 00 по 00 2F
			ConfirmLongTermOperation(device);
			ClearAvrSector(device);
			ConfirmLongTermOperation(device);
		}
		private static void WriteSoftWare(Device device, HexInfo softWareInfo)
		{
			WriteRomConfiguration(device, softWareInfo.Bytes, 0x5000); // Запись прошивки с 50 00 по 07 CF
			USBManager.Send(device, "Запись версии базы", 0x02, 0x12, 0x02, 0x30);// 02 12 version(For Example 02 30)
			// 01 12
			StopUpdating(device);
			ConfirmLongTermOperation(device);
		}
		#endregion

		public static bool WriteDeviceConfiguration(Device device, List<byte> Flash, List<byte> Rom)
		{
			var panelDatabaseReader = new ReadPanelDatabaseOperationHelper(device, false);
			var romDBFirstIndex = panelDatabaseReader.GetRomFirstIndex(device);
			BlockBD(device);
			WriteFlashConfiguration(device, Flash);
			GetOsStatus(device);
			BeginUpdateFirmWare(device);
			ConfirmLongTermOperation(device);
			var firmwareFileName = Path.Combine(AppDataFolderHelper.GetFolder("Server"), "frm.fscf");
			var hexInfo = FirmwareUpdateOperationHelper.GetHexInfo(firmwareFileName, device.Driver.ShortName + ".hex");
			WriteRomConfiguration(device, hexInfo.Bytes, hexInfo.Offset);
			BeginUpdateRom(device);
			ConfirmLongTermOperation(device);
			ClearSector(device);
			ConfirmLongTermOperation(device);
			WriteRomConfiguration(device, Rom, romDBFirstIndex);
			StopUpdating(device);
			ConfirmLongTermOperation(device);
			GetOsStatus(device);
			ServerHelper.SynchronizeTime(device);
			return true;
		}

		static void BeginUpdateRom(Device device)
		{
			var delayBytes = USBManager.Send(device, "Начать обновление Rom", 0x39, 0x04);
			int delay = 0;
			if (delayBytes.Bytes.Count > 1)
				delay = (int)Math.Pow(2, delayBytes.Bytes[1]);
			Thread.Sleep(delay);
		}

		static void BeginUpdateFirmWare(Device device)
		{
			var delayBytes = USBManager.Send(device, "Начать обновление прошивки", 0x39, 0x01);
			int delay = 0;
			if (delayBytes.Bytes.Count > 1)
				delay = (int)Math.Pow(2, delayBytes.Bytes[1]);
			Thread.Sleep(delay);
		}

		//static bool GetStatusOS(Device device)
		//{
		//    if (USBManager.Send(device, "Уточнить у Ромы", 0x01, 0x01).Bytes[1] == 0) // Если младший байт = 0, то режим пользовательский
		//        return true;
		//    return false;
		//}

		static void WriteFlashConfiguration(Device device, List<byte> deviceFlash)
		{
			deviceFlash.RemoveRange(0, 0x100);
			_crc = new List<byte> { deviceFlash[0], deviceFlash[1] };
			deviceFlash[0] = 0;
			deviceFlash[1] = 0;
			for (int i = 0; i < deviceFlash.Count - 1; i = i + 0x100)
				USBManager.Send(device, "Уточнить у Ромы", 0x02, 0x52, BitConverter.GetBytes(0x100 + i).Reverse(), Math.Min(deviceFlash.Count - i - 1, 0xFF), deviceFlash.GetRange(i, Math.Min(deviceFlash.Count - i, 0x100)));
			USBManager.Send(device, "Уточнить у Ромы", 0x02, 0x52, BitConverter.GetBytes(0x100).Reverse(), 0x01, _crc[0], _crc[1]);
		}

		static void WriteRomConfiguration(Device device, List<byte> deviceRom, int begin)
		{
			for (int i = 0; i < deviceRom.Count; i = i + 0x100)
			{
				USBManager.Send(device, "Запись базы Rom", 0x3E, BitConverter.GetBytes(begin + i).Reverse(), deviceRom.GetRange(i, Math.Min(deviceRom.Count - i, 0x100)));
			}
		}

		static void WriteAVRConfiguration(Device device, List<byte> deviceRom, int begin)
		{
			for (int i = 0; i < deviceRom.Count; i = i + 0x100)
			{
				USBManager.Send(device, "Запись AVR", 0x3E, BitConverter.GetBytes(begin + i).Reverse(), deviceRom.GetRange(i, Math.Min(deviceRom.Count - i, 0x100)));
				USBManager.Send(device, "Подтверждение долговременной операции", 0x3C);
				USBManager.Send(device, "Запрос аппаратной ошибки", 0x3D);
			}
		}

		// Чтение из файла
		//static void WriteFullFlashConfiguration(Device device, string fileName)
		//{
		//    var bytesArray = new List<byte>();
		//    var strings = File.ReadAllLines(fileName).ToList();
		//    foreach (var str in strings)
		//    {
		//        for (var i = 0; i < str.Length ; i += 3)
		//        {
		//            bytesArray.Add(Convert.ToByte(str.Substring(i, 2), 16));
		//        }
		//    }

		//    for (int i = 0; i < bytesArray.Count; i = i + 0x104)
		//    {
		//        var offset = BytesHelper.ExtractInt(bytesArray, i);
		//        USBManager.Send(device, "Уточнить у Ромы", 0x3E, BitConverter.GetBytes(offset).Reverse(), bytesArray.GetRange(i, Math.Min(bytesArray.Count - i, 0x100)));
		//    }
		//}

		// Окончание записи памяти - сброс
		private static void StopUpdating(Device device)
		{
			var delayBytes = USBManager.Send(device, "Окончание записи памяти", 0x3A);
			int delay = 0;
			if (delayBytes.Bytes.Count > 1)
				delay = (int)Math.Pow(2, delayBytes.Bytes[1]);
			Thread.Sleep(delay);
		}

		// Установка блокировки БД
		private static void BlockBD(Device device)
		{
			USBManager.Send(device, "Установка блокировки БД", 0x02, 0x55, 0x01);
			var functionCode = GetAddressList(device);
		}

		private static List<byte> GetAddressList(Device device)
		{
			var result = USBManager.Send(device, "Получить адресный лист", 0x01, 0x14);
			while (result.FunctionCode != 0x41)
			{
				GetAddressList(device);
				break;
			}
			return result.Bytes;
		}

		// Очистка сектора памяти bSectorStart, bSectorEnd
		private static void ClearSector(Device device)
		{
			USBManager.Send(device, "Очистка сектора памяти", 0x3B, 0x03, 0x04);
		}

		private static void ClearAvrSector(Device device)
		{
			USBManager.Send(device, "Очистка сектора памяти AVR", 0x3B, 0x05, 0x1A);
		}

		// Подтверждение / завершение долговременной операции
		public static void ConfirmLongTermOperation(Device device)
		{
			var functionCode = USBManager.Send(device, "Подтверждение долговременной операции", 0x3C).FunctionCode;
			//if (functionCode != 0x7c)
			//    ConfirmLongTermOperation(device);
		}

		public static int GetOsStatus(Device device)
		{
			var result = USBManager.Send(device, "Запрос статуса ОС прибора", 0x01, 0x01);
			var status = BytesHelper.ExtractShort(result.Bytes.GetRange(0, 2), 0);
			while (status != 0)
			{
				GetOsStatus(device);
				break;
			}
			return status;
		}
	}
}