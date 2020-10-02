using System.Collections.Generic;
using System.Linq;

using InfoCollector.PersonalInformation;
using InfoCollector.SystemInformation;

namespace ARMInfo
{
    public static class Getway
    {
        static ITracerException Tracer = new TracerException();

#if !DEBUG
        public static bool isInet = false;
        public static string root = $@"http://{((isInet) ? "83.169.224.42:25780" : "192.168.221.99")}/citsizi/api/v2/";
#else
        static Getway()
        {
            SystemInfo.IpFilter = "10.221.";
        }
        public static string root = $@"http://10.221.0.58/citsizi/api/v2/";
#endif
        public static string addressesUri = root + @"certification/address";
        public static string objectsUri = root + @"certification/object/";
        public static string pcUri = root + @"certification/pc/?page_size=10000";
        public static string ovdUri = root + @"ovd/extend/";
        public static string departmentsUri = root + @"department/extend/";

        public static List<IOVDInfo> LoadOvdInfo()
        {
            Tracer.Append("Загружаю список ОВД");

            AttestObjectInfo.Addresses = (new Report<Address>()).Load(addressesUri);
            OVD.AllObjects = (new Report<AttestObjectInfo>()).Load(objectsUri).Cast<IAttestObjectInfo>().ToList();
            Department.AllAttestObjects = OVD.AllObjects;
            OVD.AllDepartments = (new Report<Department>()).Load(departmentsUri).Cast<IDepartment>().ToList();
            var ovds = (new Report<OVD>()).Load(ovdUri).Select(x => (IOVDInfo)x).ToList();
            AttestObjectInfo.OVDs = ovds;

            Tracer.Append($"Загружено {ovds.Count} ОВД");

            return ovds;
        }

        public static List<IPCInfo> LoadPcInfo(string getParam = "")
        {
            Tracer.Append($"Загружаю информацию о ПК");
            var res = (new Report<PCInfo>()).Load(pcUri + getParam.Trim()).ToList();
            res.ForEach(x => x.inventory_number = x.inventory_number.ToUpper());
            Tracer.Append($"Загружено {res.Count} записей");
            return res.Cast<IPCInfo>().ToList();
        }
        public static void Log(IPCInfo pc)
        {
            Tracer.Append($"Информирую о входе в систему.");
            var result = new Report<PCInfo>().Update(root + $@"certification/pc/log/", (PCInfo)pc);
            Tracer.Append($"Информация передана");
        }

        public static bool UpdatePcInfo(IPCInfo pc)
        {
            Tracer.Append($"Передаю информацию о системе на сервер");
            var result = new Report<PCInfo>().Update(root + $@"certification/pc/{pc.id}/", (PCInfo)pc);
            Tracer.Append($"Информация {((result.IsSuccess)?"":"НЕ ")} передана.");
            return result.IsSuccess;
        }
    }

}
