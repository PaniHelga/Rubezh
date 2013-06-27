﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFS2
{
	public static class USBExceptionHelper
	{
		public static string GetError(int errorCode)
		{
			switch(errorCode)
			{
				case 0x01:
					return "Функция в принятом сообщении не поддерживается в данном приборе или в текущем режиме работы прибора";

				case 0x02:
					return "Адрес(номер параметра, подфункция), указанный в поле данных, является недопустимым";

				case 0x03:
					return "Значения в поле данных недопустимы";

				case 0x04:
					return "Прибор не может ответить на запрос или произошла авария";

				case 0x07:
					return "Функция записи/стирания при обновлении ПО не может быть выполнена. Следующее поле - код аппаратно-зависимой информации";

				case 0x08:
					return "Отказ от обслуживания поступившего запроса";

				case 0x09:
					return "Неверный ответ от адресного устройства - ошибка CRC либо устройство отсутствует";

				case 0x0A:
					return "Ошибка записи параметра в устройство";
			}
			return "";
		}
	}
}