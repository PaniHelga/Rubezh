﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FiresecAPI.SKD.ReportFilters
{
	public enum ReportPeriodType
	{
		[Description("Последние сутки")]
		Day,
		[Description("Последняя неделя")]
		Week,
		[Description("Последний месяц")]
		Month,
		[Description("Произвольный период")]
		Arbitrary
	}
}
