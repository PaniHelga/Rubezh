﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Services.Layout;
using FiresecAPI.Models.Layouts;
using Infrastructure.Client.Layout.ViewModels;
using FiresecClient;
using FiresecAPI.Models;

namespace VideoModule.ViewModels
{
	public class LayoutPartCameraViewModel : LayoutPartTitleViewModel
	{
		private LayoutPartCameraProperties _properties;

		public LayoutPartCameraViewModel(LayoutPartCameraProperties properties)
		{
			Title = "Камера";
			IconSource = LayoutPartDescription.IconPath + "BVideo.png";
			_сameraTitle = null;
			_properties = properties ?? new LayoutPartCameraProperties();
			var selectedCamera = FiresecManager.SystemConfiguration.Cameras.FirstOrDefault(item => item.UID == _properties.SourceUID);
			UpdateLayoutPart(selectedCamera);
		}

		public override ILayoutProperties Properties
		{
			get { return _properties; }
		}
		public override IEnumerable<LayoutPartPropertyPageViewModel> PropertyPages
		{
			get
			{
				yield return new LayoutPartPropertyCameraPageViewModel(this);
			}
		}

		private string _сameraTitle;
		public string CameraTitle
		{
			get { return _сameraTitle; }
			set
			{
				_сameraTitle = value;
				OnPropertyChanged(() => CameraTitle);
			}
		}

		public void UpdateLayoutPart(Camera selectedCamera)
		{
			CameraTitle = selectedCamera == null ? Title : selectedCamera.Name;
		}
	}
}