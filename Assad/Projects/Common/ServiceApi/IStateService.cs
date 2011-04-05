﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ServiceApi
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IStateService
    {
        [OperationContract]
        void Initialize();

        [OperationContract]
        CurrentConfiguration GetConfiguration();

        [OperationContract]
        CurrentStates GetStates();

        [OperationContract]
        void ResetState(string devicePath, string command);

        [OperationContract]
        void SetConfiguration(CurrentConfiguration configuration);

        [OperationContract]
        List<Firesec.ReadEvents.journalType> ReadJournal(int startIndex, int count);
    }
}
