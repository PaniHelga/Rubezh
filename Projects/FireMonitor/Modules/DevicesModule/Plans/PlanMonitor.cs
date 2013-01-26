﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using Infrustructure.Plans.Elements;
using DevicesModule.Plans.Designer;

namespace DevicesModule.Plans
{
	internal class PlanMonitor
	{
		private Plan _plan;
		private Action _callBack;
		private List<DeviceState> _deviceStates;
		private List<ZoneState> _zoneStates;

		public PlanMonitor(Plan plan, Action callBack)
		{
			_plan = plan;
			_callBack = callBack;
			_deviceStates = new List<DeviceState>();
			_zoneStates = new List<ZoneState>();
			Initialize();
		}
		private void Initialize()
		{
			_plan.ElementDevices.ForEach(item => Initialize(item));
			_plan.ElementRectangleZones.ForEach(item => Initialize(item));
			_plan.ElementPolygonZones.ForEach(item => Initialize(item));
		}
		private void Initialize(ElementDevice element)
		{
			var device = Helper.GetDevice(element);
			if (device != null)
			{
				_deviceStates.Add(device.DeviceState);
				device.DeviceState.StateChanged += _callBack;
			}
		}
		private void Initialize(IElementZone element)
		{
			if (element.ZoneUID != Guid.Empty)
			{
				var zone = Helper.GetZone(element);
				if (zone != null)
				{
					_zoneStates.Add(zone.ZoneState);
					zone.ZoneState.StateChanged += _callBack;
				}
			}
		}

		public StateType GetState()
		{
			var result = StateType.No;
			foreach (var state in _deviceStates)
			{
				if (state.StateType < result)
					result = state.StateType;
			}
			foreach (var state in _zoneStates)
			{
				if (state.StateType < result)
					result = state.StateType;
			}
			return result;
		}
	}
}
