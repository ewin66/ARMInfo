using System.Collections.Generic;
using System.Runtime.Serialization;

using InfoCollector.SystemInformation;

namespace ARMInfo.WCF
{
    [DataContract]
    public class PCInfoContainer
    {
        [DataMember]
        public List<IPCInfo> PCInfoCollection { get; set; }
    }

}
