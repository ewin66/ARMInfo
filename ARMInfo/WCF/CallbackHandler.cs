using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using InfoCollector.SystemInformation;

namespace ARMInfo.WCF
{
    [KnownType(typeof(CallbackHandler))]
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CallbackHandler : ICallbackContract
    {
        public INetContract Channel { get; set; }
        static CallbackHandler()
        {
#if !DEBUG
            SystemInfo.IpFilter = "10.221.";
#endif
        }

        public CallbackHandler()
        {
            MacAddress = new SystemInfo().MacAddress;
        }

        private readonly string MacAddress;

        public string GetMacAddress() => MacAddress;


        public void ServerToClient(string message)
        {
            RecievedServerMessage(message);
        }

        public event Action<string> RecievedServerMessage;

    }
}
