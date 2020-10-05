using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ARMInfoServer.WCF
{
    public class ProxyStorageService<Contract> : INetContract
    {
        public OVDContainer GetOVDContainer()
        {
            return new OVDContainer { OVDCollection = Storage.OVDCollection };
        }
        public PCInfoContainer GetPCInfoContainer()
        {
            return new PCInfoContainer { PCInfoCollection = Storage.PCInfoCollection };
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


        public static event Action<CommunicationException> ServiceError;
        public static void CallAllClients(Action<ICallbackContract> invoke)
        {
            var keys = Clients.Keys.ToList();
            if (Clients?.Count > 0)
                foreach (var key in keys)
                {
                    try
                    {
                        if (Clients[key] != null)
                            invoke(Clients[key]);
                        else
                            Clients.Remove(key);
                    }
                    catch (Exception err)
                    {
                        try
                        {
                            Clients.Remove(key);
                        }
                        catch (CommunicationException ce)
                        {
                            ServiceError?.Invoke(ce);
                        }
                    }
                }
        }

        public static void NotifyClients(string message)
        {
            CallAllClients((clientCallback) => { clientCallback.ServerToClient(message); });
        }
        internal IProxyStorage Storage { get; }
        public ProxyStorageService()
        {
            //IProxyStorage storage
            Storage = ProxyStorage.Instance;
            Storage.Load();
        }


    }
}
