using System.Collections.Generic;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfoServer
{
    public interface IProxyStorage
    {
        List<IOVDInfo> OVDCollection { get; set; }
        List<IPCInfo> PCInfoCollection { get; set; }

        void Load();
    }
}
