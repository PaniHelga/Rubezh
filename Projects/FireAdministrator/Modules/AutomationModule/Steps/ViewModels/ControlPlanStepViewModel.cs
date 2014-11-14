﻿using System.Collections.ObjectModel;
using System.Linq;
using FiresecAPI.Automation;
using FiresecAPI.Models;
using FiresecClient;
using System.Collections.Generic;
using Infrustructure.Plans.Elements;
using Infrastructure.Common.Services;
using Infrustructure.Plans.Events;

namespace AutomationModule.ViewModels
{
	public class ControlPlanStepViewModel : BaseStepViewModel
	{
		public ArgumentViewModel ValueArgument { get; private set; }
		ControlPlanArguments ControlPlanArguments { get; set; }
		public ProcedureLayoutCollectionViewModel ProcedureLayoutCollectionViewModel { get; private set; }

		public ControlPlanStepViewModel(StepViewModel stepViewModel) : base(stepViewModel)
		{
			ControlPlanArguments = stepViewModel.Step.ControlPlanArguments;
			ValueArgument = new ArgumentViewModel(ControlPlanArguments.ValueArgument, stepViewModel.Update, UpdateContent);
			ElementPropertyTypes = new ObservableCollection<ElementPropertyType>();
			ControlVisualTypes = ProcedureHelper.GetEnumObs<ControlVisualType>();
			ServiceFactoryBase.Events.GetEvent<ElementChangedEvent>().Subscribe(OnElementsChanged);
			ServiceFactoryBase.Events.GetEvent<ElementAddedEvent>().Subscribe(OnElementsChanged);
			ServiceFactoryBase.Events.GetEvent<ElementRemovedEvent>().Subscribe(OnElementsChanged);
		}

		private void OnElementsChanged(List<ElementBase> elements)
		{
			UpdateContent();
		}

		public ObservableCollection<PlanViewModel> Plans { get; private set; }
		PlanViewModel _selectedPlan;
		public PlanViewModel SelectedPlan
		{
			get { return _selectedPlan; }
			set
			{
				_selectedPlan = value;
				if (_selectedPlan != null)
				{
					ControlPlanArguments.PlanUid = _selectedPlan.Plan.UID;
					Elements = ProcedureHelper.GetAllElements(_selectedPlan.Plan);
					SelectedElement = Elements.FirstOrDefault(x => x.Uid == ControlPlanArguments.ElementUid);
					OnPropertyChanged(() => Elements);
				}
				OnPropertyChanged(() => SelectedPlan);
			}
		}

		public ObservableCollection<ElementViewModel> Elements { get; private set; }
		ElementViewModel _selectedElement;
		public ElementViewModel SelectedElement
		{
			get { return _selectedElement; }
			set
			{
				_selectedElement = value;
				if (_selectedElement != null)
				{
					ControlPlanArguments.ElementUid = _selectedElement.Uid;
					ElementPropertyTypes = GetElemetProperties(_selectedElement);
					SelectedElementPropertyType = ElementPropertyTypes.FirstOrDefault(x => x == ControlPlanArguments.ElementPropertyType);
					OnPropertyChanged(()=>ElementPropertyTypes);
				}
				OnPropertyChanged(() => SelectedElement);
			}
		}

		public ObservableCollection<ElementPropertyType> ElementPropertyTypes { get; private set; }
		ElementPropertyType _selectedElementPropertyType;
		public ElementPropertyType SelectedElementPropertyType
		{
			get { return _selectedElementPropertyType; }
			set
			{
				_selectedElementPropertyType = value;
				ControlPlanArguments.ElementPropertyType = _selectedElementPropertyType;
				var explicitTypeViewModel = PropertyTypeToExplicitType(SelectedElementPropertyType);
				ValueArgument.Update(Procedure, explicitTypeViewModel.ExplicitType, explicitTypeViewModel.EnumType, isList: false);
				OnPropertyChanged(() => SelectedElementPropertyType);
			}
		}

		public ObservableCollection<ControlVisualType> ControlVisualTypes { get; private set; }
		public ControlVisualType SelectedControlVisualType
		{
			get { return ControlPlanArguments.ControlVisualType; }
			set
			{
				ControlPlanArguments.ControlVisualType = value;
				if (value == ControlVisualType.Get)
					ValueArgument.VariableScopes = new ObservableCollection<VariableScope> { VariableScope.LocalVariable, VariableScope.GlobalVariable };
				else
					ValueArgument.VariableScopes = ProcedureHelper.GetEnumObs<VariableScope>();
				if (ValueArgument.VariableScopes.All(x => x != ValueArgument.SelectedVariableScope))
					ValueArgument.SelectedVariableScope = ValueArgument.VariableScopes.FirstOrDefault();
				ValueArgument.Update();
				OnPropertyChanged(() => SelectedControlVisualType);
			}
		}

		ObservableCollection<ElementPropertyType> GetElemetProperties(ElementViewModel element)
		{
			var elementPropertyTypes = new ObservableCollection<ElementPropertyType>();
			if (element.ElementType == typeof(ElementRectangle) || element.ElementType == typeof(ElementEllipse))
				elementPropertyTypes = new ObservableCollection<ElementPropertyType> { ElementPropertyType.Height, ElementPropertyType.Width,
					ElementPropertyType.Color, ElementPropertyType.BackColor, ElementPropertyType.BorderThickness, ElementPropertyType.Left, ElementPropertyType.Top };
			if (element.ElementType == typeof(ElementPolygon))
				elementPropertyTypes = new ObservableCollection<ElementPropertyType> {ElementPropertyType.Color, ElementPropertyType.BackColor, ElementPropertyType.BorderThickness, ElementPropertyType.Left, ElementPropertyType.Top };
			if (element.ElementType == typeof(ElementPolyline))
				elementPropertyTypes = new ObservableCollection<ElementPropertyType> { ElementPropertyType.Color, ElementPropertyType.BorderThickness, ElementPropertyType.Left, ElementPropertyType.Top };
			if (element.ElementType == typeof(ElementTextBlock))
				elementPropertyTypes = ProcedureHelper.GetEnumObs<ElementPropertyType>();
			return elementPropertyTypes;
		}

		ExplicitTypeViewModel PropertyTypeToExplicitType(ElementPropertyType elementPropertyType)
		{
			if (elementPropertyType == ElementPropertyType.Height || elementPropertyType == ElementPropertyType.Width || elementPropertyType == ElementPropertyType.BorderThickness ||
				elementPropertyType == ElementPropertyType.FontSize || elementPropertyType == ElementPropertyType.Left || elementPropertyType == ElementPropertyType.Top)
				return new ExplicitTypeViewModel(ExplicitType.Integer);
			if (elementPropertyType == ElementPropertyType.FontBold || elementPropertyType == ElementPropertyType.FontItalic || elementPropertyType == ElementPropertyType.Stretch ||
				elementPropertyType == ElementPropertyType.WordWrap)
				return new ExplicitTypeViewModel(ExplicitType.Boolean);
			if (elementPropertyType == ElementPropertyType.Color || elementPropertyType == ElementPropertyType.BackColor || elementPropertyType == ElementPropertyType.ForegroundColor)
				return new ExplicitTypeViewModel(EnumType.ColorType);
			if (elementPropertyType == ElementPropertyType.Text)
				return new ExplicitTypeViewModel(ExplicitType.String);
			return new ExplicitTypeViewModel(ExplicitType.Integer);
		}

		public override void UpdateContent()
		{
			Plans = new ObservableCollection<PlanViewModel>();
			foreach (var plan in FiresecManager.PlansConfiguration.AllPlans)
			{
				Plans.Add(new PlanViewModel(plan));
			}
			SelectedPlan = Plans.FirstOrDefault(x => x.Plan.UID == ControlPlanArguments.PlanUid);
			OnPropertyChanged(() => Plans);
			ProcedureLayoutCollectionViewModel = new ProcedureLayoutCollectionViewModel(ControlPlanArguments.LayoutFilter);
			OnPropertyChanged(() => ProcedureLayoutCollectionViewModel);
		}

		public override string Description
		{
			get { return ""; }
		}
	}
}