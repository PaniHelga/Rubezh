﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.SKD;
using FiresecClient;
using FiresecClient.SKDHelpers;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;

namespace SKDModule.ViewModels
{
	public class TimeTrackDetailsViewModel : SaveCancelDialogViewModel
	{
		public DayTimeTrack DayTimeTrack { get; private set; }
		public ShortEmployee ShortEmployee { get; private set; }

		public TimeTrackDetailsViewModel(DayTimeTrack dayTimeTrack, ShortEmployee shortEmployee)
		{
			dayTimeTrack.Calculate();

			Title = "Время сотрудника " + shortEmployee.FIO + " в течение дня " + dayTimeTrack.Date.Date.ToString("yyyy-MM-dd");
			AddCommand = new RelayCommand(OnAdd);
			EditCommand = new RelayCommand(OnEdit, CanEdit);
			AddCustomPartCommand = new RelayCommand(OnAddCustomPart);
			RemovePartCommand = new RelayCommand(OnRemovePart, CanEditRemovePart);
			EditPartCommand = new RelayCommand(OnEditPart, CanEditRemovePart);
			DayTimeTrack = dayTimeTrack;
			ShortEmployee = shortEmployee;

			DayTimeTrackParts = new ObservableCollection<DayTimeTrackPartViewModel>();
			foreach (var timeTrackPart in DayTimeTrack.RealTimeTrackParts)
			{
				var employeeTimeTrackPartViewModel = new DayTimeTrackPartViewModel(timeTrackPart);
				DayTimeTrackParts.Add(employeeTimeTrackPartViewModel);
			}

			Documents = new ObservableCollection<DocumentViewModel>();
			foreach (var document in dayTimeTrack.Documents)
			{
				var documentViewModel = new DocumentViewModel(document);
				Documents.Add(documentViewModel);
			}
		}


		bool _IsChanged;
		public bool IsChanged
		{
			get { return _IsChanged; }
			set
			{
				_IsChanged = value;
				OnPropertyChanged(() => IsChanged);					
			}
		}
		
			
		public ObservableCollection<DayTimeTrackPartViewModel> DayTimeTrackParts { get; private set; }

		DayTimeTrackPartViewModel _selectedDayTimeTrackPart;
		public DayTimeTrackPartViewModel SelectedDayTimeTrackPart
		{
			get { return _selectedDayTimeTrackPart; }
			set
			{
				_selectedDayTimeTrackPart = value;
				OnPropertyChanged(() => SelectedDayTimeTrackPart);
			}
		}

		public ObservableCollection<DocumentViewModel> Documents { get; private set; }

		DocumentViewModel _selectedDocument;
		public DocumentViewModel SelectedDocument
		{
			get { return _selectedDocument; }
			set
			{
				_selectedDocument = value;
				OnPropertyChanged(() => SelectedDocument);
			}
		}


		public RelayCommand AddCustomPartCommand { get; private set; }
		void OnAddCustomPart()
		{
			var timeTrackPartDetailsViewModel = new TimeTrackPartDetailsViewModel(DayTimeTrack, ShortEmployee);
			if (DialogService.ShowModalWindow(timeTrackPartDetailsViewModel))
			{
				DayTimeTrackParts.Add(new DayTimeTrackPartViewModel(timeTrackPartDetailsViewModel.UID, timeTrackPartDetailsViewModel.EnterTime, timeTrackPartDetailsViewModel.ExitTime, timeTrackPartDetailsViewModel.SelectedZone));
				IsChanged = true;
			}
		}

		public RelayCommand RemovePartCommand { get; private set; }
		void OnRemovePart()
		{
			var result = PassJournalHelper.DeletePassJournal(SelectedDayTimeTrackPart.UID);
			if (result)
			{
				DayTimeTrackParts.Remove(SelectedDayTimeTrackPart);
				SelectedDayTimeTrackPart = DayTimeTrackParts.FirstOrDefault();
				IsChanged = true;
			}
		}
		bool CanEditRemovePart()
		{
			return SelectedDayTimeTrackPart != null;
		}

		public RelayCommand EditPartCommand { get; private set; }
		void OnEditPart()
		{
			var timeTrackPartDetailsViewModel = new TimeTrackPartDetailsViewModel(DayTimeTrack, ShortEmployee, SelectedDayTimeTrackPart.UID, SelectedDayTimeTrackPart.EnterTimeSpan, SelectedDayTimeTrackPart.ExitTimeSpan);
			if (DialogService.ShowModalWindow(timeTrackPartDetailsViewModel))
			{
				SelectedDayTimeTrackPart.Update(timeTrackPartDetailsViewModel.EnterTime, timeTrackPartDetailsViewModel.ExitTime, timeTrackPartDetailsViewModel.SelectedZone);
				IsChanged = true;
			}
		}


		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var timeTrackDocument = new TimeTrackDocument();
			timeTrackDocument.StartDateTime = DayTimeTrack.Date.Date;
			timeTrackDocument.EndDateTime = DayTimeTrack.Date.Date + new TimeSpan(23, 59, 59);
			var documentDetailsViewModel = new DocumentDetailsViewModel(false, ShortEmployee.OrganisationUID, timeTrackDocument);
			if (DialogService.ShowModalWindow(documentDetailsViewModel))
			{
				var document = documentDetailsViewModel.TimeTrackDocument;
				document.EmployeeUID = ShortEmployee.UID;
				var operationResult = FiresecManager.FiresecService.AddTimeTrackDocument(document);
				if (operationResult.HasError)
				{
					MessageBoxService.ShowWarning(operationResult.Error);
				}
				else
				{
					var documentViewModel = new DocumentViewModel(document);
					Documents.Add(documentViewModel);
					SelectedDocument = documentViewModel;
				}
			}
		}

		public RelayCommand EditCommand { get; private set; }
		void OnEdit()
		{
			var documentDetailsViewModel = new DocumentDetailsViewModel(false, ShortEmployee.OrganisationUID, SelectedDocument.Document);
			if (DialogService.ShowModalWindow(documentDetailsViewModel))
			{
				var document = documentDetailsViewModel.TimeTrackDocument;
				var operationResult = FiresecManager.FiresecService.EditTimeTrackDocument(document);
				if (operationResult.HasError)
				{
					MessageBoxService.ShowWarning(operationResult.Error);
				}
				SelectedDocument.Update();
			}
		}
		bool CanEdit()
		{
			return SelectedDocument != null && SelectedDocument.Document.StartDateTime.Date == DayTimeTrack.Date.Date;
		}

		protected override bool Save()
		{
			return base.Save();
		}
	}
}