using System.Collections.Generic;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfo.WCF
{
    public interface IProxyStorage
    {
        List<IOVDInfo> OVDCollection { get; set; }
        List<IPCInfo> PCInfoCollection { get; set; }

        void Load();
    }
}
