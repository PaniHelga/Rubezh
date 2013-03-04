﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common;
using Infrastructure.Common.Navigation;

namespace Infrastructure.Common.Windows.ViewModels
{
	public class ShellViewModel : ApplicationViewModel
	{
		public ShellViewModel()
		{
			AllowHelp = true;
			AllowMaximize = true;
			AllowMinimize = true;
			ContentFotter = null;
			ContentHeader = null;
			MinWidth = 800;
			MinHeight = 600;
			ContentItems = new ObservableCollection<IViewPartViewModel>();
			MinimizeCommand = new RelayCommand(OnMinimize);
		}

		public RelayCommand MinimizeCommand { get; private set; }
		void OnMinimize()
		{
			TextVisibility = TextVisibility ? false : true;
			MinimizeButtonContent = TextVisibility ? "<<" : ">>";
		}

		private string minimizeButtonContent = "<<";
		public string MinimizeButtonContent
		{
			get { return minimizeButtonContent; }
			set
			{
				minimizeButtonContent = value;
				OnPropertyChanged("MinimizeButtonContent");
			}
		}

		private bool textVisibility = true;
		public bool TextVisibility
		{
			get { return textVisibility; }
			set
			{
				textVisibility = value;
				OnPropertyChanged("TextVisibility");
			}
		}

		private BaseViewModel _contentHeader;
		public BaseViewModel ContentHeader
		{
			get { return _contentHeader; }
			set
			{
				_contentHeader = value;
				OnPropertyChanged("ContentHeader");
			}
		}
		private BaseViewModel _contentFotter;
		public BaseViewModel ContentFotter
		{
			get { return _contentFotter; }
			set
			{
				_contentFotter = value;
				OnPropertyChanged("ContentFotter");
			}
		}
		private List<NavigationItem> _navigationItems;
		public List<NavigationItem> NavigationItems
		{
			get { return _navigationItems; }
			set
			{
				_navigationItems = value;
				foreach (var navigationItem in _navigationItems)
				{
					navigationItem.Context = this;
					if (navigationItem.Childs != null)
						foreach (var navigationItemChild in navigationItem.Childs)
						{
							navigationItemChild.Context = this;
						}
				}
				OnPropertyChanged("NavigationItems1");
			}
		}

		private double _minWidth;
		public double MinWidth
		{
			get { return _minWidth; }
			set
			{
				_minWidth = value;
				OnPropertyChanged("MinWidth");
			}
		}
		private double _minHeight;
		public double MinHeight
		{
			get { return _minHeight; }
			set
			{
				_minHeight = value;
				OnPropertyChanged("MinHeight");
			}
		}
		private double _height;
		public double Height
		{
			get { return _height; }
			set
			{
				_height = value;
				OnPropertyChanged("Height");
			}
		}
		private double _width;
		public double Width
		{
			get { return _width; }
			set
			{
				_width = value;
				OnPropertyChanged("Width");
			}
		}

		private ObservableCollection<IViewPartViewModel> _contentItems;
		public ObservableCollection<IViewPartViewModel> ContentItems
		{
			get { return _contentItems; }
			set
			{
				_contentItems = value;
				OnPropertyChanged("ContentItems");
			}
		}
	}
}