﻿using System;
using System.Linq;
using System.Threading;
using Common;
using Infrastructure.Common;

namespace GKProcessor
{
	public partial class Watcher
	{
		bool IsStopping = false;
		AutoResetEvent StopEvent;
		Thread RunThread;
		public GkDatabase GkDatabase { get; private set; }
		public DateTime LastUpdateTime { get; private set; }
		DateTime LastMissmatchCheckTime;

		public Watcher(GkDatabase gkDatabase)
		{
			GkDatabase = gkDatabase;
		}

		public void StartThread()
		{
			IsStopping = false;
			if (RunThread == null)
			{
				StopEvent = new AutoResetEvent(false);
				RunThread = new Thread(OnRunThread);
				RunThread.Start();
			}
		}

		public void StopThread()
		{
			IsStopping = true;
			if (StopEvent != null)
			{
				StopEvent.Set();
			}
			if (RunThread != null)
			{
				RunThread.Join(TimeSpan.FromSeconds(5));
			}
			RunThread = null;
		}

		void OnRunThread()
		{
			try
			{
				GetAllStates(true);
				if (!IsAnyDBMissmatch)
				{
					ReadMissingJournalItems();
				}
			}
			catch (Exception e)
			{
				Logger.Error(e, "JournalWatcher.OnRunThread GetAllStates");
			}

			while (true)
			{
				if (CheckLicense())
				{
					if (WatcherManager.IsConfigurationReloading)
					{
						if ((DateTime.Now - WatcherManager.LastConfigurationReloadingTime).TotalSeconds > 100)
							WatcherManager.IsConfigurationReloading = false;
					}
					if (!WatcherManager.IsConfigurationReloading)
					{
						if (IsAnyDBMissmatch)
						{
							if ((DateTime.Now - LastMissmatchCheckTime).TotalSeconds > 60)
							{
								GetAllStates(false);
								LastMissmatchCheckTime = DateTime.Now;
							}
						}
						else
						{
							try
							{
								CheckTasks();
							}
							catch (Exception e)
							{
								Logger.Error(e, "JournalWatcher.OnRunThread CheckTasks");
							}

							try
							{
								CheckDelays();
							}
							catch (Exception e)
							{
								Logger.Error(e, "JournalWatcher.OnRunThread CheckNPT");
							}

							try
							{
								PingJournal();
							}
							catch (Exception e)
							{
								Logger.Error(e, "JournalWatcher.OnRunThread PingJournal");
							}

							try
							{
								PingNextState();
							}
							catch (Exception e)
							{
								Logger.Error(e, "JournalWatcher.OnRunThread PingNextState");
							}
						}
					}
				}

				if (StopEvent != null)
				{
					var pollInterval = 10;
					var property = GkDatabase.RootDevice.Properties.FirstOrDefault(x => x.Name == "PollInterval");
					if (property != null)
					{
						pollInterval = property.Value;
					}
					if (StopEvent.WaitOne(pollInterval))
						break;
				}

				LastUpdateTime = DateTime.Now;
			}
		}
	}
}