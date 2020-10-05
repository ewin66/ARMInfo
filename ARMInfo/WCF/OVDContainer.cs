using System.Collections.Generic;
using System.Runtime.Serialization;

using InfoCollector.PersonalInformation;

namespace ARMInfo.WCF
{
    [DataContract]
    public class OVDContainer
    {
        [DataMember]
        public List<IOVDInfo> OVDCollection { get; set; }
    }

}
