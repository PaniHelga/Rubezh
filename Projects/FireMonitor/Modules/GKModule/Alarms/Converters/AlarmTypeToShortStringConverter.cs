﻿using System;
using System.Windows.Data;
using FiresecAPI.Models;
using XFiresecAPI;

namespace GKModule.Converters
{
	public class AlarmTypeToShortStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((XAlarmType) value)
            {
				case XAlarmType.NPTOn:
					return "ПЗ";

				case XAlarmType.Fire1:
                    return "П1";

				case XAlarmType.Fire2:
					return "П2";

				case XAlarmType.Attention:
                    return "В";

				case XAlarmType.Failure:
                    return "Н";

				case XAlarmType.Ignore:
                    return "О";

				case XAlarmType.Info:
                    return "И";

				case XAlarmType.Service:
                    return "ТО";

				case XAlarmType.AutoOff:
                    return "АО";

                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}