﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using FiresecAPI.Automation;

namespace AutomationModule.ViewModels
{
	public class ShowMessageStepViewModel : BaseStepViewModel
	{
		ShowMessageArguments ShowMessageArguments { get; set; }
		public ArgumentViewModel MessageParameter { get; private set; }
		public ProcedureLayoutCollectionViewModel ProcedureLayoutCollectionViewModel { get; private set; }

		public ShowMessageStepViewModel(StepViewModel stepViewModel) : base(stepViewModel)
		{
			ShowMessageArguments = stepViewModel.Step.ShowMessageArguments;
			MessageParameter = new ArgumentViewModel(ShowMessageArguments.MessageParameter, stepViewModel.Update);
			ExplicitTypes = new ObservableCollection<ExplicitType> (ProcedureHelper.GetEnumList<ExplicitType>().FindAll(x => x != ExplicitType.Object));
			EnumTypes = ProcedureHelper.GetEnumObs<EnumType>(); 
		}

		public override void UpdateContent()
		{
			List<Variable> allVariables;
			if (ExplicitType == ExplicitType.Enum)
				allVariables = ProcedureHelper.GetAllVariables(Procedure).FindAll(x => x.ExplicitType == ExplicitType && !x.IsList && x.EnumType == EnumType);
			else
				allVariables = ProcedureHelper.GetAllVariables(Procedure).FindAll(x => x.ExplicitType == ExplicitType && !x.IsList);
			MessageParameter.Update(allVariables);
			MessageParameter.ExplicitType = ExplicitType;
			MessageParameter.EnumType = EnumType;
			ProcedureLayoutCollectionViewModel = new ProcedureLayoutCollectionViewModel(ShowMessageArguments.ProcedureLayoutCollection);
			OnPropertyChanged(() => ProcedureLayoutCollectionViewModel);
		}

		public bool IsModalWindow
		{
			get { return ShowMessageArguments.IsModalWindow; }
			set
			{
				ShowMessageArguments.IsModalWindow = value;
				OnPropertyChanged(() => IsModalWindow);
			}
		}

		public override string Description
		{
			get 
			{
				return "Сообщение: " + MessageParameter.Description;
			}
		}

		public ObservableCollection<ExplicitType> ExplicitTypes { get; private set; }
		public ExplicitType ExplicitType
		{
			get
			{
				return ShowMessageArguments.ExplicitType;
			}
			set
			{
				ShowMessageArguments.ExplicitType = value;
				UpdateContent();
				OnPropertyChanged(() => ExplicitType);
			}
		}

		public ObservableCollection<EnumType> EnumTypes { get; private set; }
		public EnumType EnumType
		{
			get
			{
				return ShowMessageArguments.EnumType;
			}
			set
			{
				ShowMessageArguments.EnumType = value;
				UpdateContent();
				OnPropertyChanged(() => EnumType);
			}
		}
	}
}