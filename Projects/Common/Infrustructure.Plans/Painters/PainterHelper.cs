﻿using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Infrustructure.Plans.Elements;

namespace Infrustructure.Plans.Painters
{
	public static class PainterHelper
	{
		public static void SetStyle(Shape shape, ElementBase element)
		{
			shape.Fill = CreateBackgroundBrush(element);
			shape.Stroke = new SolidColorBrush(element.BorderColor);
			shape.StrokeThickness = element.BorderThickness;
		}
		public static PointCollection GetPoints(ElementBase element)
		{
			return
				element is ElementBaseShape ?
					Normalize(element.GetRectangle().TopLeft, ((ElementBaseShape)element).Points, element.BorderThickness) :
				element is ElementBaseRectangle ?
					Normalize(element.GetRectangle(), element.BorderThickness) :
					new PointCollection();
		}
		public static PointCollection GetRealPoints(ElementBase element)
		{
			if (element is ElementBaseShape)
				return ((ElementBaseShape)element).Points;
			else if (element is ElementBaseRectangle)
			{
				var rect = element.GetRectangle();
				return new PointCollection()
				{
					rect.TopLeft, 
					rect.TopRight,
					rect.BottomRight,
					rect.BottomLeft
				};
			}
			return new PointCollection();
		}
		public static PointCollection Normalize(Rect rectangle, double thickness)
		{
			double shift = thickness / 2;
			var pointCollection = new PointCollection();
			pointCollection.Add(new Point(shift, shift));
			pointCollection.Add(new Point(rectangle.Width + shift, shift));
			pointCollection.Add(new Point(rectangle.Width + shift, rectangle.Height + shift));
			pointCollection.Add(new Point(shift, rectangle.Height + shift));
			return pointCollection;
		}
		public static PointCollection Normalize(Point topLeftPoint, PointCollection points, double thickness)
		{
			double shift = thickness / 2;
			var pointCollection = new PointCollection();
			foreach (var point in points)
				pointCollection.Add(new Point(point.X - topLeftPoint.X + shift, point.Y - topLeftPoint.Y + shift));
			return pointCollection;
		}
		public static Brush CreateBrush(byte[] backgroundPixels)
		{
			using (var imageStream = new MemoryStream(backgroundPixels))
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = imageStream;
				bitmapImage.EndInit();
				bitmapImage.Freeze();
				return new ImageBrush(bitmapImage);
			}
		}

		public static Brush CreateTransparentBrush(double zoom = 1)
		{
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new System.Uri(string.Format("pack://application:,,,/{0};component/Resources/Transparent.bmp", Assembly.GetExecutingAssembly()));
			bi.EndInit();
			return new ImageBrush(bi)
			{
				TileMode = TileMode.Tile,
				ViewportUnits = BrushMappingMode.Absolute,
				Viewport = new Rect(0, 0, 16 / zoom, 16 / zoom)
			};
		}
		public static Brush CreateBackgroundBrush(ElementBase element)
		{
			if (element.BackgroundPixels != null)
				return PainterHelper.CreateBrush(element.BackgroundPixels);
			else
				return new SolidColorBrush(element.BackgroundColor);
		}
	}
}