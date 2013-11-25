﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.AvalonDock;

namespace FireMonitor.Layout.Views
{
	public partial class MonitorLayoutShellView : UserControl
	{
		public static readonly DependencyProperty ManagerProperty = DependencyProperty.Register("Manager", typeof(DockingManager), typeof(MonitorLayoutShellView), new UIPropertyMetadata(null));
		public DockingManager Manager
		{
			get { return (DockingManager)GetValue(ManagerProperty); }
			set { SetValue(ManagerProperty, value); }
		}

		public MonitorLayoutShellView()
		{
			InitializeComponent();
			Loaded += new RoutedEventHandler(MonitorLayoutShellView_Loaded);
		}

		private void MonitorLayoutShellView_Loaded(object sender, RoutedEventArgs e)
		{
			var binding = new Binding("Manager") { Mode = BindingMode.OneWayToSource };
			var expression = SetBinding(ManagerProperty, binding);
			Manager = manager;
		}
	}
}