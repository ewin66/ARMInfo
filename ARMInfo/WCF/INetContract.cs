﻿using System;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ARMInfo.WCF
{
    [ServiceContract(CallbackContract = typeof(ICallbackContract))]
    public interface INetContract : IRegisteredCallback
    {
        [OperationContract]
        OVDContainer GetOVDContainer();

        [OperationContract]
        PCInfoContainer GetPCInfoContainer();

        [OperationContract]
        string Hello();
    }

}
