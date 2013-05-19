﻿using System.Collections.Generic;
using Common;
using DeviceControls;
using FiresecAPI.Models;
using GKModule.Plans.Designer;
using GKModule.Plans.InstrumentAdorners;
using GKModule.Plans.ViewModels;
using GKModule.ViewModels;
using Infrastructure;
using Infrustructure.Plans;
using Infrustructure.Plans.Designer;
using Infrustructure.Plans.Elements;
using Infrustructure.Plans.Events;
using Infrustructure.Plans.Services;
using XFiresecAPI;

namespace GKModule.Plans
{
	class GKPlanExtension : IPlanExtension<Plan>
	{
		private PlanDevicesViewModel _devicesViewModel;
		private CommonDesignerCanvas _designerCanvas;
		private IEnumerable<IInstrument> _instruments;

		public GKPlanExtension(DevicesViewModel devicesViewModel)
		{
			ServiceFactory.Events.GetEvent<PainterFactoryEvent>().Unsubscribe(OnPainterFactoryEvent);
			ServiceFactory.Events.GetEvent<PainterFactoryEvent>().Subscribe(OnPainterFactoryEvent);
			ServiceFactory.Events.GetEvent<ShowPropertiesEvent>().Unsubscribe(OnShowPropertiesEvent);
			ServiceFactory.Events.GetEvent<ShowPropertiesEvent>().Subscribe(OnShowPropertiesEvent);

			ServiceFactory.Events.GetEvent<ElementChangedEvent>().Unsubscribe(x => { UpdateXDeviceInXZones(); });
			ServiceFactory.Events.GetEvent<ElementChangedEvent>().Subscribe(x => { UpdateXDeviceInXZones(); });
			ServiceFactory.Events.GetEvent<ElementAddedEvent>().Unsubscribe(x => { UpdateXDeviceInXZones(); });
			ServiceFactory.Events.GetEvent<ElementAddedEvent>().Subscribe(x => { UpdateXDeviceInXZones(); });

			_devicesViewModel = new PlanDevicesViewModel(devicesViewModel);
			_instruments = null;
		}

		public void Initialize()
		{
			using (new TimeCounter("DevicePictureCache.LoadXCache: {0}"))
				DevicePictureCache.LoadXCache();
		}

		#region IPlanExtension Members

		public int Index
		{
			get { return 1; }
		}
		public string Title
		{
			get { return "ГК"; }
		}

		public object TabPage
		{
			get { return _devicesViewModel; }
		}

		public IEnumerable<IInstrument> Instruments
		{
			get
			{
				if (_instruments == null)
					_instruments = new List<IInstrument>()
					{
						new InstrumentViewModel()
						{
							ImageSource="/Controls;component/Images/ZoneRectangle.png",
							ToolTip="ГК Зона",
							Adorner = new XZoneRectangleAdorner(_designerCanvas),
							Index = 200,
							Autostart = true
						},
						new InstrumentViewModel()
						{
							ImageSource="/Controls;component/Images/ZonePolygon.png",
							ToolTip="ГК Зона",
							Adorner = new XZonePolygonAdorner(_designerCanvas),
							Index = 201,
							Autostart = true
						},
						new InstrumentViewModel()
						{
							ImageSource="/Controls;component/Images/Direction.png",
							ToolTip="ГК Направление",
							Adorner = new XDirectionAdorner(_designerCanvas),
							Index = 201,
							Autostart = true
						},
					};
				return _instruments;
			}
		}

		public bool ElementAdded(Plan plan, ElementBase element)
		{
			IElementZone elementXZone = element as IElementZone;
			if (elementXZone != null)
			{
				if (elementXZone is ElementRectangleXZone)
					plan.ElementRectangleXZones.Add((ElementRectangleXZone)elementXZone);
				else if (elementXZone is ElementPolygonXZone)
					plan.ElementPolygonXZones.Add((ElementPolygonXZone)elementXZone);
				else
					return false;
				Designer.Helper.SetXZone(elementXZone);
				return true;
			}
			else if (element is ElementXDevice)
			{
				var elementXDevice = element as ElementXDevice;
				Helper.SetXDevice(elementXDevice);
				plan.ElementXDevices.Add(elementXDevice);
				return true;
			}
			else if (element is ElementXDirection)
			{
				var elementXDirection = element as ElementXDirection;
				Helper.SetXDirection(elementXDirection);
				plan.ElementXDirections.Add(elementXDirection);
			}
			return false;
		}
		public bool ElementRemoved(Plan plan, ElementBase element)
		{
			IElementZone elementXZone = element as IElementZone;
			if (elementXZone != null)
			{
				if (elementXZone is ElementRectangleXZone)
					plan.ElementRectangleXZones.Remove((ElementRectangleXZone)elementXZone);
				else if (elementXZone is ElementPolygonXZone)
					plan.ElementPolygonXZones.Remove((ElementPolygonXZone)elementXZone);
				else
					return false;
				return true;
			}
			else if (element is ElementXDevice)
			{
				var elementXDevice = element as ElementXDevice;
					plan.ElementXDevices.Remove(elementXDevice);
					return true;
			}
			else if (element is ElementXDirection)
			{
				var elementXDirection = element as ElementXDirection;
				plan.ElementXDirections.Remove(elementXDirection);
				return true;
			}
			return false;
		}

		public void RegisterDesignerItem(DesignerItem designerItem)
		{
			if (designerItem.Element is ElementRectangleXZone || designerItem.Element is ElementPolygonXZone)
			{
				designerItem.Group = "XZone";
				designerItem.UpdateProperties += UpdateDesignerItemXZone;
				UpdateDesignerItemXZone(designerItem);
			}
			else if (designerItem.Element is ElementXDevice)
			{
				designerItem.Group = "GK";
				designerItem.UpdateProperties += UpdateDesignerItemXDevice;
				UpdateDesignerItemXDevice(designerItem);
			}
			else if (designerItem.Element is ElementXDirection)
			{
				designerItem.Group = "XDirection";
				designerItem.UpdateProperties += UpdateDesignerItemXDirection;
				UpdateDesignerItemXDirection(designerItem);
			}
		}

		public IEnumerable<ElementBase> LoadPlan(Plan plan)
		{
			if (plan.ElementPolygonXZones == null)
				plan.ElementPolygonXZones = new List<ElementPolygonXZone>();
			if (plan.ElementRectangleXZones == null)
				plan.ElementRectangleXZones = new List<ElementRectangleXZone>();
			if (plan.ElementXDirections== null)
				plan.ElementXDirections = new List<ElementXDirection>();
			foreach (var element in plan.ElementXDevices)
				yield return element;
			foreach (var element in plan.ElementRectangleXZones)
				yield return element;
			foreach (var element in plan.ElementPolygonXZones)
				yield return element;
			foreach (var element in plan.ElementXDirections)
				yield return element;
		}

		public void ExtensionRegistered(CommonDesignerCanvas designerCanvas)
		{
			_designerCanvas = designerCanvas;
			LayerGroupService.Instance.RegisterGroup("GK", "ГК", 1);
			LayerGroupService.Instance.RegisterGroup("XZone", "ГК Зоны", 2);
			LayerGroupService.Instance.RegisterGroup("XDirection", "ГК Направления", 3);
		}
		public void ExtensionAttached()
		{
			using (new TimeCounter("XDevice.ExtensionAttached.BuildMap: {0}"))
				Helper.BuildMap();
		}

		#endregion

		private void UpdateDesignerItemXDevice(CommonDesignerItem designerItem)
		{
			ElementXDevice elementDevice = designerItem.Element as ElementXDevice;
			XDevice device = Designer.Helper.GetXDevice(elementDevice);
			Designer.Helper.SetXDevice(elementDevice, device);
			designerItem.Title = Helper.GetXDeviceTitle((ElementXDevice)designerItem.Element);
			designerItem.Painter.Invalidate();
			_designerCanvas.Refresh();
		}
		private void UpdateDesignerItemXZone(CommonDesignerItem designerItem)
		{
			IElementZone elementZone = designerItem.Element as IElementZone;
			designerItem.Title = Designer.Helper.GetXZoneTitle(elementZone);
			XZone zone = Designer.Helper.GetXZone(elementZone);
			elementZone.BackgroundColor = Designer.Helper.GetXZoneColor(zone);
			elementZone.SetZLayer(zone == null ? 50 : 60);
			designerItem.Painter.Invalidate();
			_designerCanvas.Refresh();
		}
		private void UpdateDesignerItemXDirection(CommonDesignerItem designerItem)
		{
			var elementXDirection = designerItem.Element as ElementXDirection;
			designerItem.Title = Designer.Helper.GetXDirectionTitle(elementXDirection);
			var xdirection = Designer.Helper.GetXDirection(elementXDirection);
			elementXDirection.BackgroundColor = Designer.Helper.GetXDirectionColor(xdirection);
			elementXDirection.SetZLayer(xdirection == null ? 10 : 11);
			designerItem.Painter.Invalidate();
			_designerCanvas.Refresh();
		}

		private void OnPainterFactoryEvent(PainterFactoryEventArgs args)
		{
			var elementXDevice = args.Element as ElementXDevice;
			if (elementXDevice != null)
				args.Painter = new Painter(elementXDevice);
		}
		private void OnShowPropertiesEvent(ShowPropertiesEventArgs e)
		{
			ElementXDevice element = e.Element as ElementXDevice;
			if (element != null)
				e.PropertyViewModel = new DevicePropertiesViewModel(_devicesViewModel, element);
			else if (e.Element is ElementRectangleXZone || e.Element is ElementPolygonXZone)
				e.PropertyViewModel = new ZonePropertiesViewModel((IElementZone)e.Element);
			else if (e.Element is ElementXDirection)
				e.PropertyViewModel = new XDirectionPropertiesViewModel((ElementXDirection)e.Element);
		}

		public void UpdateXDeviceInXZones()
		{
			///
		}
	}
}