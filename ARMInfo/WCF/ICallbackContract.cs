using System.ServiceModel;

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

}
