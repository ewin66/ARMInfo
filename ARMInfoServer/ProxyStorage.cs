using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

using Newtonsoft.Json;

namespace ARMInfoServer
{
    public class ProxyStorage
    {
        public static List<IOVDInfo> OVDCollection { get; set; }
        public static List<IPCInfo> PCInfoCollection { get; set; }

#if DEBUG
        public static string root = $@"http://83.169.224.42:25780/citsizi/api/v2/";
#else
        public const string root = @"http://10.221.0.58/citsizi/api/v2/";
#endif
        public string addressUrl = root + @"certification/address";
        public string objectUrl = root + @"certification/object/";
        public string pcUrl = root + @"certification/pc/?page_size=10000";
        public string ovdUrl = root + @"ovd/extend/";
        public string departmentUrl = root + @"department/extend/";

        public void Init()
        {
            AttestObjectInfo.Addresses = (new Report<Address>()).Load(addressUrl);
            OVD.AllObjects = (new Report<AttestObjectInfo>()).Load(objectUrl).Cast<IAttestObjectInfo>().ToList();
            Department.AllAttestObjects = OVD.AllObjects;
            OVD.AllDepartments = (new Report<Department>()).Load(departmentUrl).Cast<IDepartment>().ToList();
            var ovds = (new Report<OVD>()).Load(ovdUrl).Select(x => (IOVDInfo)x).ToList();
            AttestObjectInfo.OVDs = ovds;
            OVDCollection = ovds;

            PCInfoCollection = (new Report<PCInfo>()).Load(pcUrl).Cast<IPCInfo>().ToList();
        }
    }

}
