﻿using System.Collections.Generic;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using ReportsModule.Models;
using System.Text;

namespace ReportsModule.Reports
{
	public class ReportDriverCounter : BaseReportGeneric<ReportDriverCounterModel>
	{
		public ReportDriverCounter()
		{
			ReportFileName = "DriverCounterRdlc.rdlc";
			DataSourceFileName = "DriverCounterData";
		}

		public override void LoadData()
		{
			DataList = new List<ReportDriverCounterModel>();

			foreach (var driver in FiresecManager.Drivers)
			{
				if (driver.IsPlaceable && driver.DriverType != DriverType.Computer)
				{
					AddDrivers(driver);
				}
				else
				{
					switch (driver.DriverType)
					{
						case DriverType.IndicationBlock:
							AddDrivers(driver);
							break;
						case DriverType.MS_1:
							AddDrivers(driver);
							break;
						case DriverType.MS_2:
							AddDrivers(driver);
							break;
						case DriverType.MS_3:
							AddDrivers(driver);
							break;
						case DriverType.MS_4:
							AddDrivers(driver);
							break;
						default:
							break;
					}
				}
			}

			DataList.Add(new ReportDriverCounterModel()
			{
				DriverName = "Всего устройств",
				Count = CountDrivers()
			});
		}

		void AddDrivers(Driver driver)
		{
			var devices = FiresecManager.Devices.FindAll(x => x.Driver.UID == driver.UID);
			if (devices.IsNotNullOrEmpty())
			{
				DataList.Add(new ReportDriverCounterModel()
				{
					DriverName = driver.ShortName,
					Count = devices.Count
				});
			}
		}

		int CountDrivers()
		{
			int count = 0;
			DataList.ForEach(x => count += x.Count);
			return count;
		}
	}
}