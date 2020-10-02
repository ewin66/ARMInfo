using System.Collections.Generic;
using System.Linq;

namespace InfoCollector.PersonalInformation
{
    public interface IAttestObjectInfo
    {
        int id { get; }
        string name { get; }
        int ovd { get; }
        List<int> department { get; }
        int address { get; }
        string FullName { get; }
        string Address { get; }
    }

    public class AttestObjectInfo : IAttestObjectInfo
    {
        public static List<Address> Addresses { get; set; } = new List<Address>();
        public static List<IOVDInfo> OVDs { get; set; } = new List<IOVDInfo>();
        public int id { get; set; }
        public string name { get; set; }
        public int ovd { get; set; }
        public List<int> department { get; set; }
        public int address { get; set; }
        public string Address => Addresses?.First(a => a.id == this.address).address;
        public string FullName => name;
    }
}
