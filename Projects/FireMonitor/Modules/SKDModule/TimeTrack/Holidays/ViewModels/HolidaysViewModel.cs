﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.SKD;
using FiresecClient;
using FiresecClient.SKDHelpers;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using SKDModule.Common;

namespace SKDModule.ViewModels
{
	public class HolidaysViewModel : ViewPartViewModel, ISelectable<Guid>
	{
		HolidayFilter Filter;

		public HolidaysViewModel()
		{
			AddCommand = new RelayCommand(OnAdd, CanAdd);
			RemoveCommand = new RelayCommand(OnRemove, CanRemove);
			EditCommand = new RelayCommand(OnEdit, CanEdit);
			ShowSettingsCommand = new RelayCommand(OnShowSettings);

			InitializeYears();
		}

		public void Initialize(HolidayFilter filter)
		{
			var organisations = OrganisationHelper.GetByCurrentUser();
			if (organisations == null)
				return;
			var holidays = HolidayHelper.Get(filter);
			if (holidays == null)
				return;

			AllHolidays = new List<HolidayViewModel>();
			Organisations = new List<HolidayViewModel>();
			foreach (var organisation in organisations)
			{
				var organisationViewModel = new HolidayViewModel(organisation);
				Organisations.Add(organisationViewModel);
				AllHolidays.Add(organisationViewModel);
				foreach (var holiday in holidays)
				{
					if (holiday.OrganisationUID == organisation.UID)
					{
						var holidayViewModel = new HolidayViewModel(organisation, holiday);
						organisationViewModel.AddChild(holidayViewModel);
						AllHolidays.Add(holidayViewModel);
					}
				}
			}
			OnPropertyChanged(() => Organisations);
			SelectedHoliday = Organisations.FirstOrDefault();
		}

		public List<HolidayViewModel> Organisations { get; private set; }
		List<HolidayViewModel> AllHolidays { get; set; }

		public void Select(Guid holidayUID)
		{
			if (holidayUID != Guid.Empty)
			{
				var holidayViewModel = AllHolidays.FirstOrDefault(x => x.Holiday != null && x.Holiday.UID == holidayUID);
				if (holidayViewModel != null)
					holidayViewModel.ExpandToThis();
				SelectedHoliday = holidayViewModel;
			}
		}

		HolidayViewModel _selectedHoliday;
		public HolidayViewModel SelectedHoliday
		{
			get { return _selectedHoliday; }
			set
			{
				_selectedHoliday = value;
				if (value != null)
					value.ExpandToThis();
				OnPropertyChanged(() => SelectedHoliday);
			}
		}

		public HolidayViewModel ParentOrganisation
		{
			get
			{
				HolidayViewModel OrganisationViewModel = SelectedHoliday;
				if (!OrganisationViewModel.IsOrganisation)
					OrganisationViewModel = SelectedHoliday.Parent;

				if (OrganisationViewModel.Organisation != null)
					return OrganisationViewModel;

				return null;
			}
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var holidayDetailsViewModel = new HolidayDetailsViewModel(SelectedHoliday.Organisation, SelectedYear);
			if (DialogService.ShowModalWindow(holidayDetailsViewModel))
			{
				var holidayViewModel = new HolidayViewModel(SelectedHoliday.Organisation, holidayDetailsViewModel.Holiday);

				HolidayViewModel OrganisationViewModel = SelectedHoliday;
				if (!OrganisationViewModel.IsOrganisation)
					OrganisationViewModel = SelectedHoliday.Parent;

				if (OrganisationViewModel == null || OrganisationViewModel.Organisation == null)
					return;

				OrganisationViewModel.AddChild(holidayViewModel);
				SelectedHoliday = holidayViewModel;
			}
		}
		bool CanAdd()
		{
			return SelectedHoliday != null;
		}

		public RelayCommand RemoveCommand { get; private set; }
		void OnRemove()
		{
			HolidayViewModel OrganisationViewModel = SelectedHoliday;
			if (!OrganisationViewModel.IsOrganisation)
				OrganisationViewModel = SelectedHoliday.Parent;

			if (OrganisationViewModel == null || OrganisationViewModel.Organisation == null)
				return;

			var index = OrganisationViewModel.Children.ToList().IndexOf(SelectedHoliday);
			var holiday = SelectedHoliday.Holiday;
			bool removeResult = HolidayHelper.MarkDeleted(holiday);
			if (!removeResult)
				return;
			OrganisationViewModel.RemoveChild(SelectedHoliday);
			index = Math.Min(index, OrganisationViewModel.Children.Count() - 1);
			if (index > -1)
				SelectedHoliday = OrganisationViewModel.Children.ToList()[index];
			else
				SelectedHoliday = OrganisationViewModel;
		}
		bool CanRemove()
		{
			return SelectedHoliday != null && !SelectedHoliday.IsOrganisation;
		}

		public RelayCommand EditCommand { get; private set; }
		void OnEdit()
		{
			var holidayDetailsViewModel = new HolidayDetailsViewModel(SelectedHoliday.Organisation, SelectedYear, SelectedHoliday.Holiday);
			if (DialogService.ShowModalWindow(holidayDetailsViewModel))
			{
				SelectedHoliday.Update(holidayDetailsViewModel.Holiday);
			}
		}
		bool CanEdit()
		{
			return SelectedHoliday != null && SelectedHoliday.Parent != null && !SelectedHoliday.IsOrganisation;
		}

		Holiday CopyHoliday(Holiday source, bool newName = true)
		{
			var copy = new Holiday();
			copy.Name = newName ? CopyHelper.CopyName(source.Name, ParentOrganisation.Children.Select(item => item.Name)) : source.Name;
			copy.Type = source.Type;
			copy.Date = source.Date;
			copy.TransferDate = source.TransferDate;
			copy.Reduction = source.Reduction;
			copy.OrganisationUID = ParentOrganisation.Organisation.UID;
			return copy;
		}

		void InitializeYears()
		{
			AvailableYears = new ObservableCollection<int>();
			for (int i = 2014; i <= 2020; i++)
				AvailableYears.Add(i);
			SelectedYear = AvailableYears.FirstOrDefault(x => x == DateTime.Now.Year);
		}

		ObservableCollection<int> _availableYears;
		public ObservableCollection<int> AvailableYears
		{
			get { return _availableYears; }
			set
			{
				_availableYears = value;
				OnPropertyChanged(() => AvailableYears);
			}
		}

		int _selectedYear;
		public int SelectedYear
		{
			get { return _selectedYear; }
			set
			{
				_selectedYear = value;
				OnPropertyChanged(() => SelectedYear);

				Filter = new HolidayFilter() { UserUID = FiresecManager.CurrentUser.UID, Year = value };
				Initialize(Filter);
			}
		}

		public RelayCommand ShowSettingsCommand { get; private set; }
		void OnShowSettings()
		{
			var nightSettingsViewModel = new NightSettingsViewModel(ParentOrganisation.Organisation.UID);
			DialogService.ShowModalWindow(nightSettingsViewModel);
		}
	}
}