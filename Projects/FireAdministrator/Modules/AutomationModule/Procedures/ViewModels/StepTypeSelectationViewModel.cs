﻿using System.Collections.Generic;
using FiresecAPI.Automation;
using Infrastructure.Common.Windows.ViewModels;

namespace AutomationModule.ViewModels
{
	public class StepTypeSelectationViewModel : SaveCancelDialogViewModel
	{
		public StepTypeSelectationViewModel()
		{
			Title = "Выбор типа функции";

			BuildStepTypeTree();
			FillAllStepTypes();

			RootStepType.IsExpanded = true;
			SelectedStepType = RootStepType;
			foreach (var stepType in AllStepTypes)
			{
				stepType.ExpandToThis();
			}

			OnPropertyChanged(() => RootStepTypes);
		}

		public List<StepTypeViewModel> AllStepTypes;

		public void FillAllStepTypes()
		{
			AllStepTypes = new List<StepTypeViewModel>();
			AddChildPlainStepTypes(RootStepType);
		}

		void AddChildPlainStepTypes(StepTypeViewModel parentViewModel)
		{
			AllStepTypes.Add(parentViewModel);
			foreach (var childViewModel in parentViewModel.Children)
				AddChildPlainStepTypes(childViewModel);
		}

		StepTypeViewModel _selectedStepType;
		public StepTypeViewModel SelectedStepType
		{
			get { return _selectedStepType; }
			set
			{
				_selectedStepType = value;
				OnPropertyChanged(() => SelectedStepType);
			}
		}

		StepTypeViewModel _rootStepType;
		public StepTypeViewModel RootStepType
		{
			get { return _rootStepType; }
			private set
			{
				_rootStepType = value;
				OnPropertyChanged(() => RootStepType);
			}
		}

		public StepTypeViewModel[] RootStepTypes
		{
			get { return new[] { RootStepType }; }
		}

		void BuildStepTypeTree()
		{
			RootStepType = new StepTypeViewModel("Реестр функций",
				new List<StepTypeViewModel>
				{
					new StepTypeViewModel("Операции",
						new List<StepTypeViewModel>
						{
							new StepTypeViewModel(ProcedureStepType.Arithmetics),
							new StepTypeViewModel(ProcedureStepType.SetValue),
							new StepTypeViewModel(ProcedureStepType.IncrementValue),
							new StepTypeViewModel(ProcedureStepType.FindObjects),
							new StepTypeViewModel(ProcedureStepType.GetObjectProperty),
							new StepTypeViewModel(ProcedureStepType.Random),
							new StepTypeViewModel(ProcedureStepType.GenerateGuid)
						}),
						new StepTypeViewModel("Функции управления списками",
							new List<StepTypeViewModel>
						{
							new StepTypeViewModel(ProcedureStepType.ChangeList),
							new StepTypeViewModel(ProcedureStepType.GetListCount),
							new StepTypeViewModel(ProcedureStepType.GetListItem)
						}),
						new StepTypeViewModel("Интерактивная логика",
						new List<StepTypeViewModel>
						{
							new StepTypeViewModel(ProcedureStepType.PlaySound),
							new StepTypeViewModel(ProcedureStepType.AddJournalItem),
							new StepTypeViewModel(ProcedureStepType.SendEmail),
							new StepTypeViewModel(ProcedureStepType.ShowMessage),
							new StepTypeViewModel(ProcedureStepType.ControlVisualGet),
							new StepTypeViewModel(ProcedureStepType.ControlVisualSet),
							new StepTypeViewModel(ProcedureStepType.ControlPlanGet),
							new StepTypeViewModel(ProcedureStepType.ControlPlanSet),
							new StepTypeViewModel(ProcedureStepType.ShowDialog),
							new StepTypeViewModel(ProcedureStepType.ShowProperty)
						}),
					new StepTypeViewModel("Служебные функции",
						new List<StepTypeViewModel>
						{
							new StepTypeViewModel(ProcedureStepType.Exit),
							new StepTypeViewModel(ProcedureStepType.RunProgram),
							new StepTypeViewModel(ProcedureStepType.Pause),
							new StepTypeViewModel(ProcedureStepType.ProcedureSelection),
							new StepTypeViewModel(ProcedureStepType.CheckPermission),
							new StepTypeViewModel(ProcedureStepType.GetJournalItem)
						}),
					new StepTypeViewModel("Функции цикла",
						new List<StepTypeViewModel>
						{
							new StepTypeViewModel(ProcedureStepType.For),
							new StepTypeViewModel(ProcedureStepType.While),
							new StepTypeViewModel(ProcedureStepType.Break),
							new StepTypeViewModel(ProcedureStepType.Continue)
						}),
					new StepTypeViewModel("Управление аппаратурой",
						new List<StepTypeViewModel>
						{
							new StepTypeViewModel("Управление ГК",
								new List<StepTypeViewModel>
								{
									new StepTypeViewModel(ProcedureStepType.ControlGKDevice),
									new StepTypeViewModel(ProcedureStepType.ControlGKFireZone),
									new StepTypeViewModel(ProcedureStepType.ControlGKGuardZone),
									new StepTypeViewModel(ProcedureStepType.ControlDirection),
									new StepTypeViewModel(ProcedureStepType.ControlDelay)
								}),
							new StepTypeViewModel("Управление СКД",
								new List<StepTypeViewModel>
								{
									new StepTypeViewModel(ProcedureStepType.ControlSKDDevice),
									new StepTypeViewModel(ProcedureStepType.ControlSKDZone),
									new StepTypeViewModel(ProcedureStepType.ControlDoor),
									new StepTypeViewModel(ProcedureStepType.ExportJournal),
									new StepTypeViewModel(ProcedureStepType.ExportConfiguration),
									new StepTypeViewModel(ProcedureStepType.ExportOrganisation),
									new StepTypeViewModel(ProcedureStepType.ImportOrganisation),
									new StepTypeViewModel(ProcedureStepType.ExportOrganisationList),
									new StepTypeViewModel(ProcedureStepType.ImportOrganisationList),
								}),
							new StepTypeViewModel("Управление Видео",
								new List<StepTypeViewModel>
								{
									new StepTypeViewModel(ProcedureStepType.ControlCamera)
								}),
						}),

				});
		}

		protected override bool CanSave()
		{
			return ((SelectedStepType != null) && (!SelectedStepType.IsFolder));
		}
	}
}