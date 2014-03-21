﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure.Common.Windows.ViewModels;

namespace VideoModule.RVI_VSS.ViewModels
{
	public class PropertyViewModel : BaseViewModel
	{
		public string CellName { get; set; }
		public string PresentationCellName { get; set; }

		public PropertyViewModel(string cellName, string presentationCellName, Guid cameraUid)
		{
			CellName = cellName;
			PresentationCellName = presentationCellName;
			Cameras = new ObservableCollection<Camera>(FiresecManager.SystemConfiguration.Cameras);
			Cameras.Insert(0, new Camera{UID = new Guid()});
			SelectedCamera = Cameras.FirstOrDefault(x => x.UID == cameraUid);
		}

		private ObservableCollection<Camera> _cameras;
		public ObservableCollection<Camera> Cameras
		{
			get { return _cameras; }
			set
			{
				_cameras = value;
				OnPropertyChanged(() => Cameras);
			}
		}

		private Camera _selectedCamera;
		public Camera SelectedCamera
		{
			get { return _selectedCamera; }
			set
			{
				_selectedCamera = value;
				OnPropertyChanged("SelectedCamera");
			}
		}
	}
}