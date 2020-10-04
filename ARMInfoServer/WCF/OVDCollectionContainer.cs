using System.Collections.Generic;
using System.Runtime.Serialization;

using InfoCollector.PersonalInformation;

namespace ARMInfoServer.WCF
{
    [DataContract]
    public class OVDCollectionContainer
    {
        [DataMember]
        public List<IOVDInfo> OVDCollection { get; set; }
    }
}