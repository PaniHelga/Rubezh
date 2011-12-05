﻿using System;
using System.Collections.Generic;
using FiresecAPI.Models;
using Infrastructure.Common;

namespace InstructionsModule.ViewModels
{
    public class InstructionViewModel : BaseViewModel
    {
        public InstructionViewModel(Instruction instruction)
        {
            Instruction = instruction;
        }

        public Instruction Instruction { get; private set; }

        public StateType StateType
        {
            get { return Instruction.StateType; }
        }

        public string InstructionText
        {
            get { return Instruction.Text; }
        }

        public ulong InstructionNo
        {
            get { return Instruction.No; }
        }

        public InstructionType InstructionType
        {
            get { return Instruction.InstructionType; }
        }

        public List<ulong?> InstructionZones
        {
            get { return Instruction.InstructionZonesList; }
        }

        public List<Guid> InstructionDevices
        {
            get { return Instruction.InstructionDevicesList; }
        }

        public void Update()
        {
            OnPropertyChanged("Instruction");
            OnPropertyChanged("InstructionType");
            OnPropertyChanged("StateType");
            OnPropertyChanged("InstructionText");
            OnPropertyChanged("InstructionNo");
            OnPropertyChanged("InstructionType");
            OnPropertyChanged("InstructionDevices");
            OnPropertyChanged("InstructionZones");
        }
    }
}