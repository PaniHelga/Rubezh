﻿using System.Collections.Generic;
using FiresecAPI.Models;
using FiresecAPI.Models.Layouts;
using Infrastructure;
using Infrastructure.Client;
using Infrastructure.Client.Layout;
using Infrastructure.Common;
using Infrastructure.Common.Navigation;
using Infrastructure.Common.Services.Layout;
using Infrastructure.Events;
using Infrustructure.Plans.Events;
using VideoModule.Plans;
using VideoModule.Plans.Designer;
using VideoModule.ViewModels;
using CamerasViewModel = VideoModule.ViewModels.CamerasViewModel;

namespace VideoModule
{
	public class VideoModule : ModuleBase, ILayoutDeclarationModule
	{
		CamerasViewModel CamerasViewModel;
		PlanExtension _planExtension;

		public override void CreateViewModels()
		{
			CamerasViewModel = new CamerasViewModel();
			_planExtension = new PlanExtension(CamerasViewModel);
		}

		public override void Initialize()
		{
			_planExtension.Initialize();
			ServiceFactory.Events.GetEvent<RegisterPlanExtensionEvent<Plan>>().Publish(_planExtension);
			Helper.BuildMap();
		}
		public override void RegisterResource()
		{
			base.RegisterResource();
			ServiceFactory.ResourceService.AddResource(new ResourceDescription(GetType().Assembly, "Plans/DataTemplates/Dictionary.xaml"));
		}
		public override IEnumerable<NavigationItem> CreateNavigation()
		{
			return new List<NavigationItem>()
			{
				new NavigationItem<ShowVideoEvent>(CamerasViewModel,"Видео", "/Controls;component/Images/Video1.png"),
			};
		}
		public override string Name
		{
			get { return "Видео"; }
		}
		public override void Dispose()
		{
			VideoService.Close();
		}

		public IEnumerable<ILayoutPartDescription> GetLayoutPartDescriptions()
		{
			yield return new LayoutPartDescription(LayoutPartDescriptionGroup.Video, LayoutPartIdentities.CamerasList, 203, "Список камер", "Панель список камер", "BVideo.png");
			yield return new LayoutPartDescription(LayoutPartDescriptionGroup.Video, LayoutPartIdentities.CameraVideo, 204, "Одна камера", "Панель видео с камеры", "BVideo.png")
			{
				Factory = (p) => new LayoutPartCameraViewModel(p as LayoutPartCameraProperties),
			};
			yield return new LayoutPartDescription(LayoutPartDescriptionGroup.Video, LayoutPartIdentities.MultiCamera, 205, "Раскладка камер", "Панель раскладки камер", "BVideo.png", false);
		}
	}
}