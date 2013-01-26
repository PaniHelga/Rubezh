﻿using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using DeviceControls;
using FiresecAPI;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Events;
using Infrustructure.Plans.Elements;
using Infrustructure.Plans.Painters;
using Infrustructure.Plans.Presenter;
using XFiresecAPI;
using System.Windows.Controls;

namespace GKModule.Plans.Designer
{
	class XDevicePainter : PointPainter
	{
		private PresenterItem _presenterItem;
		//private XDeviceControl _xdeviceControl;
		private XDevice _xdevice;
		private ContextMenu _contextMenu;

		public XDevicePainter(PresenterItem presenterItem)
			: base(presenterItem.Element)
		{
			_contextMenu = null;
			var elementXDevice = presenterItem.Element as ElementXDevice;
			if (elementXDevice != null)
			{
				_xdevice = Helper.GetXDevice(elementXDevice);
				if (_xdevice != null)
				{
					//_xdeviceControl = new XDeviceControl();
					//_xdeviceControl.DriverId = _xdevice.DriverUID;
					//_xdeviceControl.StateClass = _xdevice.DeviceState.StateClass;
					_xdevice.DeviceState.StateChanged += OnPropertyChanged;
				}
			}
			_presenterItem = presenterItem;
			_presenterItem.IsPoint = true;
			_presenterItem.ShowBorderOnMouseOver = true;
			_presenterItem.ContextMenuProvider = CreateContextMenu;
			_presenterItem.Title = GetDeviceTooltip();
		}

		private void OnPropertyChanged()
		{
			if (_presenterItem != null)
			{
				//_xdeviceControl.StateClass = _xdevice.DeviceState.StateClass;
				_presenterItem.Title = GetDeviceTooltip();
				_presenterItem.RefreshPainter();
				_presenterItem.DesignerCanvas.Refresh();
			}
		}
		private string GetDeviceTooltip()
		{
			if (_xdevice == null)
				return null;
			var stringBuilder = new StringBuilder();
			stringBuilder.Append(_xdevice.PresentationAddressAndDriver);
			stringBuilder.Append(" - ");
			stringBuilder.AppendLine(_xdevice.Driver.ShortName);

			foreach (var state in _xdevice.DeviceState.States)
				stringBuilder.AppendLine(state.ToDescription());

			return stringBuilder.ToString().TrimEnd();
		}

		protected override Brush GetBrush()
		{
			return DevicePictureCache.GetBrush(_xdevice);
		}

		public RelayCommand ShowInTreeCommand { get; private set; }
		void OnShowInTree()
		{
			ServiceFactory.Events.GetEvent<ShowXDeviceEvent>().Publish(_xdevice.UID);
		}

		public RelayCommand ShowPropertiesCommand { get; private set; }
		void OnShowProperties()
		{
			ServiceFactory.Events.GetEvent<ShowXDeviceDetailsEvent>().Publish(_xdevice.UID);
		}

		private ContextMenu CreateContextMenu()
		{
			if (_contextMenu == null)
			{
				ShowInTreeCommand = new RelayCommand(OnShowInTree);
				ShowPropertiesCommand = new RelayCommand(OnShowProperties);
				
				_contextMenu = new ContextMenu();
				_contextMenu.Items.Add(new MenuItem()
				{
					Header = "Показать в дереве",
					Command = ShowInTreeCommand
				});
				_contextMenu.Items.Add(new MenuItem()
				{
					Header = "Свойства",
					Command = ShowPropertiesCommand
				});
			}
			return _contextMenu;
		}
	}
}