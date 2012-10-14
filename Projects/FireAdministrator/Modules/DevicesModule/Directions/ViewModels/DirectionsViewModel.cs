﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using DevicesModule.Views;
using FiresecClient;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using Infrastructure.Common.Windows.ViewModels;
using Infrastructure.ViewModels;
using System.Windows.Input;
using KeyboardKey = System.Windows.Input.Key;

namespace DevicesModule.ViewModels
{
	public class DirectionsViewModel : MenuViewPartViewModel, IEditingViewModel, ISelectable<Guid>
	{
		public DirectionsViewModel()
		{
			Menu = new DirectionsMenuViewModel(this);
			DeleteCommand = new RelayCommand(OnDelete, CanEditOrDelete);
			EditCommand = new RelayCommand(OnEdit, CanEditOrDelete);
			AddCommand = new RelayCommand(OnAdd);
            RegisterShortcuts();
		}

		public void Initialize()
		{
			Directions = new ObservableCollection<DirectionViewModel>(
				from direction in FiresecManager.Directions
				select new DirectionViewModel(direction));
			SelectedDirection = Directions.FirstOrDefault(); ;
		}

		ObservableCollection<DirectionViewModel> _directions;
		public ObservableCollection<DirectionViewModel> Directions
		{
			get { return _directions; }
			set
			{
				_directions = value;
				OnPropertyChanged("Directions");
			}
		}

		DirectionViewModel _selectedDirection;
		public DirectionViewModel SelectedDirection
		{
			get { return _selectedDirection; }
			set
			{
				_selectedDirection = value;
				OnPropertyChanged("SelectedDirection");
			}
		}

		public RelayCommand DeleteCommand { get; private set; }
		void OnDelete()
		{
			FiresecManager.Directions.Remove(SelectedDirection.Direction);
			Directions.Remove(SelectedDirection);

			ServiceFactory.SaveService.DevicesChanged = true;
		}

		public RelayCommand EditCommand { get; private set; }
		void OnEdit()
		{
			var directionDetailsViewModel = new DirectionDetailsViewModel(SelectedDirection.Direction);
			if (DialogService.ShowModalWindow(directionDetailsViewModel))
			{
				SelectedDirection.Update();

				ServiceFactory.SaveService.DevicesChanged = true;
			}
		}

		bool CanEditOrDelete()
		{
			return SelectedDirection != null;
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var directionDetailsViewModel = new DirectionDetailsViewModel();
			if (DialogService.ShowModalWindow(directionDetailsViewModel))
			{
				FiresecManager.Directions.Add(directionDetailsViewModel.Direction);
                var directionViewModel = new DirectionViewModel(directionDetailsViewModel.Direction);
                Directions.Add(directionViewModel);
                SelectedDirection = directionViewModel;
				ServiceFactory.SaveService.DevicesChanged = true;
			}
		}

		#region ISelectable<Guid> Members
		public void Select(Guid directionUID)
		{
            if (directionUID != Guid.Empty)
                SelectedDirection = Directions.FirstOrDefault(x => x.Direction.UID == directionUID);
		}
		#endregion

        private void RegisterShortcuts()
        {
            RegisterShortcut(new KeyGesture(KeyboardKey.N, ModifierKeys.Control), AddCommand);
            RegisterShortcut(new KeyGesture(KeyboardKey.Delete, ModifierKeys.Control), DeleteCommand);
            RegisterShortcut(new KeyGesture(KeyboardKey.E, ModifierKeys.Control), EditCommand);
        }
	}
}