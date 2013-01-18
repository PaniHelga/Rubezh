﻿using System.Windows.Controls;
using Infrustructure.Plans.Elements;
using PlansModule.Designer.Adorners;
using System.Windows;

namespace PlansModule.Designer.DesignerItems
{
	public class DesignerItemPoint : DesignerItemBase
	{
		public DesignerItemPoint(ElementBase element)
			: base(element)
		{
			SetResizeChrome(new ResizeChromePoint(this));
		}

		public override void UpdateZoomPoint()
		{
			Redraw();
			ResizeChrome.InvalidateVisual();
		}

		public override Rect GetRectangle()
		{
			var rect = base.GetRectangle();
			if (DesignerCanvas == null)
				return rect;
			return new Rect(rect.Left - DesignerCanvas.PointZoom / 2, rect.Top - DesignerCanvas.PointZoom / 2, DesignerCanvas.PointZoom, DesignerCanvas.PointZoom);
		}
	}
}
