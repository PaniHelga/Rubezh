﻿using FiresecAPI;
using Infrastructure.Common.Windows.ViewModels;

namespace GKModule.ViewModels
{
	public class ZoneTooltipViewModel : BaseViewModel
	{
		public SKDZone Zone { get; private set; }
		public SKDZoneState State { get; private set; }

		public ZoneTooltipViewModel(SKDZone zone)
		{
			State = zone.State;
			Zone = zone;
		}

		public void OnStateChanged()
		{
			OnPropertyChanged("State");
			OnPropertyChanged("Zone");
		}
	}
}