﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Common.Services.Layout;
using System.Collections.ObjectModel;
using System.Windows;

namespace LayoutModule.ViewModels
{
	public class LayoutPartPropertyGeneralPageViewModel : LayoutPartPropertyPageViewModel
	{
		private LayoutPartSize _layoutPartSize;
		private bool _initialized;
		public LayoutPartPropertyGeneralPageViewModel(LayoutPartSize layoutPartSize)
		{
			_initialized = false;
			_layoutPartSize = layoutPartSize;
			UnitTypes = new ObservableCollection<GridUnitType>(Enum.GetValues(typeof(GridUnitType)).Cast<GridUnitType>());
			_initialized = true;
		}

		public ObservableCollection<GridUnitType> UnitTypes { get; private set; }

		private GridUnitType _widthType;
		public GridUnitType WidthType
		{
			get { return _widthType; }
			set
			{
				if (WidthType != value)
				{
					_widthType = value;
					OnPropertyChanged(() => WidthType);
					if (_initialized && WidthType == GridUnitType.Auto)
						Width = _layoutPartSize.PreferedSize.Width;
				}
			}
		}
		private GridUnitType _heightType;
		public GridUnitType HeightType
		{
			get { return _heightType; }
			set
			{
				if (HeightType != value)
				{
					_heightType = value;
					OnPropertyChanged(() => HeightType);
					if (_initialized && HeightType == GridUnitType.Auto)
						Height = _layoutPartSize.PreferedSize.Height;
				}
			}
		}
		private double _width;
		public double Width
		{
			get { return _width; }
			set
			{
				_width = value;
				OnPropertyChanged(() => Width);
			}
		}
		private double _height;
		public double Height
		{
			get { return _height; }
			set
			{
				_height = value;
				OnPropertyChanged(() => Height);
			}
		}
		private double _minWidth;
		public double MinWidth
		{
			get { return _minWidth; }
			set
			{
				_minWidth = value;
				OnPropertyChanged(() => MinWidth);
			}
		}
		private double _minHeight;
		public double MinHeight
		{
			get { return _minHeight; }
			set
			{
				_minHeight = value;
				OnPropertyChanged(() => MinHeight);
			}
		}
		private bool _isWidthFixed;
		public bool IsWidthFixed
		{
			get { return _isWidthFixed; }
			set
			{
				_isWidthFixed = value;
				OnPropertyChanged(() => IsWidthFixed);
			}
		}
		private bool _isHeightFixed;
		public bool IsHeightFixed
		{
			get { return _isHeightFixed; }
			set
			{
				_isHeightFixed = value;
				OnPropertyChanged(() => IsHeightFixed);
			}
		}
		private int _margin;
		public int Margin
		{
			get { return _margin; }
			set
			{
				_margin = value;
				OnPropertyChanged(() => Margin);
			}
		}

		public override string Header
		{
			get { return "Общее"; }
		}
		public override void CopyProperties()
		{
			Height = _layoutPartSize.Height;
			HeightType = _layoutPartSize.HeightType;
			IsHeightFixed = _layoutPartSize.IsHeightFixed;
			IsWidthFixed = _layoutPartSize.IsWidthFixed;
			MinHeight = _layoutPartSize.MinHeight;
			MinWidth = _layoutPartSize.MinWidth;
			Width = _layoutPartSize.Width;
			WidthType = _layoutPartSize.WidthType;
			Margin = _layoutPartSize.Margin;
		}
		public override bool CanSave()
		{
			return true;
		}
		public override bool Save()
		{
			if (_layoutPartSize.Height != Height || _layoutPartSize.HeightType != HeightType || _layoutPartSize.IsHeightFixed != IsHeightFixed || _layoutPartSize.IsWidthFixed != IsWidthFixed || _layoutPartSize.MinHeight != MinHeight || _layoutPartSize.MinWidth != MinWidth || _layoutPartSize.Width != Width || _layoutPartSize.WidthType != WidthType || _layoutPartSize.Margin != Margin)
			{
				_layoutPartSize.Height = Height;
				_layoutPartSize.HeightType = HeightType;
				_layoutPartSize.IsHeightFixed = IsHeightFixed;
				_layoutPartSize.IsWidthFixed = IsWidthFixed;
				_layoutPartSize.MinHeight = MinHeight;
				_layoutPartSize.MinWidth = MinWidth;
				_layoutPartSize.Width = Width;
				_layoutPartSize.WidthType = WidthType;
				_layoutPartSize.Margin = Margin;
				return true;
			}
			return false;
		}
	}
}