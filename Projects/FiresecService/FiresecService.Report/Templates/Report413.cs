﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FiresecService.Report.DataSources;
using System.Data;
using System.Linq;
using FiresecAPI.SKD.ReportFilters;
using System.Collections.Generic;
using FiresecAPI.SKD;
using FiresecAPI;
using SKDDriver;
using Common;

namespace FiresecService.Report.Templates
{
    public partial class Report413 : BaseReport
	{
		public Report413()
		{
			InitializeComponent();
		}

		public override string ReportTitle
		{
            get { return "Права доступа сотрудников/посетителей"; }
		}
        protected override DataSet CreateDataSet(DataProvider dataProvider)
		{
			var filter = GetFilter<ReportFilter413>();

			var useEmployeesFilter = dataProvider.IsEmployeeFilter(filter);
            var employees = new List<Guid>();
			if (useEmployeesFilter)
				employees = dataProvider.GetEmployees(filter).Select(item => item.UID).ToList();

			var cardFilter = new CardFilter();
            var cardsResult = dataProvider.DatabaseService.CardTranslator.Get(cardFilter);

			var dataSet = new DataSet413();
			if (!cardsResult.HasError)
			{
				foreach (var card in cardsResult.Result)
				{
                    if (useEmployeesFilter && !employees.Contains(card.EmployeeUID))
                        continue;

					if (filter.PassCardPermanent || filter.PassCardTemprorary || filter.PassCardOnceOnly || filter.PassCardForcing || filter.PassCardLocked)
					{
						if (filter.PassCardPermanent && card.CardType != CardType.Constant)
							continue;
						if (filter.PassCardTemprorary && card.CardType != CardType.Temporary)
							continue;
						if (filter.PassCardOnceOnly && card.CardType != CardType.OneTime)
							continue;
						if (filter.PassCardForcing && card.CardType != CardType.Duress)
							continue;
						if (filter.PassCardLocked && card.CardType != CardType.Blocked)
							continue;
					}

					if (filter.PassCardActive && card.IsDeleted)
						continue;

                    var employeeResult = dataProvider.DatabaseService.EmployeeTranslator.GetSingle(card.EmployeeUID);
                    var accessTemplateResult = dataProvider.DatabaseService.AccessTemplateTranslator.GetSingle(card.AccessTemplateUID);

					var cardDoors = new List<CardDoor>();
					if (accessTemplateResult.Result != null)
					{
						cardDoors = accessTemplateResult.Result.CardDoors.ToList();
					}
					cardDoors.AddRange(card.CardDoors.ToList());

					foreach (var cardDoor in cardDoors)
					{
						var door = SKDManager.Doors.FirstOrDefault(x => x.UID == cardDoor.DoorUID);
						if (door == null)
						{
							continue;
						}

						var dataRow = dataSet.Data.NewDataRow();
						dataRow.Type = card.CardType.ToDescription();
						dataRow.Number = card.Number.ToString();
						if (employeeResult.Result != null)
						{
							dataRow.Employee = employeeResult.Result.FIO;
                            var organisationResult = dataProvider.DatabaseService.OrganisationTranslator.GetSingle(employeeResult.Result.OrganisationUID);
							if (organisationResult.Result != null)
							{
								dataRow.Organisation = organisationResult.Result.Name;
							}
							if (employeeResult.Result.Department != null)
							{
								dataRow.Department = employeeResult.Result.Department.Name;
							}
							if (employeeResult.Result.Position != null)
							{
								dataRow.Position = employeeResult.Result.Position.Name;
							}
						}

						dataRow.AccessPoint = door.PresentationName;
						if (door.InDevice != null && door.InDevice.Zone != null)
						{
							dataRow.ZoneIn = door.InDevice.Zone.PresentationName;
							var enterSchedule = SKDManager.SKDConfiguration.TimeIntervalsConfiguration.WeeklyIntervals.FirstOrDefault(x => x.ID == cardDoor.EnterScheduleNo);
							if(enterSchedule != null)
							{
								dataRow.Enter = enterSchedule.Name;
							}

							if (filter.Zones != null && filter.Zones.Count > 0 && filter.ZoneIn)
							{
								if (!filter.Zones.Contains(door.InDevice.Zone.UID))
									continue;
							}
							if (filter.Schedules != null && filter.Schedules.Count > 0 && filter.ScheduleEnter)
							{
								if (!filter.Schedules.Contains(cardDoor.EnterScheduleNo))
									continue;
							}
						}
						if (door.OutDevice != null && door.OutDevice.Zone != null)
						{
							dataRow.ZoneOut = door.OutDevice.Zone.PresentationName;
							var exitSchedule = SKDManager.SKDConfiguration.TimeIntervalsConfiguration.WeeklyIntervals.FirstOrDefault(x => x.ID == cardDoor.ExitScheduleNo);
							if (exitSchedule != null)
							{
								dataRow.Exit = exitSchedule.Name;
							}

							if (filter.Zones != null && filter.Zones.Count > 0 && filter.ZoneOut)
							{
								if (!filter.Zones.Contains(door.OutDevice.Zone.UID))
									continue;
							}
							if (filter.Schedules != null && filter.Schedules.Count > 0 && filter.ScheduleExit)
							{
								if (!filter.Schedules.Contains(cardDoor.ExitScheduleNo))
									continue;
							}
						}

						dataSet.Data.Rows.Add(dataRow);
					}
				}
			}
			return dataSet;
		}
	}
}
