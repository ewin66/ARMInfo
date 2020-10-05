using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ARMInfo.WCF
{
    public class TcpBindingFactory
    {
        public static NetTcpBinding Create()
        {
            return new NetTcpBinding(SecurityMode.None)
            {
                MaxBufferPoolSize = 1024 * 1024 * 10,
                MaxBufferSize = 1024 * 1024 * 10,
                MaxReceivedMessageSize = 1024 * 1024 * 10
            };
        }
    }
}
