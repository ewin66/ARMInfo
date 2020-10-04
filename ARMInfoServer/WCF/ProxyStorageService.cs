using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ARMInfoServer.WCF
{
    public class ProxyStorageService<Contract> : INetContract
    {
        public OVDCollectionContainer GetOVDCollection()
        {
            return new OVDCollectionContainer { OVDCollection = Storage.OVDCollection };
        }
        #region Подключение/Отключение клиентов
        public static event Action<string> NewClientRegistered;
        public static event Action<string> ClientUnregistered;
        // все зарегистрированные клиенты
        public static Dictionary<string, ICallbackContract> Clients = new Dictionary<string, ICallbackContract>();
        public void Register()
        {
            ICallbackContract clientCallback = OperationContext.Current.GetCallbackChannel<ICallbackContract>();
            var mac = clientCallback.GetMacAddress();
            if (!Clients.ContainsKey(mac))
            {
                Clients.Add(mac, clientCallback);
                NewClientRegistered?.Invoke(mac);
            }
        }
        public void Unregister()
        {
            if (Clients != null)
            {
                ICallbackContract clientCallback = OperationContext.Current.GetCallbackChannel<ICallbackContract>();
                var mac = clientCallback.GetMacAddress();
                if (Clients.ContainsKey(mac))
                {
                    Clients.Remove(mac);
                    ClientUnregistered?.Invoke(mac);
                }
            }
        }
        #endregion
        private IProxyStorage Storage { get; }
        public ProxyStorageService(IProxyStorage storage)
        {
            Storage = storage;
            Storage.Load();
        }        
    }
}
