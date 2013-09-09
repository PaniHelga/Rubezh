﻿using System.Data;
using CodeReason.Reports;
using FiresecAPI;
using FiresecClient;
using Infrastructure.Common.Reports;

namespace GKModule.Reports
{
	internal class DeviceListReport : ISingleReportProvider
	{
		#region ISingleReportProvider Members
		public ReportData GetData()
		{
			var data = new ReportData();

			DataTable table = new DataTable("Devices");
			table.Columns.Add("Type");
			table.Columns.Add("Address");
			table.Columns.Add("Zone");

			if (FiresecManager.Devices.IsNotNullOrEmpty())
			{
				foreach (var device in XManager.Devices)
				{
					if (device.Driver.DriverType == XFiresecAPI.XDriverType.System)
						continue;
					if (device.Driver.IsGroupDevice)
						continue;

					var type = device.ShortName;
					var address = device.DottedPresentationAddress;
					var zonePresentationName = XManager.GetPresentationZone(device);
					table.Rows.Add(type, address, zonePresentationName);
				}
			}
			data.DataTables.Add(table);
			return data;
		}
		#endregion

		#region IReportProvider Members
		public string Template
		{
			get { return "Reports/DeviceListReport.xaml"; }
		}

		public string Title
		{
			get { return "Список устройств"; }
		}

		public bool IsEnabled
		{
			get { return true; }
		}
		#endregion
	}
}