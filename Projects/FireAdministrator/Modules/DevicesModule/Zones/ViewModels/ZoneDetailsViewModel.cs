﻿using System;
using System.Collections.Generic;
using System.Linq;
using FiresecAPI.Models;
using FiresecClient;
using Infrastructure.Common;

namespace DevicesModule.ViewModels
{
    public class ZoneDetailsViewModel : SaveCancelDialogContent
    {
        public Zone _zone;

        public void Initialize()
        {
            Title = "Создание новой зоны";

            var newZone = new Zone()
            {
                Name = "Новая зона",
                No = 1
            };
            if (FiresecManager.DeviceConfiguration.Zones.Count != 0)
                newZone.No = FiresecManager.DeviceConfiguration.Zones.Select(x => x.No).Max() + 1;

            CopyProperties(newZone);
        }

        public void Initialize(Zone zone)
        {
            Title = string.Format("Свойства зоны:   {0}", zone.PresentationName);
            CopyProperties(zone);
        }

        void CopyProperties(Zone zone)
        {
            _zone = zone;
            ZoneType = zone.ZoneType;
            Name = zone.Name;
            No = zone.No;
            Description = zone.Description;
            DetectorCount = zone.DetectorCount;
            EvacuationTime = zone.EvacuationTime;
            AutoSet = zone.AutoSet;
            Delay = zone.Delay;
            Skipped = zone.Skipped;
            GuardZoneType = zone.GuardZoneType;
        }

        public List<ZoneType> AvailableZoneTypes
        {
            get { return Enum.GetValues(typeof(ZoneType)).Cast<ZoneType>().ToList(); }
        }

        ZoneType _zoneType;
        public ZoneType ZoneType
        {
            get { return _zoneType; }
            set
            {
                _zoneType = value;
                OnPropertyChanged("ZoneType");
                OnPropertyChanged("IsFireZone");
                OnPropertyChanged("IsGuardZone");
            }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        ulong? _no;
        public ulong? No
        {
            get { return _no; }
            set
            {
                _no = value;
                OnPropertyChanged("No");
            }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        int _detectorCount;
        public int DetectorCount
        {
            get { return _detectorCount; }
            set
            {
                _detectorCount = value;
                OnPropertyChanged("DetectorCount");
            }
        }

        string _evacuationTime;
        public string EvacuationTime
        {
            get { return _evacuationTime; }
            set
            {
                _evacuationTime = value;
                OnPropertyChanged("EvacuationTime");
            }
        }

        string _delay;
        public string Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                OnPropertyChanged("Delay");
            }
        }

        public bool CanAutoSet
        {
            get
            {
                return (GuardZoneType != GuardZoneType.Ordinary);
            }
        }

        string _autoSet;
        public string AutoSet
        {
            get { return _autoSet; }
            set
            {
                _autoSet = value;
                OnPropertyChanged("AutoSet");
            }
        }

        bool _skipped;
        public bool Skipped
        {
            get { return _skipped; }
            set
            {
                _skipped = value;
                OnPropertyChanged("Skipped");
            }
        }

        public List<GuardZoneType> AvailableGuardZoneTypes
        {
            get { return Enum.GetValues(typeof(GuardZoneType)).Cast<GuardZoneType>().ToList(); }
        }

        GuardZoneType _guardZoneType;
        public GuardZoneType GuardZoneType
        {
            get { return _guardZoneType; }
            set
            {
                _guardZoneType = value;
                OnPropertyChanged("GuardZoneType");
                OnPropertyChanged("CanAutoSet");
            }
        }

        public bool IsFireZone
        {
            get
            {
                return ZoneType == ZoneType.Fire;
            }
        }

        public bool IsGuardZone
        {
            get
            {
                return ZoneType == ZoneType.Guard;
            }
        }

        protected override void Save(ref bool cancel)
        {
            if (_zone.No != No && FiresecManager.DeviceConfiguration.Zones.Any(x => x.No == No))
            {
                DialogBox.DialogBox.Show("Зона с таким номером уже существует");
                return;
            }

            if (_zone.No != No)
            {
                foreach (var device in FiresecManager.DeviceConfiguration.Devices)
                {
                    if (device.ZoneNo == _zone.No)
                        device.ZoneNo = No;
                }
            }

            _zone.ZoneType = ZoneType;
            _zone.Name = Name;
            _zone.No = No;
            _zone.Description = Description;
            _zone.DetectorCount = DetectorCount;
            _zone.EvacuationTime = EvacuationTime;
            _zone.AutoSet = AutoSet;
            _zone.Delay = Delay;
            _zone.Skipped = Skipped;
            _zone.GuardZoneType = GuardZoneType;
        }
    }
}