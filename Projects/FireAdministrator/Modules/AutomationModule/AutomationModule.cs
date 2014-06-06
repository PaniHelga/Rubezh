﻿using System;
using System.Collections.Generic;
using AutomationModule.Events;
using AutomationModule.Validation;
using AutomationModule.ViewModels;
using Infrastructure.Client;
using Infrastructure.Common;
using Infrastructure.Common.Navigation;
using Infrastructure.Common.Validation;

namespace AutomationModule
{
	public class AutomationModule : ModuleBase, IValidationModule
	{
		SoundsViewModel SoundsViewModel;
		ProceduresViewModel ProceduresViewModel;
		SchedulesViewModel SchedulesViewModel;

		public override void CreateViewModels()
		{
			SoundsViewModel = new SoundsViewModel();
			ProceduresViewModel = new ProceduresViewModel();
			SchedulesViewModel = new SchedulesViewModel();
		}

		public override void Initialize()
		{
			SoundsViewModel.Initialize();
			ProceduresViewModel.Initialize();
			SchedulesViewModel.Initialize();
		}
		public override IEnumerable<NavigationItem> CreateNavigation()
		{
			return new List<NavigationItem>
				{
					new NavigationItem("Автоматизация", "/Controls;component/Images/tree.png",
						new List<NavigationItem>()
						{
							new NavigationItem<ShowAutomationSoundsEvent, Guid>(SoundsViewModel, "Звуки", "/Controls;component/Images/Music.png"),
							new NavigationItem<ShowProceduresEvent>(ProceduresViewModel, "Процедуры", "/Controls;component/Images/Tree.png"),
							new NavigationItem<ShowAutomationSchedulesEvents>(SchedulesViewModel, "Расписания", "/Controls;component/Images/SelectNone.png")
						}),
				};
		}
		public override string Name
		{
			get { return "Автоматизация"; }
		}
		public override void RegisterResource()
		{
			base.RegisterResource();
			var resourceService = new ResourceService();
			resourceService.AddResource(new ResourceDescription(GetType().Assembly, "Sounds/DataTemplates/Dictionary.xaml"));
			resourceService.AddResource(new ResourceDescription(GetType().Assembly, "Procedures/DataTemplates/Dictionary.xaml"));
			resourceService.AddResource(new ResourceDescription(GetType().Assembly, "Schedules/DataTemplates/Dictionary.xaml"));
		}

		public IEnumerable<IValidationError> Validate()
		{
			var validator = new Validator();
			return validator.Validate();
		}
	}
}