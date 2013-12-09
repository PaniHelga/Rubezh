﻿//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Threading;
//using System.Windows.Media;
//using GKProcessor;
//using DeviceControls;
//using FiresecClient;
//using Infrastructure;
//using Infrastructure.Common;
//using Infrastructure.Common.Windows.ViewModels;
//using Infrastructure.Events;
//using XFiresecAPI;
//using FiresecAPI.Models;
//using Infrustructure.Plans.Events;
//using Infrustructure.Plans.Elements;
//using System.Windows.Input;

//namespace GKModule.ViewModels
//{
//    public class DeviceDetailsViewModel : DialogViewModel, IWindowIdentity
//    {
//        Guid _guid;
//        public XDevice Device { get; private set; }
//        public DeviceStateViewModel DeviceStateViewModel { get; private set; }
//        public XState State { get; private set; }
//        public DeviceCommandsViewModel DeviceCommandsViewModel { get; private set; }
//        public DevicePropertiesViewModel DevicePropertiesViewModel { get; private set; }
//        BackgroundWorker BackgroundWorker;
//        bool CancelBackgroundWorker = false;

//        public DeviceDetailsViewModel(Guid deviceUID)
//        {
//            _guid = deviceUID;
//            ShowCommand = new RelayCommand(OnShow);
//            ShowParentCommand = new RelayCommand(OnShowParent, CanShowParent);
//            ShowOnPlanCommand = new RelayCommand<Plan>(OnShowOnPlan);
//            ShowZoneCommand = new RelayCommand(OnShowZone);
//            ShowJournalCommand = new RelayCommand(OnShowJournal);

//            Device = XManager.Devices.FirstOrDefault(x => x.UID == deviceUID);
//            State = Device.State;
//            DeviceStateViewModel = new DeviceStateViewModel(State);
//            State.StateChanged += new Action(OnStateChanged);
//            DeviceCommandsViewModel = new DeviceCommandsViewModel(State);
//            DevicePropertiesViewModel = new DevicePropertiesViewModel(Device);
//            InitializePlans();

//            Title = Device.Driver.ShortName + " " + Device.DottedAddress;
//            TopMost = true;

//            StartParametersMonitoring();
//        }

//        void StartParametersMonitoring()
//        {
//            AUParameterValues = new ObservableCollection<AUParameterValue>();
//            foreach (var auParameter in Device.Driver.AUParameters)
//            {
//                var auParameterValue = new AUParameterValue()
//                {
//                    Name = auParameter.Name,
//                    IsDelay = auParameter.IsDelay
//                };
//                AUParameterValues.Add(auParameterValue);
//            }

//            ThreadSafeAUParameterValues = new ObservableCollection<AUParameterValue>();
//            foreach (var auParameter in Device.Driver.AUParameters)
//            {
//                var auParameterValue = new AUParameterValue()
//                {
//                    Name = auParameter.Name,
//                    IsDelay = auParameter.IsDelay
//                };
//                ThreadSafeAUParameterValues.Add(auParameterValue);
//            }

//            BackgroundWorker = new BackgroundWorker();
//            BackgroundWorker.DoWork += new DoWorkEventHandler(UpdateAuParameters);
//            BackgroundWorker.RunWorkerAsync();
//        }

//        void OnStateChanged()
//        {
//            OnPropertyChanged("DevicePicture");
//            OnPropertyChanged("State");
//            OnPropertyChanged("DeviceStateViewModel");
//            OnPropertyChanged("HasOnDelay");
//            OnPropertyChanged("HasHoldDelay");
//            OnPropertyChanged("HasOffDelay");
//            CommandManager.InvalidateRequerySuggested();
//        }

//        public Brush DevicePicture
//        {
//            get { return DevicePictureCache.GetDynamicXBrush(Device); }
//        }

//        public string PresentationZone
//        {
//            get { return XManager.GetPresentationZone(Device); }
//        }

//        public bool HasAUParameters
//        {
//            get { return Device.Driver.AUParameters.Where(x => !x.IsDelay).Count() > 0; }
//        }

//        void UpdateAuParameters(object sender, DoWorkEventArgs e)
//        {
//            while (true)
//            {
//                if (CancelBackgroundWorker)
//                    break;
//                OnUpdateAuParameters();
//                Thread.Sleep(TimeSpan.FromSeconds(1));
//            }
//        }

//        void OnUpdateAuParameters()
//        {
//            if (Device.KauDatabaseParent != null && Device.KauDatabaseParent.DriverType == XDriverType.KAU)
//            {
//                foreach (var auParameter in Device.Driver.AUParameters)
//                {
//                    var bytes = new List<byte>();
//                    bytes.Add((byte)Device.Driver.DriverTypeNo);
//                    bytes.Add(Device.IntAddress);
//                    bytes.Add((byte)(Device.ShleifNo - 1));
//                    bytes.Add(auParameter.No);
//                    var result = SendManager.Send(Device.KauDatabaseParent, 4, 131, 2, bytes);
//                    if (!result.HasError)
//                    {
//                        for (int i = 0; i < 100; i++)
//                        {
//                            result = SendManager.Send(Device.GkDatabaseParent, 2, 12, 68, BytesHelper.ShortToBytes(Device.GKDescriptorNo));
//                            if (!result.HasError && result.Bytes.Count > 0)
//                            {
//                                var resievedParameterNo = result.Bytes[63];
//                                if (resievedParameterNo == auParameter.No)
//                                {
//                                    var parameterValue = BytesHelper.SubstructShort(result.Bytes, 64);
//                                    if (auParameter.IsHighByte)
//                                    {
//                                        parameterValue = (ushort)(parameterValue / 256);
//                                    }
//                                    else if (auParameter.IsLowByte)
//                                    {
//                                        parameterValue = (ushort)(parameterValue << 8);
//                                        parameterValue = (ushort)(parameterValue >> 8);
//                                    }
//                                    var stringValue = parameterValue.ToString();
//                                    if (auParameter.Name == "Дата последнего обслуживания")
//                                    {
//                                        stringValue = (parameterValue / 256).ToString() + "." + (parameterValue % 256).ToString();
//                                    }
//                                    if ((Device.DriverType == XDriverType.Valve || Device.DriverType == XDriverType.Pump)
//                                        && auParameter.Name == "Режим работы")
//                                    {
//                                        stringValue = "Неизвестно";
//                                        switch (parameterValue & 3)
//                                        {
//                                            case 0:
//                                                stringValue = "Автоматический";
//                                                break;

//                                            case 1:
//                                                stringValue = "Ручной";
//                                                break;

//                                            case 2:
//                                                stringValue = "Отключено";
//                                                break;
//                                        }
//                                    }

//                                    var auParameterValue = ThreadSafeAUParameterValues.FirstOrDefault(x => x.Name == auParameter.Name);
//                                    auParameterValue.Value = parameterValue;
//                                    auParameterValue.StringValue = stringValue;

//                                    Dispatcher.BeginInvoke(new Action(() =>
//                                    {
//                                        foreach (var threadSafeAUParameterValue in ThreadSafeAUParameterValues)
//                                        {
//                                            var localAUParameterValue = AUParameterValues.FirstOrDefault(x => x.Name == threadSafeAUParameterValue.Name);
//                                            localAUParameterValue.Value = threadSafeAUParameterValue.Value;
//                                            localAUParameterValue.StringValue = threadSafeAUParameterValue.StringValue;
//                                        }
//                                    }));

//                                    break;
//                                }
//                                Thread.Sleep(100);
//                            }
//                        }
//                    }
//                }
//            }
//            else if (Device.KauDatabaseParent != null && Device.KauDatabaseParent.DriverType == XDriverType.RSR2_KAU)
//            {
//                var result = SendManager.Send(Device.GkDatabaseParent, 2, 12, 68, BytesHelper.ShortToBytes(Device.GKDescriptorNo));
//                if (!result.HasError && result.Bytes.Count > 0)
//                {
//                    for (int i = 0; i < Device.Driver.AUParameters.Count; i++)
//                    {
//                        var auParameter = Device.Driver.AUParameters[i];
//                        var parameterValue = BytesHelper.SubstructShort(result.Bytes, 48 + i * 2);
//                        var stringValue = parameterValue.ToString();
//                        if (auParameter.Name == "Дата последнего обслуживания")
//                        {
//                            stringValue = (parameterValue / 256).ToString() + "." + (parameterValue % 256).ToString();
//                        }

//                        Dispatcher.BeginInvoke(new Action(() =>
//                        {
//                            var auParameterValue = AUParameterValues.FirstOrDefault(x => x.Name == auParameter.Name);
//                            auParameterValue.Value = parameterValue;
//                            auParameterValue.StringValue = stringValue;
//                        }));
//                    }
//                }
//            }
//        }

//        public ObservableCollection<AUParameterValue> ThreadSafeAUParameterValues;

//        ObservableCollection<AUParameterValue> _auParameterValues;
//        public ObservableCollection<AUParameterValue> AUParameterValues
//        {
//            get { return _auParameterValues; }
//            set
//            {
//                _auParameterValues = value;
//                OnPropertyChanged("AUParameterValues");
//            }
//        }

//        public bool HasOnDelay
//        {
//            get { return State.StateClasses.Contains(XStateClass.TurningOn) && State.OnDelay > 0; }
//        }
//        public bool HasHoldDelay
//        {
//            get { return State.StateClasses.Contains(XStateClass.On) && State.HoldDelay > 0; }
//        }
//        public bool HasOffDelay
//        {
//            get { return State.StateClasses.Contains(XStateClass.TurningOff) && State.OffDelay > 0; }
//        }

//        public RelayCommand ShowCommand { get; private set; }
//        void OnShow()
//        {
//            ServiceFactory.Events.GetEvent<ShowXDeviceEvent>().Publish(Device.UID);
//        }

//        public RelayCommand ShowParentCommand { get; private set; }
//        void OnShowParent()
//        {
//            ServiceFactory.Events.GetEvent<ShowXDeviceEvent>().Publish(Device.Parent.UID);
//        }
//        bool CanShowParent()
//        {
//            return Device.Parent != null;
//        }

//        public ObservableCollection<PlanLinkViewModel> Plans { get; private set; }
//        public bool HasPlans
//        {
//            get { return Plans.Count > 0; }
//        }

//        void InitializePlans()
//        {
//            Plans = new ObservableCollection<PlanLinkViewModel>();
//            foreach (var plan in FiresecManager.PlansConfiguration.AllPlans)
//            {
//                ElementBase elementBase = plan.ElementXDevices.FirstOrDefault(x => x.XDeviceUID == Device.UID);
//                if (elementBase != null)
//                {
//                    var alarmPlanViewModel = new PlanLinkViewModel(plan, elementBase);
//                    alarmPlanViewModel.Device = Device;
//                    Plans.Add(alarmPlanViewModel);
//                }
//            }
//        }

//        public ObservableCollection<PlanViewModel> PlanNames
//        {
//            get
//            {
//                var planes = FiresecManager.PlansConfiguration.AllPlans.Where(item => item.ElementXDevices.Any(element => element.XDeviceUID == Device.UID));
//                var planViewModels = new ObservableCollection<PlanViewModel>();
//                foreach (var plan in planes)
//                {
//                    planViewModels.Add(new PlanViewModel(plan, Device));
//                }
//                return planViewModels;
//            }
//        }
//        public RelayCommand<Plan> ShowOnPlanCommand { get; private set; }
//        void OnShowOnPlan(Plan plan)
//        {
//            ShowOnPlanHelper.ShowDevice(Device, plan);
//        }

//        public RelayCommand ShowZoneCommand { get; private set; }
//        void OnShowZone()
//        {
//            var zone = Device.Zones.FirstOrDefault();
//            if (zone != null)
//            {
//                ServiceFactory.Events.GetEvent<ShowXZoneEvent>().Publish(zone.UID);
//            }
//        }

//        public RelayCommand ShowJournalCommand { get; private set; }
//        void OnShowJournal()
//        {
//            var showXArchiveEventArgs = new ShowXArchiveEventArgs()
//            {
//                Device = Device,
//                Zone = null,
//                Direction = null
//            };
//            ServiceFactory.Events.GetEvent<ShowXArchiveEvent>().Publish(showXArchiveEventArgs);
//        }

//        #region IWindowIdentity Members
//        public string Guid
//        {
//            get { return _guid.ToString(); }
//        }
//        #endregion

//        public override void OnClosed()
//        {
//            CancelBackgroundWorker = true;
//            State.StateChanged -= new Action(OnStateChanged);
//        }
//    }

//    public class PlanViewModel : BaseViewModel
//    {
//        Plan Plan;
//        XDevice Device;
//        public string Name
//        {
//            get { return Plan.Caption; }
//        }
//        public PlanViewModel(Plan plan, XDevice device)
//        {
//            Plan = plan;
//            Device = device;
//            ShowOnPlanCommand = new RelayCommand(OnShowOnPlan);
//        }

//        public RelayCommand ShowOnPlanCommand { get; private set; }
//        void OnShowOnPlan()
//        {
//            ShowOnPlanHelper.ShowDevice(Device, Plan);
//        }
//    }
//}