﻿using System;
using System.Collections.ObjectModel;
using FiresecAPI.SKD;
using FiresecClient.SKDHelpers;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using SKDModule.Model;

namespace SKDModule.ViewModels
{
	public class TimeTrackingViewModel : ViewPartViewModel
	{
		EmployeeFilter _employeeFilter;
		TimeTrackingSettings _settings;

		public TimeTrackingViewModel()
		{
			_employeeFilter = new EmployeeFilter()
			{
				UserUID = FiresecClient.FiresecManager.CurrentUser.UID,
			};

			_settings = new TimeTrackingSettings()
			{
				Period = TimeTrackingPeriod.CurrentMonth,
				StartDate = DateTime.Today.AddDays(1 - DateTime.Today.Day),
				EndDate = DateTime.Today
			};
			ShowFilterCommand = new RelayCommand(OnShowFilter);
			ShowSettingsCommand = new RelayCommand(OnShowSettings);
			RefreshCommand = new RelayCommand(OnRefresh);
			PrintCommand = new RelayCommand(OnPrint);
			UpdateGrid();
		}

		ObservableCollection<TimeTrackViewModel> _timeTracks;
		public ObservableCollection<TimeTrackViewModel> TimeTracks
		{
			get { return _timeTracks; }
			set
			{
				_timeTracks = value;
				OnPropertyChanged(() => TimeTracks);
			}
		}

		TimeTrackViewModel _selectedTimeTrack;
		public TimeTrackViewModel SelectedTimeTrack
		{
			get { return _selectedTimeTrack; }
			set
			{
				_selectedTimeTrack = value;
				OnPropertyChanged(() => SelectedTimeTrack);
			}
		}

		int _totalDays;
		public int TotalDays
		{
			get { return _totalDays; }
			set
			{
				_totalDays = value;
				OnPropertyChanged(() => TotalDays);
			}
		}

		DateTime _firstDay;
		public DateTime FirstDay
		{
			get { return _firstDay; }
			set
			{
				_firstDay = value;
				OnPropertyChanged(() => FirstDay);
			}
		}

		public RelayCommand ShowFilterCommand { get; private set; }
		void OnShowFilter()
		{
			var employeeFilter = new EmployeeFilter()
			{
				OrganisationUIDs = _employeeFilter.OrganisationUIDs,
				DepartmentUIDs = _employeeFilter.DepartmentUIDs,
				PositionUIDs = _employeeFilter.PositionUIDs,
				Appointed = _employeeFilter.Appointed,
				PersonType = _employeeFilter.PersonType
			};
			var filter = new HRFilter()
			{
				EmployeeFilter = employeeFilter,
				RemovalDates = _employeeFilter.RemovalDates,
				UIDs = _employeeFilter.UIDs,
				LogicalDeletationType = _employeeFilter.LogicalDeletationType,
			};
			var filterViewModel = new HRFilterViewModel(filter);
			if (DialogService.ShowModalWindow(filterViewModel))
			{
				_employeeFilter.OrganisationUIDs = filterViewModel.Filter.OrganisationUIDs;
				_employeeFilter.DepartmentUIDs = filterViewModel.Filter.EmployeeFilter.DepartmentUIDs;
				_employeeFilter.PositionUIDs = filterViewModel.Filter.EmployeeFilter.PositionUIDs;
				_employeeFilter.Appointed = filterViewModel.Filter.EmployeeFilter.Appointed;
				_employeeFilter.PersonType = filterViewModel.Filter.EmployeeFilter.PersonType;
				_employeeFilter.RemovalDates = filterViewModel.Filter.RemovalDates;
				_employeeFilter.UIDs = filterViewModel.Filter.UIDs;
				_employeeFilter.LogicalDeletationType = filterViewModel.Filter.LogicalDeletationType;
				UpdateGrid();
			}
		}

		public RelayCommand ShowSettingsCommand { get; private set; }
		void OnShowSettings()
		{
			var settingsViewModel = new TimeTrackingSettingsViewModel(_settings);
			if (DialogService.ShowModalWindow(settingsViewModel))
				UpdateGrid();
		}

		public RelayCommand RefreshCommand { get; private set; }
		void OnRefresh()
		{
			UpdateGrid();
		}

		public RelayCommand PrintCommand { get; private set; }
		void OnPrint()
		{
			MessageBoxService.Show("Not Implemented");
		}

		void UpdateGrid()
		{
			using (new WaitWrapper())
			{
				TotalDays = (int)(_settings.EndDate - _settings.StartDate).TotalDays + 1;
				FirstDay = _settings.StartDate;
				TimeTracks = new ObservableCollection<TimeTrackViewModel>();
				var timeTrackResult = EmployeeHelper.GetTimeTracks(_employeeFilter, _settings.StartDate, _settings.EndDate);
				foreach (var timeTrackEmployeeResult in timeTrackResult.TimeTrackEmployeeResults)
				{
					TimeTracks.Add(new TimeTrackViewModel(timeTrackEmployeeResult.ShortEmployee, timeTrackEmployeeResult.DayTimeTracks));
				}
			}
		}
	}
}