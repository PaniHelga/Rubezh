﻿using System.Collections.ObjectModel;
using FiresecAPI;
using Infrastructure.Common.Windows.ViewModels;
using System.Linq;
using Infrastructure.Common;
using Infrastructure.Common.Windows;
using FiresecClient;
using System;
using System.Collections.Generic;
using FiresecClient.SKDHelpers;

namespace SKDModule.ViewModels
{
	public class AdditionalColumnTypesViewModel : ViewPartViewModel, ISelectable<Guid>
	{
		public AdditionalColumnTypesViewModel()
		{
			AddCommand = new RelayCommand(OnAdd, CanAdd);
			RemoveCommand = new RelayCommand(OnRemove, CanRemove);
			EditCommand = new RelayCommand(OnEdit, CanEdit);
		}

		public void Initialize(AdditionalColumnTypeFilter filter)
		{
			var organisations = OrganisationHelper.Get(new OrganisationFilter() { Uids = FiresecManager.CurrentUser.OrganisationUIDs });
			var additionalColumnTypes = AdditionalColumnTypeHelper.Get(filter);

			AllAdditionalColumnTypes = new List<AdditionalColumnTypeViewModel>();
			Organisations = new List<AdditionalColumnTypeViewModel>();
			foreach (var organisation in organisations)
			{
				var organisationViewModel = new AdditionalColumnTypeViewModel(organisation);
				Organisations.Add(organisationViewModel);
				AllAdditionalColumnTypes.Add(organisationViewModel);
				foreach (var additionalColumnType in additionalColumnTypes)
				{
					if (additionalColumnType.OrganisationUID == organisation.UID)
					{
						var additionalColumnTypeViewModel = new AdditionalColumnTypeViewModel(additionalColumnType);
						organisationViewModel.AddChild(additionalColumnTypeViewModel);
						AllAdditionalColumnTypes.Add(additionalColumnTypeViewModel);
					}
				}
			}

			foreach (var organisation in Organisations)
			{
				organisation.ExpandToThis();
			}
			OnPropertyChanged("RootAdditionalColumnTypes");
		}

		#region AdditionalColumnTypeSelection
		public List<AdditionalColumnTypeViewModel> AllAdditionalColumnTypes;

		public void Select(Guid additionalColumnTypeUID)
		{
			if (additionalColumnTypeUID != Guid.Empty)
			{
				var additionalColumnTypeViewModel = AllAdditionalColumnTypes.FirstOrDefault(x => x.AdditionalColumnType != null && x.AdditionalColumnType.UID == additionalColumnTypeUID);
				if (additionalColumnTypeViewModel != null)
					additionalColumnTypeViewModel.ExpandToThis();
				SelectedAdditionalColumnType = additionalColumnTypeViewModel;
			}
		}
		#endregion

		AdditionalColumnTypeViewModel _selectedAdditionalColumnType;
		public AdditionalColumnTypeViewModel SelectedAdditionalColumnType
		{
			get { return _selectedAdditionalColumnType; }
			set
			{
				_selectedAdditionalColumnType = value;
				if (value != null)
					value.ExpandToThis();
				OnPropertyChanged("SelectedAdditionalColumnType");
			}
		}

		List<AdditionalColumnTypeViewModel> _organisations;
		public List<AdditionalColumnTypeViewModel> Organisations
		{
			get { return _organisations; }
			private set
			{
				_organisations = value;
				OnPropertyChanged("Organisations");
			}
		}

		public Organisation Organisation
		{
			get
			{
				AdditionalColumnTypeViewModel OrganisationViewModel = SelectedAdditionalColumnType;
				if (!OrganisationViewModel.IsOrganisation)
					OrganisationViewModel = SelectedAdditionalColumnType.Parent;

				if (OrganisationViewModel != null)
					return OrganisationViewModel.Organization;

				return null;
			}
		}

		public AdditionalColumnTypeViewModel[] RootAdditionalColumnTypes
		{
			get { return Organisations.ToArray(); }
		}

		public RelayCommand AddCommand { get; private set; }
		void OnAdd()
		{
			var additionalColumnTypeDetailsViewModel = new AdditionalColumnTypeDetailsViewModel(this, Organisation);
			if (DialogService.ShowModalWindow(additionalColumnTypeDetailsViewModel))
			{
				var additionalColumnTypeViewModel = new AdditionalColumnTypeViewModel(additionalColumnTypeDetailsViewModel.ShortAdditionalColumnType);

				AdditionalColumnTypeViewModel OrganisationViewModel = SelectedAdditionalColumnType;
				if (!OrganisationViewModel.IsOrganisation)
					OrganisationViewModel = SelectedAdditionalColumnType.Parent;

				if (OrganisationViewModel == null || OrganisationViewModel.Organization == null)
					return;

				OrganisationViewModel.AddChild(additionalColumnTypeViewModel);
				SelectedAdditionalColumnType = additionalColumnTypeViewModel;
			}
		}
		bool CanAdd()
		{
			return SelectedAdditionalColumnType != null;
		}

		public RelayCommand RemoveCommand { get; private set; }
		void OnRemove()
		{
			AdditionalColumnTypeViewModel OrganisationViewModel = SelectedAdditionalColumnType;
			if (!OrganisationViewModel.IsOrganisation)
				OrganisationViewModel = SelectedAdditionalColumnType.Parent;

			if (OrganisationViewModel == null || OrganisationViewModel.Organization == null)
				return;

			var index = OrganisationViewModel.Children.ToList().IndexOf(SelectedAdditionalColumnType);
			var additionalColumnType = SelectedAdditionalColumnType.AdditionalColumnType;
			bool removeResult = AdditionalColumnTypeHelper.MarkDeleted(additionalColumnType);
			if (!removeResult)
				return;
			OrganisationViewModel.RemoveChild(SelectedAdditionalColumnType);
			index = Math.Min(index, OrganisationViewModel.Children.Count() - 1);
			if (index > -1)
				SelectedAdditionalColumnType = OrganisationViewModel.Children.ToList()[index];
			else
				SelectedAdditionalColumnType = OrganisationViewModel;
		}
		bool CanRemove()
		{
			return SelectedAdditionalColumnType != null && !SelectedAdditionalColumnType.IsOrganisation;
		}

		public RelayCommand EditCommand { get; private set; }
		void OnEdit()
		{
			var additionalColumnTypeDetailsViewModel = new AdditionalColumnTypeDetailsViewModel(this, Organisation, SelectedAdditionalColumnType.AdditionalColumnType.UID);
			if (DialogService.ShowModalWindow(additionalColumnTypeDetailsViewModel))
			{
				SelectedAdditionalColumnType.Update(additionalColumnTypeDetailsViewModel.ShortAdditionalColumnType);
			}
		}
		bool CanEdit()
		{
			return SelectedAdditionalColumnType != null && SelectedAdditionalColumnType.Parent != null && !SelectedAdditionalColumnType.IsOrganisation;
		}
	}
}