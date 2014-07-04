﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using FiresecAPI.Automation;
using FiresecService.Processor;

namespace FiresecService
{
	public static class AutomationProcessor
	{
		static int timeValidator;
		static DateTime startTime;
		static int TimeDelta
		{
			get { return (int)((DateTime.Now - startTime).TotalSeconds); }
		}

		static Thread Thread;
		static bool IsStopping = false;
		static AutoResetEvent AutoResetEvent = new AutoResetEvent(false);

		public static void Start()
		{
			timeValidator = 0;
			startTime = DateTime.Now;
			Thread = new Thread(OnRun);
			Thread.Start();
		}

		public static void Stop()
		{
			IsStopping = true;
			if (AutoResetEvent != null)
			{
				AutoResetEvent.Set();
				if (Thread != null)
				{
					Thread.Join(TimeSpan.FromSeconds(2));
				}

			}
		}

		public static void SetNewConfig()
		{
			Stop();
			Start();
		}

		static void OnRun()
		{
			AutoResetEvent = new AutoResetEvent(false);
			while (true)
			{
				var shedules = ConfigurationCashHelper.SystemConfiguration.AutomationConfiguration.AutomationSchedules;
				
				if (AutoResetEvent.WaitOne(TimeSpan.FromSeconds(1)))
				{
					return;
				}

				timeValidator++;
				foreach (var schedule in shedules)
				{
					if (timeValidator <= TimeDelta)
					{
						var dateList = new List<DateTime>();
						for (int i = 0; i <= TimeDelta - timeValidator; i++)
						{
							dateList.Add(DateTime.Now - TimeSpan.FromSeconds(i));
						}
						dateList.Reverse();
						timeValidator = TimeDelta;
						foreach (var date in dateList)
						{
							//Trace.WriteLine(date.TimeOfDay);
							if (CheckSchedule(schedule, date))
								RunProcedures(schedule);
						}
					}
				}
			}
		}

		static bool CheckSchedule(AutomationSchedule schedule, DateTime dateTime)
		{
			return (((schedule.Year == dateTime.Year) || (schedule.Year == -1)) &&
					((schedule.Month == dateTime.Month) || (schedule.Month == -1)) &&
					((schedule.Day == dateTime.Day) || (schedule.Day == -1)) &&
					((schedule.Hour == dateTime.Hour) || (schedule.Hour == -1)) &&
					((schedule.Minute == dateTime.Minute) || (schedule.Minute == -1)) &&
					((schedule.Second == dateTime.Second) || (schedule.Second == -1)) &&
					((schedule.DayOfWeek.ToString() == dateTime.DayOfWeek.ToString()) || (schedule.DayOfWeek == DayOfWeekType.Any)));
		}

		static void RunProcedures(AutomationSchedule schedule)
		{
			if (!schedule.IsActive)
				return;
			foreach (var procedure in ConfigurationCashHelper.SystemConfiguration.AutomationConfiguration.Procedures.FindAll(x => x.IsActive))
			{
				var scheduleProcedure = schedule.ScheduleProcedures.FirstOrDefault(x => (x.ProcedureUid == procedure.Uid));
				if (scheduleProcedure != null)
					AutomationProcessorRunner.Run(procedure, scheduleProcedure.Arguments);
			}
		}
	}
}