using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ARMInfoServer.WCF;

namespace ARMInfoServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }


    public class Server
    {
        #region private fields
        Uri address;
        NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
        ServiceHost service;
        private CommunicationState state; // State server
        #endregion

        public IPAddress ServerIP { get; set; }
#if DEBUG
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
                service.Faulted += (s, e) => { State = CommunicationState.Opened; };


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

        public async void Start()
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
                task.Start();
        }

        public async void Stop()
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
