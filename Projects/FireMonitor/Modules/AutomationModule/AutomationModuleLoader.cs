﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutomationModule.Events;
using AutomationModule.Plans;
using AutomationModule.ViewModels;
using FiresecAPI;
using FiresecAPI.GK;
using FiresecAPI.Models;
using FiresecAPI.Models.Layouts;
using FiresecClient;
using Infrastructure.Client;
using Infrastructure.Client.Layout;
using Infrastructure.Common;
using Infrastructure.Common.Navigation;
using Infrastructure.Common.Services;
using Infrastructure.Common.Services.Layout;
using Infrastructure.Common.Windows;
using Infrustructure.Plans.Events;

namespace AutomationModule
{
	public class AutomationModuleLoader : ModuleBase, ILayoutProviderModule
	{
		PlanPresenter _planPresenter;
		ProceduresViewModel ProceduresViewModel;
		NavigationItem _proceduresNavigationItem;

		public override void CreateViewModels()
		{
			_planPresenter = new PlanPresenter();
			ProceduresViewModel = new ProceduresViewModel();
			ProcessShedule();
		}

		void ProcessShedule()
		{
		}

		public override void Initialize()
		{
			_proceduresNavigationItem.IsVisible = FiresecManager.SystemConfiguration.AutomationConfiguration.Procedures.Count > 0;
			ProceduresViewModel.Initialize();
			_planPresenter.Initialize();
			ServiceFactoryBase.Events.GetEvent<RegisterPlanPresenterEvent<Plan, XStateClass>>().Publish(_planPresenter);
		}

		public override IEnumerable<NavigationItem> CreateNavigation()
		{
			_proceduresNavigationItem = new NavigationItem<ShowAutomationEvent, object>(ProceduresViewModel, ModuleType.ToDescription(), "/Controls;component/Images/Video1.png");
			return new List<NavigationItem>
			{
				_proceduresNavigationItem
			};
		}

		protected override ModuleType ModuleType
		{
			get { return ModuleType.Automation; }
		}
		public override void RegisterResource()
		{
			base.RegisterResource();
			ServiceFactoryBase.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "DataTemplates/Dictionary.xaml"));
		}
		public override void Dispose()
		{
		}

		public override void AfterInitialize()
		{
			SafeFiresecService.AutomationEvent -= OnAutomationCallback;
			SafeFiresecService.AutomationEvent += OnAutomationCallback;
		}

		void OnAutomationCallback(AutomationCallbackResult automationCallbackResult)
		{
			ApplicationService.Invoke(() =>
			{
				switch (automationCallbackResult.AutomationCallbackType)
				{
					case AutomationCallbackType.Sound:
						var sound = FiresecManager.SystemConfiguration.AutomationConfiguration.AutomationSounds.FirstOrDefault(x => x.Uid == automationCallbackResult.SoundUID);
						if (sound != null)
							AlarmPlayerHelper.Play(FileHelper.GetSoundFilePath(Path.Combine(ServiceFactoryBase.ContentService.ContentFolder, sound.Uid.ToString())), BeeperType.Alarm, false);
						break;

					case AutomationCallbackType.Message:
						MessageBoxService.Show(automationCallbackResult.Message, "Сообщение", automationCallbackResult.IsModalWindow);
						break;
				}
			});
		}

		#region ILayoutProviderModule Members
		public IEnumerable<ILayoutPartPresenter> GetLayoutParts()
		{
			yield return new LayoutPartPresenter(LayoutPartIdentities.AutomationProcedure, "Процедура", "Procedures.png", p => new LayoutProcedurePartViewModel((LayoutPartProcedureProperties)p));
		}
		#endregion
	}
}