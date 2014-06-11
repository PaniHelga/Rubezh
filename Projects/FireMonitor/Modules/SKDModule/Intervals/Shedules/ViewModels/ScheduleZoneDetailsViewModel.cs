﻿using System.Collections.Generic;
using System.Linq;
using FiresecAPI.EmployeeTimeIntervals;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using System.Collections.ObjectModel;

namespace SKDModule.ViewModels
{
	public class ScheduleZoneDetailsViewModel : SaveCancelDialogViewModel
	{
		private Schedule _schedule;
		public ScheduleZone ScheduleZone { get; private set; }

		public ScheduleZoneDetailsViewModel(Schedule schedule, ScheduleZone sheduleZone = null)
		{
			_schedule = schedule;
			if (sheduleZone == null)
			{
				Title = "Выбор помещения";
				sheduleZone = new ScheduleZone()
				{
					ScheduleUID = schedule.UID,
				};
			}
			else
				Title = "Редактирование помещения";
			ScheduleZone = sheduleZone;
			IsControl = sheduleZone.IsControl;

			Zones = new ObservableCollection<ZoneViewModel>();
			foreach (var zone in FiresecAPI.SKD.SKDManager.Zones)
			{
				var zoneViewModel = new ZoneViewModel(zone);
				Zones.Add(zoneViewModel);
			}
			SelectedZone = Zones.FirstOrDefault(x => x.Zone.UID == ScheduleZone.ZoneUID);

		}
		public ObservableCollection<ZoneViewModel> Zones { get; private set; }

		ZoneViewModel _selectedZone;
		public ZoneViewModel SelectedZone
		{
			get { return _selectedZone; }
			set
			{
				_selectedZone = value;
				OnPropertyChanged(() => SelectedZone);
			}
		}

		private bool _isControl;
		public bool IsControl
		{
			get { return _isControl; }
			set
			{
				_isControl = value;
				OnPropertyChanged("IsControl");
			}
		}

		protected override bool CanSave()
		{
			return SelectedZone != null;
		}
		protected override bool Save()
		{
			if (SelectedZone != null)
			{
				if (_schedule.Zones.Any(x => x.ZoneUID == SelectedZone.Zone.UID && ScheduleZone.UID != x.UID))
				{
					MessageBoxService.ShowWarning("Выбранная зона уже включена");
					return false;
				}
			}
			ScheduleZone.ZoneUID = SelectedZone.Zone.UID;
			ScheduleZone.IsControl = IsControl;
			return true;
		}
	}
}