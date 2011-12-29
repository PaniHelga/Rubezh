﻿using System.Windows;
using System.Windows.Controls;

namespace DevicesModule.Views
{
    public partial class DirectionsView : UserControl
    {
        public DirectionsView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(DevicesView_Loaded);
            _directionDataGrid.SelectionChanged += new SelectionChangedEventHandler(DevicesView_Loaded);

            if (width != 0)
                leftColumn.Width = new System.Windows.GridLength(width);

            if (height != 0)
                bottomRow.Height = new System.Windows.GridLength(height);
        }

        static double width = 0;
        static double height = 0;

        private void OnWidthChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            width = leftColumn.Width.Value;
        }

        private void OnHeightChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            height = bottomRow.Height.Value;
        }

        void DevicesView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_directionDataGrid.SelectedItem != null)
                _directionDataGrid.ScrollIntoView(_directionDataGrid.SelectedItem);
        }
    }
}