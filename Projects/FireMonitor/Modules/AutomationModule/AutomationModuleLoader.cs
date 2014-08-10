﻿using System.Collections.Generic;
using AutomationModule.Events;
using AutomationModule.ViewModels;
using FiresecClient;
using Infrastructure;
using Infrastructure.Client;
using Infrastructure.Common;
using Infrastructure.Common.Navigation;
using FiresecAPI;
using System;
using Infrastructure.Common.Windows;
using System.Linq;
using FiresecAPI.Models;
using Infrastructure.Common.Services;
using System.IO;

namespace AutomationModule
{
	public class AutomationModuleLoader : ModuleBase
	{
		ProceduresViewModel ProceduresViewModel;
		NavigationItem _proceduresNavigationItem;

		public override void CreateViewModels()
		{
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
		}

		public override IEnumerable<NavigationItem> CreateNavigation()
		{
			_proceduresNavigationItem = new NavigationItem<ShowAutomationEvent, object>(ProceduresViewModel, "Автоматизация", "/Controls;component/Images/Video1.png");
			return new List<NavigationItem>()
			{
				_proceduresNavigationItem
			};
		}

		public override string Name
		{
			get { return "Автоматизация"; }
		}
		public override void RegisterResource()
		{
			base.RegisterResource();
			ServiceFactory.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "DataTemplates/Dictionary.xaml"));
		}
		public override void Dispose()
		{
		}

		public override void AfterInitialize()
		{
			SafeFiresecService.AutomationEvent -= new Action<AutomationCallbackResult>(OnAutomationCallback);
			SafeFiresecService.AutomationEvent += new Action<AutomationCallbackResult>(OnAutomationCallback);
		}

		void OnAutomationCallback(AutomationCallbackResult automationCallbackResult)
		{
			ApplicationService.Invoke(() =>
			{
				switch (automationCallbackResult.AutomationCallbackType)
				{
					case AutomationCallbackType.Sound:
						var sound = FiresecClient.FiresecManager.SystemConfiguration.AutomationConfiguration.AutomationSounds.FirstOrDefault(x => x.Uid == automationCallbackResult.SoundUID);
						AlarmPlayerHelper.Play(FiresecClient.FileHelper.GetSoundFilePath(Path.Combine(ServiceFactoryBase.ContentService.ContentFolder, sound.Uid.ToString())), BeeperType.Alarm, false);
						break;

					case AutomationCallbackType.Message:
						var message = automationCallbackResult.Message;
						MessageBoxService.Show(message, "Сообщение");
						break;
				}
			});
		}
	}
}