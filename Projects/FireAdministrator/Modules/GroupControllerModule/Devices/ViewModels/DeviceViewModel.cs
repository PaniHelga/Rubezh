﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using DeviceControls;
using FiresecAPI.GK;
using FiresecAPI.Models;
using FiresecClient;
using GKModule.Events;
using GKModule.Plans;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Services;
using Infrastructure.Common.TreeList;
using Infrastructure.Common.Windows;
using Infrastructure.Events;
using Infrustructure.Plans.Events;
using Infrustructure.Plans.Painters;
using GKModule.Plans.Designer;

namespace GKModule.ViewModels
{
	public partial class DeviceViewModel : TreeNodeViewModel<DeviceViewModel>
	{
		public GKDevice Device { get; private set; }
		public PropertiesViewModel PropertiesViewModel { get; private set; }

		public DeviceViewModel(GKDevice device)
		{
			AddCommand = new RelayCommand(OnAdd, CanAdd);
			AddToParentCommand = new RelayCommand(OnAddToParent, CanAddToParent);
			RemoveCommand = new RelayCommand(OnRemove, CanRemove);
			SelectCommand = new RelayCommand(OnSelect, CanSelect);
			ShowPropertiesCommand = new RelayCommand(OnShowProperties, CanShowProperties);
			ShowLogicCommand = new RelayCommand(OnShowLogic, CanShowLogic);
			ShowNSLogicCommand = new RelayCommand(OnShowNSLogic, CanShowNSLogic);
			ShowZonesCommand = new RelayCommand(OnShowZones, CanShowZones);
			ShowZoneOrLogicCommand = new RelayCommand(OnShowZoneOrLogic, CanShowZoneOrLogic);
			ShowZoneCommand = new RelayCommand(OnShowZone, CanShowZone);
			ShowOnPlanCommand = new RelayCommand(OnShowOnPlan);
			ShowParentCommand = new RelayCommand(OnShowParent, CanShowParent);
			ShowMPTCommand = new RelayCommand(OnShowMPT, CanShowMPT);

			CreateDragObjectCommand = new RelayCommand<DataObject>(OnCreateDragObjectCommand, CanCreateDragObjectCommand);
			CreateDragVisual = OnCreateDragVisual;
			AllowMultipleVizualizationCommand = new RelayCommand<bool>(OnAllowMultipleVizualizationCommand, CanAllowMultipleVizualizationCommand);

			Device = device;
			PropertiesViewModel = new PropertiesViewModel(Device);

			AvailvableDrivers = new ObservableCollection<GKDriver>();
			UpdateDriver();
			InitializeParamsCommands();
			Device.Changed += OnChanged;
			Device.AUParametersChanged += UpdateDeviceParameterMissmatch;
		}

		void OnChanged()
		{
			OnPropertyChanged(() => PresentationAddress);
			OnPropertyChanged(() => PresentationZone);
			OnPropertyChanged(() => EditingPresentationZone);
			OnPropertyChanged(() => IsParamtersEnabled);
		}

		public void UpdateProperties()
		{
			PropertiesViewModel = new PropertiesViewModel(Device);
			OnPropertyChanged(() => PropertiesViewModel);
			OnPropertyChanged(() => IsParamtersEnabled);
		}

		public void Update()
		{
			OnPropertyChanged(() => HasChildren);
			OnPropertyChanged(() => IsOnPlan);
			OnPropertyChanged(() => VisualizationState);
		}

		public string Address
		{
			get { return Device.Address; }
			set
			{
				Device.SetAddress(value);
				if (Driver.IsGroupDevice)
				{
					foreach (var deviceViewModel in Children)
					{
						deviceViewModel.OnPropertyChanged("Ip");
						deviceViewModel.OnPropertyChanged(() => PresentationAddress);
					}
				}
				OnPropertyChanged("Ip");
				OnPropertyChanged(() => PresentationAddress);
				ServiceFactory.SaveService.GKChanged = true;
			}
		}

		public string PresentationAddress
		{
			get { return Device.PresentationAddress; }
		}

		public string Description
		{
			get { return Device.Description; }
			set
			{
				Device.Description = value;
				OnPropertyChanged(() => Description);
				ServiceFactory.SaveService.GKChanged = true;
			}
		}

		public bool IsUsed
		{
			get { return !Device.IsNotUsed; }
			set
			{
				Device.IsNotUsed = !value;
				GKManager.ChangeLogic(Device, new GKLogic());
				OnPropertyChanged(() => IsUsed);
				OnPropertyChanged(() => ShowOnPlan);
				OnPropertyChanged(() => PresentationZone);
				OnPropertyChanged(() => EditingPresentationZone);
				ServiceFactory.SaveService.GKChanged = true;
			}
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			NewDeviceViewModelBase newDeviceViewModel;
			if (Device.IsConnectedToKAURSR2OrIsKAURSR2)
				newDeviceViewModel = new RSR2NewDeviceViewModel(this);
			else
				newDeviceViewModel = new NewDeviceViewModel(this);

			if (newDeviceViewModel.Drivers.Count == 1)
			{
				newDeviceViewModel.SaveCommand.Execute();
				foreach (var addedDevice in newDeviceViewModel.AddedDevices)
				{
					DevicesViewModel.Current.AllDevices.Add(addedDevice);
				}
				DevicesViewModel.Current.SelectedDevice = newDeviceViewModel.AddedDevices.LastOrDefault();
				GKPlanExtension.Instance.Cache.BuildSafe<GKDevice>();
				ServiceFactory.SaveService.GKChanged = true;
				return;
			}
			if (DialogService.ShowModalWindow(newDeviceViewModel))
			{
				foreach (var addedDevice in newDeviceViewModel.AddedDevices)
				{
					DevicesViewModel.Current.AllDevices.Add(addedDevice);
					foreach (var childDeviceViewModel in addedDevice.Children)
					{
						DevicesViewModel.Current.AllDevices.Add(childDeviceViewModel);
					}
				}
				DevicesViewModel.Current.SelectedDevice = newDeviceViewModel.AddedDevices.LastOrDefault();
				GKPlanExtension.Instance.Cache.BuildSafe<GKDevice>();
				ServiceFactory.SaveService.GKChanged = true;
			}
		}
		public bool CanAdd()
		{
			if (Device.AllParents.Any(x => x.DriverType == GKDriverType.RSR2_KAU))
			{
				if (Device.DriverType == GKDriverType.KAUIndicator)
					return false;
				if (Device.Parent != null && Device.Parent.Driver.IsGroupDevice)
					return false;
				return true;
			}
			if (Driver.Children.Count > 0)
				return true;
			if ((Driver.DriverType == GKDriverType.MPT || Driver.DriverType == GKDriverType.MRO_2) && Parent != null && Parent.Device.DriverType != GKDriverType.MPT && Parent.Device.DriverType != GKDriverType.MRO_2)
				return true;
			return false;
		}

		public RelayCommand AddToParentCommand { get; private set; }
		void OnAddToParent()
		{
			Parent.AddCommand.Execute();
		}
		public bool CanAddToParent()
		{
			return ((Parent != null) && (Parent.AddCommand.CanExecute(null)));
		}

		public RelayCommand RemoveCommand { get; private set; }
		void OnRemove()
		{
			Remove(true);
			if (Device.KAURSR2Parent != null)
				GKManager.RebuildRSR2Addresses(Device.KAURSR2Parent);
		}
		bool CanRemove()
		{
			return !(Driver.IsAutoCreate || Parent == null || Parent.Driver.IsGroupDevice);
		}
		public void Remove(bool updateParameters)
		{
			var allDevices = Device.AllChildrenAndSelf;
			foreach (var device in allDevices)
			{
				GKManager.RemoveDevice(device);
			}
			allDevices.ForEach(device => device.OnChanged());
			using (var cache = new ElementDeviceUpdater())
				cache.ResetDevices(allDevices);
			if (updateParameters)
			{
				if (Parent != null)
				{
					Parent.Device.OnAUParametersChanged();
				}
			}

			var parent = Parent;
			if (parent != null)
			{
				var index = DevicesViewModel.Current.SelectedDevice.VisualIndex;
				parent.Nodes.Remove(this);
				parent.Update();
				index = Math.Min(index, parent.ChildrenCount - 1);
				foreach (var childDeviceViewModel in GetAllChildren(true))
				{
					DevicesViewModel.Current.AllDevices.Remove(childDeviceViewModel);
				}
				DevicesViewModel.Current.SelectedDevice = index >= 0 ? parent.GetChildByVisualIndex(index) : parent;
			}
			GKPlanExtension.Instance.Cache.BuildSafe<GKDevice>();
			ServiceFactory.SaveService.GKChanged = true;
		}

		public RelayCommand SelectCommand { get; private set; }
		void OnSelect()
		{
			var devicesOnShleifViewModel = new DevicesOnShleifViewModel(Device);
			DialogService.ShowModalWindow(devicesOnShleifViewModel);
		}
		bool CanSelect()
		{
			return Driver.DriverType == GKDriverType.KAU_Shleif || Driver.DriverType == GKDriverType.RSR2_KAU_Shleif;
		}

		public RelayCommand ShowPropertiesCommand { get; private set; }
		void OnShowProperties()
		{
		}
		bool CanShowProperties()
		{
			return false;
		}

		public string PresentationZone
		{
			get
			{
				if (Device.IsNotUsed)
					return null;
				return GKManager.GetPresentationZoneOrLogic(Device);
			}
		}

		public string EditingPresentationZone
		{
			get
			{
				if (Device.IsNotUsed)
					return null;
				var presentationZone = GKManager.GetPresentationZoneOrLogic(Device);
				IsLogicGrayed = string.IsNullOrEmpty(presentationZone);
				if (string.IsNullOrEmpty(presentationZone))
				{
					if (Driver.HasZone)
						presentationZone = "Нажмите для выбора зон";
					if (Driver.HasLogic)
						presentationZone = "Нажмите для настройки логики";
				}
				return presentationZone;
			}
		}

		bool _isLogicGrayed;
		public bool IsLogicGrayed
		{
			get { return _isLogicGrayed; }
			set
			{
				_isLogicGrayed = value;
				OnPropertyChanged(() => IsLogicGrayed);
			}
		}

		public bool IsOnPlan
		{
			get { return Device.PlanElementUIDs.Count > 0; }
		}
		public bool ShowOnPlan
		{
			get { return !Device.IsNotUsed && (Device.Driver.IsDeviceOnShleif || Device.Children.Count > 0); }
		}
		public VisualizationState VisualizationState
		{
			get { return Driver != null && Driver.IsPlaceable ? (IsOnPlan ? (Device.AllowMultipleVizualization ? VisualizationState.Multiple : VisualizationState.Single) : VisualizationState.NotPresent) : VisualizationState.Prohibit; }
		}

		public RelayCommand<DataObject> CreateDragObjectCommand { get; private set; }
		private void OnCreateDragObjectCommand(DataObject dataObject)
		{
			IsSelected = true;
			var plansElement = new ElementGKDevice
				{
					DeviceUID = Device.UID
				};
			dataObject.SetData("DESIGNER_ITEM", plansElement);
		}
		private bool CanCreateDragObjectCommand(DataObject dataObject)
		{
			return VisualizationState == VisualizationState.NotPresent || VisualizationState == VisualizationState.Multiple;
		}

		public Converter<IDataObject, UIElement> CreateDragVisual { get; private set; }
		private UIElement OnCreateDragVisual(IDataObject dataObject)
		{
			ServiceFactory.Layout.SetRightPanelVisible(true);
			var brush = PictureCacheSource.GKDevicePicture.GetBrush(Device);
			return new Rectangle
			{
				Fill = brush,
				Height = PainterCache.DefaultPointSize,
				Width = PainterCache.DefaultPointSize,
			};
		}

		public RelayCommand ShowOnPlanCommand { get; private set; }
		void OnShowOnPlan()
		{
			if (Device.PlanElementUIDs.Count > 0)
				ServiceFactoryBase.Events.GetEvent<FindElementEvent>().Publish(Device.PlanElementUIDs);
		}

		public RelayCommand<bool> AllowMultipleVizualizationCommand { get; private set; }
		private void OnAllowMultipleVizualizationCommand(bool isAllow)
		{
			Device.AllowMultipleVizualization = isAllow;
			Update();
		}
		private bool CanAllowMultipleVizualizationCommand(bool isAllow)
		{
			return Device.AllowMultipleVizualization != isAllow;
		}

		#region Zone and Logic
		public RelayCommand ShowLogicCommand { get; private set; }
		void OnShowLogic()
		{
			var hasOnNowClause = Device.Driver.AvailableCommandBits.Contains(GKStateBit.TurnOnNow_InManual);
			var hasOffNowClause = Device.Driver.AvailableCommandBits.Contains(GKStateBit.TurnOffNow_InManual);
			var hasStopClause = Device.DriverType == GKDriverType.RSR2_Valve_DU || Device.DriverType == GKDriverType.RSR2_Valve_KV || Device.DriverType == GKDriverType.RSR2_Valve_KVMV;
			var logicViewModel = new LogicViewModel(Device, Device.Logic, true, hasOnNowClause, hasOffNowClause, hasStopClause);
			if (DialogService.ShowModalWindow(logicViewModel))
			{
				GKManager.ChangeLogic(Device, logicViewModel.GetModel());
				OnPropertyChanged(() => PresentationZone);
				ServiceFactory.SaveService.GKChanged = true;
			}
		}
		bool CanShowLogic()
		{
			return Driver.HasLogic && !Device.IsNotUsed && !Device.IsChildMPTOrMRO();
		}

		public RelayCommand ShowZonesCommand { get; private set; }
		void OnShowZones()
		{
			var zonesSelectationViewModel = new ZonesSelectationViewModel(Device.Zones, true);
			if (DialogService.ShowModalWindow(zonesSelectationViewModel))
			{
				GKManager.ChangeDeviceZones(Device, zonesSelectationViewModel.Zones);
				OnPropertyChanged("PresentationZone");
				ServiceFactory.SaveService.GKChanged = true;
			}
		}
		bool CanShowZones()
		{
			return Driver.HasZone && !Device.IsNotUsed;
		}

		public RelayCommand ShowZoneOrLogicCommand { get; private set; }
		void OnShowZoneOrLogic()
		{
			IsSelected = true;

			if (CanShowZones())
				OnShowZones();

			if (CanShowLogic())
				OnShowLogic();
		}
		bool CanShowZoneOrLogic()
		{
			return !Device.IsInMPT && (CanShowZones() || CanShowLogic());
		}

		public bool IsZoneOrLogic
		{
			get { return CanShowZoneOrLogic(); }
		}

		public RelayCommand ShowZoneCommand { get; private set; }
		void OnShowZone()
		{
			var zone = Device.Zones.FirstOrDefault();
			if (zone != null)
			{
				ServiceFactoryBase.Events.GetEvent<ShowGKZoneEvent>().Publish(zone.UID);
			}
		}
		bool CanShowZone()
		{
			return Device.Zones.Count == 1;
		}

		public RelayCommand ShowNSLogicCommand { get; private set; }
		void OnShowNSLogic()
		{
			var logicViewModel = new LogicViewModel(Device, Device.NSLogic);
			if (DialogService.ShowModalWindow(logicViewModel))
			{
				Device.NSLogic = logicViewModel.GetModel();
				OnPropertyChanged("NSPresentationZone");
				ServiceFactory.SaveService.GKChanged = true;
			}
		}
		bool CanShowNSLogic()
		{
			return Driver.IsPump;
		}

		public bool IsNSLogic
		{
			get { return CanShowNSLogic(); }
		}

		public string NSPresentationZone
		{
			get
			{
				var presentationZone = GKManager.GetPresentationLogic(Device.NSLogic);
				IsLogicGrayed = string.IsNullOrEmpty(presentationZone);
				if (string.IsNullOrEmpty(presentationZone))
				{
					if (CanShowNSLogic())
						presentationZone = "Нажмите для настройки логики насоса";
				}
				return presentationZone;
			}
		}

		#endregion

		#region Driver
		public GKDriver Driver
		{
			get { return Device.Driver; }
			set
			{
				if (Device.DriverType != value.DriverType)
				{
					GKManager.ChangeDriver(Device, value);
					Nodes.Clear();
					foreach (var childDevice in Device.Children)
					{
						DevicesViewModel.Current.AddDevice(childDevice, this);
					}
					OnPropertyChanged(() => Device);
					OnPropertyChanged(() => Driver);
					OnPropertyChanged(() => Device);
					OnPropertyChanged(() => Children);
					OnPropertyChanged(() => EditingPresentationZone);
					PropertiesViewModel = new PropertiesViewModel(Device);
					OnPropertyChanged(() => PropertiesViewModel);
					if (Device.KAURSR2Parent != null)
						GKManager.RebuildRSR2Addresses(Device.KAURSR2Parent);
					GKManager.DeviceConfiguration.Update();
					Update();
					ServiceFactory.SaveService.GKChanged = true;
				}
			}
		}

		public ObservableCollection<GKDriver> AvailvableDrivers { get; private set; }

		void UpdateDriver()
		{
			AvailvableDrivers.Clear();
			if (CanChangeDriver)
			{
				switch (Device.Parent.DriverType)
				{
					case GKDriverType.AM_4:
						AvailvableDrivers.Add(GKManager.Drivers.FirstOrDefault(x => x.DriverType == GKDriverType.AM_1));
						AvailvableDrivers.Add(GKManager.Drivers.FirstOrDefault(x => x.DriverType == GKDriverType.AM1_T));
						break;

					case GKDriverType.AMP_4:
						AvailvableDrivers.Add(GKManager.Drivers.FirstOrDefault(x => x.DriverType == GKDriverType.AMP_1));
						break;

					default:
						foreach (var driverType in Device.Parent.Driver.Children)
						{
							var driver = GKManager.Drivers.FirstOrDefault(x => x.DriverType == driverType);
							if (CanDriverBeChanged(driver))
							{
								AvailvableDrivers.Add(driver);
							}
						}
						break;
				}
			}
		}

		public bool CanDriverBeChanged(GKDriver driver)
		{
			if (driver == null || Device.Parent == null)
				return false;

			if (Device.IsChildMPTOrMRO())
				return false;

			if (driver.DriverType == GKDriverType.RSR2_MVP_Part)
				return false;

			if (Device.Parent.Driver.DriverType == GKDriverType.AM_4)
				return true;

			if (driver.IsAutoCreate)
				return false;
			if (Device.Parent.Driver.IsGroupDevice)
				return false;
			return driver.IsDeviceOnShleif;
		}

		public bool CanChangeDriver
		{
			get { return CanDriverBeChanged(Device.Driver); }
		}
		#endregion

		public RelayCommand ShowParentCommand { get; private set; }
		void OnShowParent()
		{
			ServiceFactoryBase.Events.GetEvent<ShowGKDeviceEvent>().Publish(Device.Parent.UID);
		}
		bool CanShowParent()
		{
			return Device.Parent != null;
		}

		public bool IsBold { get; set; }

		public string MPTName
		{
			get
			{
				var mpt = GKManager.MPTs.FirstOrDefault(x => x.Devices.Any(y => y.UID == Device.UID));
				if (mpt != null)
					return mpt.Name;
				return null;
			}
		}

		public RelayCommand ShowMPTCommand { get; private set; }
		void OnShowMPT()
		{
			var mpt = GKManager.MPTs.FirstOrDefault(x => x.Devices.Any(y => y.UID == Device.UID));
			if (mpt != null)
				ServiceFactoryBase.Events.GetEvent<ShowGKMPTEvent>().Publish(mpt.UID);
		}
		bool CanShowMPT()
		{
			return true;
		}

		public RelayCommand CopyCommand { get { return DevicesViewModel.Current.CopyCommand; } }
		public RelayCommand CutCommand { get { return DevicesViewModel.Current.CutCommand; } }
		public RelayCommand PasteCommand { get { return DevicesViewModel.Current.PasteCommand; } }

		#region OPC
		public bool CanOPCUsed
		{
			get { return Device.Driver.IsPlaceable; }
		}

		public bool IsOPCUsed
		{
			get { return Device.IsOPCUsed; }
			set
			{
				Device.IsOPCUsed = value;
				OnPropertyChanged(() => IsOPCUsed);
				ServiceFactory.SaveService.GKChanged = true;
			}
		}
		#endregion
	}
}