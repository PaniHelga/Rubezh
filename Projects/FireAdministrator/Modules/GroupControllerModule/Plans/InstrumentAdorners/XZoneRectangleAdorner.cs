﻿using FiresecAPI.Models;
using GKModule.Plans.Designer;
using GKModule.Plans.ViewModels;
using Infrastructure.Common.Windows;
using Infrustructure.Plans.Designer;
using PlansModule.InstrumentAdorners;

namespace GKModule.Plans.InstrumentAdorners
{
	public class XZoneRectangleAdorner : BaseRectangleAdorner
	{
		public XZoneRectangleAdorner(CommonDesignerCanvas designerCanvas)
			: base(designerCanvas)
		{
		}

		protected override Infrustructure.Plans.Elements.ElementBaseRectangle CreateElement()
		{
			var element = new ElementRectangleXZone();
			var propertiesViewModel = new XZonePropertiesViewModel(element);
			DialogService.ShowModalWindow(propertiesViewModel);
			Helper.SetXZone(element);
			return element;
		}
	}
}