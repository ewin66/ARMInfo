using System.ServiceModel;

namespace ARMInfo.WCF
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
