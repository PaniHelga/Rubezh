﻿using System;
using System.Collections.Generic;
using FiresecAPI;
using Infrastructure.Common.Validation;

namespace SKDModule.Validation
{
	public static partial class Validator
	{
		static void ValidateSlideDayIntervals()
		{
			ValidateSlideDayIntervalEquality();
			foreach (var slideDayInterval in SKDManager.SKDConfiguration.SlideDayIntervals)
			{
				if (string.IsNullOrEmpty(slideDayInterval.Name))
				{
					Errors.Add(new SlideDayIntervalValidationError(slideDayInterval, "Отсутствует название интервала", ValidationErrorLevel.CannotWrite));
				}
				if (slideDayInterval.TimeIntervalUIDs.Count == 0)
				{
					Errors.Add(new SlideDayIntervalValidationError(slideDayInterval, "Отсутствуют составляющие части интервала", ValidationErrorLevel.CannotWrite));
				}
			}
		}

		static void ValidateSlideDayIntervalEquality()
		{
			var slideDayIntervals = new HashSet<string>();
			foreach (var slideDayInterval in SKDManager.SKDConfiguration.SlideDayIntervals)
			{
				if (!slideDayIntervals.Add(slideDayInterval.Name))
				{
					Errors.Add(new SlideDayIntervalValidationError(slideDayInterval, "Дублируется название интервала", ValidationErrorLevel.CannotWrite));
				}
			}
		}
	}
}