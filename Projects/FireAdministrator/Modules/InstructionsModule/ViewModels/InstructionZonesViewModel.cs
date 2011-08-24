﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common;
using DevicesModule.ViewModels;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure.Common;

namespace InstructionsModule.ViewModels
{
    public class InstructionZonesViewModel : DialogContent
    {

        public InstructionZonesViewModel()
        {
            AddZoneCommand = new RelayCommand(OnAddZone, CanAddAvailableZone);
            RemoveZoneCommand = new RelayCommand(OnRemoveZone, CanRemoveZone);
            AddAllZoneCommand = new RelayCommand(OnAddAllZone, CanAddAllAvailableZone);
            RemoveAllZoneCommand = new RelayCommand(OnRemoveAllZone, CanRemoveAllZone);
            SaveCommand = new RelayCommand(OnSave, CanSaveInstruction);
            CancelCommand = new RelayCommand(OnCancel);

            InstructionZones = new ObservableCollection<ZoneViewModel>();
            AvailableZones = new ObservableCollection<ZoneViewModel>();
            InstructionZonesList = new List<string>();
        }

        public void Inicialize(List<string> instructionDetailsList)
        {
            if (instructionDetailsList.IsNotNullOrEmpty())
            {
                InstructionZonesList = new List<string>(instructionDetailsList);
            }
            InicializeZones();
            if (InstructionZones.IsNotNullOrEmpty())
            {
                SelectedInstructionZone = InstructionZones[0];
            }
        }

        void InicializeZones()
        {
            foreach (var zone in FiresecManager.DeviceConfiguration.Zones)
            {
                var zoneViewModel = new ZoneViewModel(zone);
                if (InstructionZonesList.IsNotNullOrEmpty())
                {
                    var instructionZone = InstructionZonesList.FirstOrDefault(x => x == zoneViewModel.No);
                    if (instructionZone != null)
                    {
                        InstructionZones.Add(zoneViewModel);
                    }
                    else
                    {
                        AvailableZones.Add(zoneViewModel);
                    }
                }
                else
                {
                    AvailableZones.Add(zoneViewModel);
                }
            }

            if (InstructionZones.IsNotNullOrEmpty())
            {
                SelectedInstructionZone = InstructionZones[0];
            }
            if (AvailableZones.IsNotNullOrEmpty())
            {
                SelectedAvailableZone = AvailableZones[0];
            }
        }

        public List<string> InstructionZonesList { get; set; }
        public ObservableCollection<ZoneViewModel> AvailableZones { get; set; }
        public ObservableCollection<ZoneViewModel> InstructionZones { get; set; }
        public ZoneViewModel SelectedAvailableZone { get; set; }
        public ZoneViewModel SelectedInstructionZone { get; set; }

        public bool CanAddAvailableZone(object obj)
        {
            return (SelectedAvailableZone != null);
        }

        public bool CanAddAllAvailableZone(object obj)
        {
            return (AvailableZones.IsNotNullOrEmpty());
        }

        public bool CanRemoveZone(object obj)
        {
            return (SelectedInstructionZone != null);
        }

        public bool CanRemoveAllZone(object obj)
        {
            return (InstructionZones.IsNotNullOrEmpty());
        }

        public bool CanSaveInstruction(object obj)
        {
            return (InstructionZones.IsNotNullOrEmpty());
        }

        public RelayCommand AddZoneCommand { get; private set; }
        void OnAddZone()
        {
            if (CanAddAvailableZone(null))
            {
                InstructionZones.Add(SelectedAvailableZone);
                AvailableZones.Remove(SelectedAvailableZone);
                if (AvailableZones.IsNotNullOrEmpty())
                {
                    SelectedAvailableZone = AvailableZones[0];
                }
                if (InstructionZones.IsNotNullOrEmpty())
                {
                    SelectedInstructionZone = InstructionZones[0];
                }
            }
        }

        public RelayCommand AddAllZoneCommand { get; private set; }
        void OnAddAllZone()
        {
            if (CanAddAllAvailableZone(null))
            {
                foreach (var availableZone in AvailableZones)
                {
                    InstructionZones.Add(availableZone);
                }
                AvailableZones.Clear();
                SelectedAvailableZone = null;
                if (InstructionZones.IsNotNullOrEmpty())
                {
                    SelectedInstructionZone = InstructionZones[0];
                }
            }
        }

        public RelayCommand RemoveAllZoneCommand { get; private set; }
        void OnRemoveAllZone()
        {
            if (CanRemoveAllZone(null))
            {
                foreach (var instructionZone in InstructionZones)
                {
                    AvailableZones.Add(instructionZone);
                }
                InstructionZones.Clear();
                SelectedInstructionZone = null;
                if (AvailableZones.IsNotNullOrEmpty())
                {
                    SelectedAvailableZone = AvailableZones[0];
                }
            }
        }

        public RelayCommand RemoveZoneCommand { get; private set; }
        void OnRemoveZone()
        {
            if (CanRemoveZone(null))
            {
                AvailableZones.Add(SelectedInstructionZone);
                InstructionZones.Remove(SelectedInstructionZone);
                if (AvailableZones.IsNotNullOrEmpty())
                {
                    SelectedAvailableZone = AvailableZones[0];
                }
                if (InstructionZones.IsNotNullOrEmpty())
                {
                    SelectedInstructionZone = InstructionZones[0];
                }
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        void OnSave()
        {
            Save();
            Close(true);
        }

        public RelayCommand CancelCommand { get; private set; }
        void OnCancel()
        {
            Close(false);
        }

        public void Save()
        {
            var instructionZones = new List<string>(
                from zone in InstructionZones
                select zone.No);
            InstructionZonesList = instructionZones;
        }
    }
}