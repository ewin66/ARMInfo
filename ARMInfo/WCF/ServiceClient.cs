using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfo.WCF
{
    public class ServiceClient
    {
        private DuplexChannelFactory<INetContract> channelFactory = null;
        private INetContract channel = null;
        private EndpointAddress serverEndpoint = null;

        private CallbackHandler _callbackHandler = new CallbackHandler();

        public bool IsConnected { get; private set; }

        public event Action<bool> ConnectedChanged;
        public event Action<string> RecievedServerMessage;
        public event Action<CommunicationState> ChannelStateChanged;
        public event Action<CommunicationException> CommunicationError;

        public ServiceClient()
        {
            _callbackHandler.RecievedServerMessage += (msg) => RecievedServerMessage?.Invoke(msg);
            ConnectedChanged += (state) => { IsConnected = state; };
        }

        public void InitService(IPAddress serverIP, uint serverPort, string serviceName)
        {
            Uri address = new Uri($"net.tcp://{serverIP}:{serverPort}/{serviceName}");
            serverEndpoint = new EndpointAddress(address);
        }
        public string Hello()
        {
            try
            {
                if (IsConnected)
                {
                    
                    return channel.Hello();
                }
            }
            catch (CommunicationException ce)
            {
                CommunicationError?.Invoke(ce);
            }
            return null;
        }
        public List<IOVDInfo> DownloadOVDList()
        {
            try
            {
                if (IsConnected)
                {
                    var container = _callbackHandler.Channel.GetOVDContainer();
                    return container.OVDCollection;
                }
            }
            catch (CommunicationException ce)
            {
                CommunicationError?.Invoke(ce);
            }
            return null;
        }
        public List<IPCInfo> DownloadPcInfoList()
        {
            try
            {
                if (IsConnected)
                {
                    var container = _callbackHandler.Channel.GetPCInfoContainer();
                    return container.PCInfoCollection;
                }
            }
            catch (CommunicationException ce)
            {
                CommunicationError?.Invoke(ce);
            }
            return null;
        }

        public void Connect()
        {
            if (serverEndpoint == null) return;
            //var connecting = Task.Factory.StartNew(() => {
            _callbackHandler.Channel = null;
            channel = null;
            if (channelFactory == null)
            {
                channelFactory = new DuplexChannelFactory<INetContract>(
                    new InstanceContext(_callbackHandler),
                    TcpBindingFactory.Create(),
                    serverEndpoint
                );
            }
            if (channel == null)
            {
                EventHandler stateChanged = (s, e) =>
                {
                    ChannelStateChanged?.Invoke(channelFactory.State);
                };
                channelFactory.Faulted += stateChanged;
                channelFactory.Opening += stateChanged;
                channelFactory.Opened += stateChanged;
                channelFactory.Closing += stateChanged;
                channelFactory.Closed += stateChanged;

                try
                {
                    channel = channelFactory.CreateChannel();

                    if (channelFactory != null && channel != null)
                    {
                        if (channelFactory.State == CommunicationState.Opened)
                        {
                            _callbackHandler.Channel = channel;
                            _callbackHandler.Channel.Register();
                            ConnectedChanged?.Invoke(true);
                        }
                    }
                }
                catch (Exception e)
                {
                    channelFactory = null;
                    channel = null;
                    ChannelStateChanged?.Invoke(CommunicationState.Faulted);
                    CommunicationError?.Invoke(e as CommunicationException);
                    ConnectedChanged?.Invoke(false);
                }
            }
            //});
        }

        public void Disconnect()
        {
            // var disconnecting = Task.Factory.StartNew(() =>            {
            if (channelFactory?.State == CommunicationState.Opened)
            {
                try
                {
                    _callbackHandler.Channel.Unregister();
                    System.Threading.Thread.Sleep(200);
                    this.channelFactory?.Close();
                    _callbackHandler.Channel = null;
                    this.channel = null;
                    this.channelFactory = null;
                    ConnectedChanged?.Invoke(false);
                }
                catch (CommunicationException ce)
                {
                    CommunicationError?.Invoke(ce);
                    _callbackHandler.Channel = null;
                    this.channel = null;
                    this.channelFactory = null;
                    ConnectedChanged?.Invoke(false);
                }

            }
            //});

        }
    }
}
