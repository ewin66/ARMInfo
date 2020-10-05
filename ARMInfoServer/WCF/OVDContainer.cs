using System.Collections.Generic;
using System.Runtime.Serialization;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfoServer.WCF
{
    [DataContract]
    public class OVDContainer
    {
        [DataMember]
        public List<IOVDInfo> OVDCollection { get; set; }
    }
}