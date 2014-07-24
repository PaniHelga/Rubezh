﻿using System;
using System.Linq;
using System.Threading;
using ChinaSKDDriverAPI;
using FiresecAPI;
using FiresecAPI.GK;
using FiresecAPI.SKD;
using FiresecAPI.Journal;
using System.Collections.Generic;
using ChinaSKDDriverNativeApi;

namespace ChinaSKDDriver
{
	public class DeviceProcessor
	{
		public Wrapper Wrapper { get; private set; }
		public SKDDevice Device { get; private set; }
		public int LoginID { get; private set; }
		public bool IsConnected { get; private set; }
		Thread Thread;
		bool IsStopping;
		static AutoResetEvent AutoResetEvent = new AutoResetEvent(false);
		public event Action<JournalItem> NewJournalItem;

		public DeviceProcessor(SKDDevice device)
		{
			Device = device;
			Wrapper = new Wrapper();
			Wrapper.NewJournalItem -= new Action<SKDJournalItem>(Wrapper_NewJournalItem);
			Wrapper.NewJournalItem += new Action<SKDJournalItem>(Wrapper_NewJournalItem);
		}

		void Wrapper_NewJournalItem(SKDJournalItem skdJournalItem)
		{
			if (skdJournalItem.LoginID == LoginID)
			{
				var journalItem = new JournalItem();
				journalItem.SystemDateTime = skdJournalItem.SystemDateTime;
				journalItem.DeviceDateTime = skdJournalItem.DeviceDateTime;
				journalItem.JournalEventNameType = skdJournalItem.JournalEventNameType;
				journalItem.DescriptionText = skdJournalItem.Description;
				journalItem.CardNo = skdJournalItem.CardNo;

				switch (skdJournalItem.JournalEventNameType)
				{
					case JournalEventNameType.Потеря_связи:
						OnConnectionChanged(true);
						return;

					case JournalEventNameType.Восстановление_связи:
						OnConnectionChanged(false);
						return;

					case JournalEventNameType.Проход_разрешен:
					case JournalEventNameType.Проход_запрещен:
						var readerDevice = Device.Children.FirstOrDefault(x => x.DriverType == SKDDriverType.Reader && x.IntAddress == skdJournalItem.DoorNo);
						if (readerDevice != null)
						{
							journalItem.JournalObjectType = JournalObjectType.SKDDevice;
							journalItem.ObjectUID = readerDevice.UID;
						}
						journalItem.DescriptionText = "Метод открытия: " + skdJournalItem.emOpenMethod.ToDescription();
						journalItem.JournalDetalisationItems.Add(new JournalDetalisationItem("Направление", skdJournalItem.emEventType.ToDescription()));

						if (skdJournalItem.emOpenMethod == NativeWrapper.NET_ACCESS_DOOROPEN_METHOD.NET_ACCESS_DOOROPEN_METHOD_CARD)
						{
							journalItem.JournalDetalisationItems.Add(new JournalDetalisationItem("Тип карты", skdJournalItem.emCardType.ToDescription()));
							journalItem.JournalDetalisationItems.Add(new JournalDetalisationItem("Номер карты", skdJournalItem.CardNo.ToString()));
						}
						if (skdJournalItem.emOpenMethod == NativeWrapper.NET_ACCESS_DOOROPEN_METHOD.NET_ACCESS_DOOROPEN_METHOD_PWD_ONLY)
						{
							journalItem.JournalDetalisationItems.Add(new JournalDetalisationItem("Пароль", skdJournalItem.szPwd.ToString()));
							journalItem.CardNo = 0;
						}
						break;

					case JournalEventNameType.Дверь_не_закрыта:
					case JournalEventNameType.Взлом:
					case JournalEventNameType.Повторный_проход:
					case JournalEventNameType.Принуждение:
						var device = Device.Children.FirstOrDefault(x => x.DriverType == SKDDriverType.Lock && x.IntAddress == skdJournalItem.DoorNo);
						if (device != null)
						{
							journalItem.JournalObjectType = JournalObjectType.SKDDevice;
							journalItem.ObjectUID = device.UID;
						}

						if (skdJournalItem.JournalEventNameType == JournalEventNameType.Принуждение)
						{
							journalItem.JournalDetalisationItems.Add(new JournalDetalisationItem("Номер карты", skdJournalItem.CardNo.ToString()));
						}
						break;

					case JournalEventNameType.Открытие_двери:
					case JournalEventNameType.Закрытие_двери:
					case JournalEventNameType.Неизвестный_статус_двери:
						var doorDevice = Device.Children.FirstOrDefault(x => x.DriverType == SKDDriverType.Lock && x.IntAddress == skdJournalItem.DoorNo);
						if (doorDevice != null)
						{
							journalItem.JournalObjectType = JournalObjectType.SKDDevice;
							journalItem.ObjectUID = doorDevice.UID;

							doorDevice.State.StateClass = EventDescriptionAttributeHelper.ToStateClass(skdJournalItem.JournalEventNameType);
							doorDevice.State.StateClasses = new List<XStateClass>() { doorDevice.State.StateClass };
							var skdStates = new SKDStates();
							skdStates.DeviceStates.Add(doorDevice.State);
							Processor.DoCallback(skdStates);
						}
						break;
				}

				if (NewJournalItem != null)
					NewJournalItem(journalItem);
			}
		}

		void OnConnectionChanged(bool isConnectionLost, bool fireJournalItem = true)
		{
			if (fireJournalItem)
			{
				var journalItem = new JournalItem();
				journalItem.SystemDateTime = DateTime.Now;
				journalItem.DeviceDateTime = DateTime.Now;
				if (isConnectionLost)
					journalItem.JournalEventNameType = JournalEventNameType.Потеря_связи;
				else
					journalItem.JournalEventNameType = JournalEventNameType.Восстановление_связи;
				journalItem.JournalObjectType = JournalObjectType.SKDDevice;
				journalItem.ObjectUID = Device.UID;
				journalItem.ObjectName = Device.Name;
				if (NewJournalItem != null)
					NewJournalItem(journalItem);
			}

			var connectionLostSKDStates = new SKDStates();

			var allDevices = new List<SKDDevice>(Device.Children);
			allDevices.Add(Device);

			foreach (var device in allDevices)
			{
				if (isConnectionLost)
				{
					device.State.StateClass = XStateClass.ConnectionLost;
				}
				else
				{
					if (device.DriverType == SKDDriverType.Lock)
					{
						var result = Wrapper.GetDoorStatus(device.IntAddress);
						switch (result)
						{
							case 1:
								device.State.StateClass = XStateClass.On;
								break;

							case 2:
								device.State.StateClass = XStateClass.Off;
								break;

							default:
								device.State.StateClass = XStateClass.Unknown;
								break;
						}
					}
					else
					{
						device.State.StateClass = XStateClass.Norm;
					}
				}
				device.State.StateClasses = new List<XStateClass>() { device.State.StateClass };
				connectionLostSKDStates.DeviceStates.Add(device.State);
			}

			Processor.DoCallback(connectionLostSKDStates);
		}

		public void Start()
		{
			Device.State.StateClass = XStateClass.Unknown;
			Device.State.StateClasses = new List<XStateClass>() { Device.State.StateClass };
			foreach (var child in Device.Children)
			{
				child.State.StateClass = XStateClass.Unknown;
				child.State.StateClasses = new List<XStateClass>() { child.State.StateClass };
			}

			IsStopping = false;
			AutoResetEvent = new AutoResetEvent(false);
			Thread = new Thread(OnStart);
			Thread.Start();
		}

		public void Stop()
		{
			Wrapper.Disconnect();
			IsStopping = true;
			if (AutoResetEvent != null)
			{
				AutoResetEvent.Set();
				if (Thread != null)
				{
					Thread.Join(TimeSpan.FromSeconds(1));
				}
			}
		}

		void OnStart()
		{
			var attemptCount = 0;

			while (true)
			{
				attemptCount++;
				try
				{
					Connect();
					IsConnected = LoginID > 0;
					if (IsConnected)
					{
						Thread = null;
						OnConnectionChanged(false, attemptCount > 2);
						break;
					}

					if (attemptCount == 2)
					{
						OnConnectionChanged(true);
					}

					if (IsStopping)
						return;
					if (AutoResetEvent.WaitOne(TimeSpan.FromSeconds(5)))
					{
						return;
					}
				}
				catch { }
			}
		}

		public void Reconnect()
		{
			LoginID = 0;
			Start();
		}

		public void Connect()
		{
			var addresss = "";
			var port = 0;
			var login = "";
			var password = "";

			var addressProperty = Device.Properties.FirstOrDefault(x => x.Name == "Address");
			if (addressProperty != null)
			{
				addresss = addressProperty.StringValue;
			}
			var portProperty = Device.Properties.FirstOrDefault(x => x.Name == "Port");
			if (portProperty != null)
			{
				port = portProperty.Value;
			}
			var loginProperty = Device.Properties.FirstOrDefault(x => x.Name == "Login");
			if (portProperty != null)
			{
				login = loginProperty.StringValue;
			}
			var passwordProperty = Device.Properties.FirstOrDefault(x => x.Name == "Password");
			if (passwordProperty != null)
			{
				password = passwordProperty.StringValue;
			}
			LoginID = Wrapper.Connect(addresss, port, login, password);
		}
	}
}