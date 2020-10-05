using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

using Newtonsoft.Json;

namespace ARMInfoServer
{
    public sealed class ProxyStorage : IProxyStorage
    {
        private static readonly ProxyStorage instance = new ProxyStorage();

        public List<IOVDInfo> OVDCollection { get; set; }
        public List<IPCInfo> PCInfoCollection { get; set; }

        #region urls

#if !DEBUG
        public const string root = @"http://83.169.224.42:25780/citsizi/api/v2/";
#else
        public const string root = @"http://10.221.0.58/citsizi/api/v2/";
#endif


        public string addressUrl;
        public string objectUrl;
        public string pcUrl;
        public string ovdUrl;
        public string departmentUrl;

        #endregion

        public void Load()
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
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ProxyStorage()
        {
        }

        private ProxyStorage()
        {
            addressUrl = root + @"certification/address";
            objectUrl = root + @"certification/object/";
            pcUrl = root + @"certification/pc/?page_size=10000";
            ovdUrl = root + @"ovd/extend/";
            departmentUrl = root + @"department/extend/";
        }

        public static ProxyStorage Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
