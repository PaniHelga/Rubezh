﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using FiresecClient;
using GKModule.Events;
using GKModule.Plans.Designer;
using GKModule.ViewModels;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows.ViewModels;
using Infrustructure.Plans.Elements;
using FiresecAPI.GK;

namespace GKModule.Plans.ViewModels
{
	public class ZonePropertiesViewModel : SaveCancelDialogViewModel
	{
		IElementZone IElementZone;
		ZonesViewModel _zonesViewModel;

		public ZonePropertiesViewModel(IElementZone iElementZone, ZonesViewModel zonesViewModel)
		{
			_zonesViewModel = zonesViewModel;
			IElementZone = iElementZone;
			CreateCommand = new RelayCommand(OnCreate);
			EditCommand = new RelayCommand(OnEdit, CanEdit);
			Title = "Свойства фигуры: Зона";
			var zones = XManager.DeviceConfiguration.SortedZones;
			Zones = new ObservableCollection<ZoneViewModel>();
			foreach (var zone in zones)
			{
				var zoneViewModel = new ZoneViewModel(zone);
				Zones.Add(zoneViewModel);
			}
			if (iElementZone.ZoneUID != Guid.Empty)
				SelectedZone = Zones.FirstOrDefault(x => x.Zone.BaseUID == iElementZone.ZoneUID);
			IsHiddenZone = iElementZone.IsHiddenZone;
		}

		public ObservableCollection<ZoneViewModel> Zones { get; private set; }

		ZoneViewModel _selectedZone;
		public ZoneViewModel SelectedZone
		{
			get { return _selectedZone; }
			set
			{
				_selectedZone = value;
				OnPropertyChanged("SelectedZone");
			}
		}

		private bool _isHiddenZone;
		public bool IsHiddenZone
		{
			get { return _isHiddenZone; }
			set
			{
				_isHiddenZone = value;
				OnPropertyChanged(() => IsHiddenZone);
			}
		}

		public RelayCommand CreateCommand { get; private set; }
		private void OnCreate()
		{
			Guid zoneUID = IElementZone.ZoneUID;
			var createZoneEventArg = new CreateXZoneEventArg();
			ServiceFactory.Events.GetEvent<CreateXZoneEvent>().Publish(createZoneEventArg);
			if (createZoneEventArg.Zone != null)
			{
				GKPlanExtension.Instance.Cache.BuildSafe<XZone>();
				GKPlanExtension.Instance.SetItem<XZone>(IElementZone, createZoneEventArg.Zone.BaseUID);
			}
			UpdateZones(zoneUID);
			if (!createZoneEventArg.Cancel)
				Close(true);
		}

		public RelayCommand EditCommand { get; private set; }
		private void OnEdit()
		{
			ServiceFactory.Events.GetEvent<EditXZoneEvent>().Publish(SelectedZone.Zone.BaseUID);
			SelectedZone.Update(SelectedZone.Zone);
		}
		private bool CanEdit()
		{
			return SelectedZone != null;
		}

		protected override bool Save()
		{
			Guid zoneUID = IElementZone.ZoneUID;
			GKPlanExtension.Instance.SetItem<XZone>(IElementZone, SelectedZone == null ? null : SelectedZone.Zone);
			UpdateZones(zoneUID);
			return base.Save();
		}
		private void UpdateZones(Guid xzoneUID)
		{
			IElementZone.IsHiddenZone = IsHiddenZone;
			if (_zonesViewModel != null)
			{
				if (xzoneUID != IElementZone.ZoneUID)
					Update(xzoneUID);
				Update(IElementZone.ZoneUID);
				_zonesViewModel.LockedSelect(IElementZone.ZoneUID);
			}
		}
		private void Update(Guid zoneUID)
		{
			var zone = _zonesViewModel.Zones.FirstOrDefault(x => x.Zone.BaseUID == zoneUID);
			if (zone != null)
				zone.Update();
		}
	}
}