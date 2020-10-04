using System.ServiceModel;

namespace ARMInfoServer.WCF
{
    [ServiceContract]
    public interface ICallbackContract
    {
        //[OperationContract(IsOneWay = false)]
        //string GetClientHostName();

        [OperationContract(IsOneWay = false)]
        string GetMacAddress();
    }
}