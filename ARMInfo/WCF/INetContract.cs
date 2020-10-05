using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfo.WCF
{
    [ServiceContract]
    public interface ICallbackContract
    {
        [OperationContract(IsOneWay = true)]
        void ServerToClient(string message);

        //[OperationContract(IsOneWay = false)]
        //string GetClientHostName();

        [OperationContract(IsOneWay = false)]
        string GetMacAddress();
    }
    [ServiceContract]
    public interface IRegisteredCallback
    {
        [OperationContract(IsOneWay = true)]
        void Register();/// Зарегистрировать клиента
        [OperationContract(IsOneWay = true)]
        void Unregister();/// Снять регистрацию клиента
    }
    [ServiceContract(CallbackContract = typeof(ICallbackContract))]
    public interface INetContract : IRegisteredCallback
    {
        [OperationContract]
        OVDContainer GetOVDContainer();

        [OperationContract]
        PCInfoContainer GetPCInfoContainer();
    }


    [DataContract]
    public class OVDContainer
    {
        [DataMember]
        public List<IOVDInfo> OVDCollection { get; set; }
    }

    [DataContract]
    public class PCInfoContainer
    {
        [DataMember]
        public List<IPCInfo> PCInfoCollection { get; set; }
    }

}
