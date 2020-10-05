using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

using ARMInfoServer.WCF;

using InfoCollector.PersonalInformation;

namespace ARMInfoServer
{
    public class Server
    {
        #region private fields
        Uri address;
        NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
        ServiceHost service;
        private CommunicationState state; // State server

        ProxyStorageService<INetContract> proxyStorageService { get; set; }
        #endregion

        public IPAddress ServerIP { get; set; }
#if !DEBUG
            = new IPAddress(new byte[] { 192, 168, 1, 100 });
#else
            = new IPAddress(new byte[] { 10, 221, 0, 2 });
#endif

        public uint ServerPort { get; set; } = 10221;
        public string ServiceName { get; set; } = "ARMInfo";

        public CommunicationState State
        {
            get => state; private set
            {
                state = value;
                StateChanged?.Invoke(state);
            }
        }

        public IDictionary<string, ICallbackContract> Clients => ProxyStorageService<INetContract>.Clients;

        public List<IOVDInfo> OVDCollection => proxyStorageService.Storage.OVDCollection;


        #region EVENTS
        /// <summary>
        /// Состояние сервера изменилось
        /// </summary>
        public event Action<CommunicationState> StateChanged;
        /// <summary>
        /// Сервер выдал ошибку
        /// </summary>
        public event Action<CommunicationException> ServerCatchError;
        /// <summary>
        /// Зарегистрировался новый клиент
        /// </summary>
        public event Action<string> ClientRegistered;
        /// <summary>
        /// Клиент отключился от сервиса
        /// </summary>
        public event Action<string> ClientUnregistered;
        /// <summary>
        /// Список клиентов изменился
        /// </summary>
        public event Action ClientsListChanged;
        #endregion

        /// <summary>
        /// Отправить сообщение всем клиентам
        /// </summary>
        /// <param name="message"></param>
        public void NotifyAllClients(string message) => ProxyStorageService<INetContract>.NotifyClients(message);

        #region START/STOP 

        public Server()
        {
            proxyStorageService = new ProxyStorageService<INetContract>();

        }

        private void SetUp()
        {
            address = new Uri($"net.tcp://{ServerIP}:{ServerPort}/{ServiceName}");
            service = new ServiceHost(typeof(ProxyStorageService<INetContract>), address);
            if (State != CommunicationState.Opened)
            {
                service.Opening += (s, e) => { State = CommunicationState.Opening; };
                service.Closing += (s, e) => { State = CommunicationState.Closing; };
                service.Closed += (s, e) => { State = CommunicationState.Closed; };
                service.Opened += (s, e) => { State = CommunicationState.Opened; };
                service.Faulted += (s, e) => { State = CommunicationState.Faulted; };


                ProxyStorageService<INetContract>.ServiceError += (err) =>
                {
                    ServerCatchError?.Invoke(err);
                    var cd = service.ChannelDispatchers;
                };

                ProxyStorageService<INetContract>.NewClientRegistered += (mac) => { ClientRegistered?.Invoke(mac); };
                ProxyStorageService<INetContract>.ClientUnregistered += (mac) => { ClientUnregistered?.Invoke(mac); };

                try
                {
                    service.AddServiceEndpoint(typeof(INetContract), binding, address.AbsoluteUri);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        public void Start()
        {
            var task = new Task(() =>
            {
                try
                {
                    if (service?.State != CommunicationState.Opened ||
                        service?.State != CommunicationState.Opening || service == null)
                    {
                        SetUp();
                    }
                }
                catch (Exception ex)
                {
                    service = null;
                    State = CommunicationState.Faulted;
                }
                if (service != null)
                {
                    service?.Open();
                }
            });

            if (State != CommunicationState.Opened)
            {
                //task.Start();
                try
                {
                    if (service?.State != CommunicationState.Opened ||
                        service?.State != CommunicationState.Opening || service == null)
                    {
                        SetUp();
                    }
                }
                catch (Exception ex)
                {
                    service = null;
                    State = CommunicationState.Faulted;
                }
                if (service != null)
                {
                    service?.Open();
                }
            }

        }

        public void Stop()
        {
            if (State == CommunicationState.Opened)
            {
                Task.Factory.StartNew(() =>
                {
                    //Dispatcher.Invoke(() => {
                    service?.Close();
                    service = null;
                    //});
                });
            }
        }
        #endregion
    }
}
