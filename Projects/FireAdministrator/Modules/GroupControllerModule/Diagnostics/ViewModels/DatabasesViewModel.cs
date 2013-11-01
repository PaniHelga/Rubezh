﻿using System.Collections.Generic;
using System.Linq;
using Common.GK;
using Infrastructure.Common.Windows.ViewModels;
using System.Collections.ObjectModel;

namespace GKModule.ViewModels
{
	public class DatabasesViewModel : DialogViewModel
	{
		public DatabasesViewModel()
		{
			Title = "Бинарный формат конфигурации";

			Databases = new List<CommonDatabase>();
			foreach (var gkDatabase in DatabaseManager.GkDatabases)
			{
				Databases.Add(gkDatabase);
			}
			foreach (var kauDatabase in DatabaseManager.KauDatabases)
			{
				Databases.Add(kauDatabase);
			}
			SelectedDatabase = Databases.FirstOrDefault();
		}

		public List<CommonDatabase> Databases { get; private set; }

		CommonDatabase _selectedDatabase;
        public CommonDatabase SelectedDatabase
        {
            get { return _selectedDatabase; }
            set
            {
                _selectedDatabase = value;
                OnPropertyChanged("SelectedDatabase");
                if (value != null)
                {
                    InitializeSelectedDB();
                }
            }
        }

		void InitializeSelectedDB()
		{
			Descriptors = new ObservableCollection<DescriptorViewModel>();
			foreach (var descriptor in SelectedDatabase.Descriptors)
			{
				var binObjectViewModel = new DescriptorViewModel(descriptor);
				Descriptors.Add(binObjectViewModel);
			}
			SelectedDescriptor = Descriptors.FirstOrDefault();
		}

		ObservableCollection<DescriptorViewModel> _descriptors;
		public ObservableCollection<DescriptorViewModel> Descriptors
		{
			get { return _descriptors; }
			set
			{
				_descriptors = value;
				OnPropertyChanged("Descriptors");
			}
		}

		DescriptorViewModel _selectedDescriptor;
		public DescriptorViewModel SelectedDescriptor
		{
			get { return _selectedDescriptor; }
			set
			{
				_selectedDescriptor = value;
				OnPropertyChanged("SelectedDescriptor");
			}
		}
	}
}