using System.ServiceModel;

namespace ARMInfoServer.WCF
{
    [ServiceContract]
    public interface IRegisteredCallback
    {
        [OperationContract(IsOneWay = true)]
        void Register();/// Зарегистрировать клиента
        [OperationContract(IsOneWay = true)]
        void Unregister();/// Снять регистрацию клиента
    }
}