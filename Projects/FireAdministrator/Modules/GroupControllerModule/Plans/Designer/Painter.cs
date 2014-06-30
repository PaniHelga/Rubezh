﻿using System.Windows.Media;
using DeviceControls;
using FiresecAPI.Models;
using Infrastructure.Client.Plans.ViewModels;
using Infrustructure.Plans.Designer;
using Infrustructure.Plans.Painters;
using FiresecAPI.GK;

namespace GKModule.Plans.Designer
{
	public class Painter : PointPainter
	{
		private ElementXDevice _elementXDevice;
		private ImageTextTooltipViewModel _toolTip;
		public Painter(CommonDesignerCanvas designerCanvas, ElementXDevice elementXDevice)
			: base(designerCanvas, elementXDevice)
		{
			_elementXDevice = elementXDevice;
			_toolTip = new ImageTextTooltipViewModel();
		}

		protected override Brush GetBrush()
		{
			var xdevice = GKPlanExtension.Instance.GetItem<XDevice>(_elementXDevice);
			_toolTip.ImageSource = xdevice == null ? null : xdevice.Driver.ImageSource;
			return PictureCacheSource.XDevicePicture.GetBrush(xdevice);
		}

		public override object GetToolTip(string title)
		{
			_toolTip.Title = title;
			return _toolTip;
		}
	}
}