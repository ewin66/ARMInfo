using System.Collections.Generic;
using System.Linq;

namespace InfoCollector.PersonalInformation
{
    public class Department : IDepartment
    {
        public static List<IAttestObjectInfo> AllAttestObjects;
        public int id { get; set; }
        public string name_abbreviation { get; set; }
        public string name_full { get; set; }
        public List<int> ovd { get; set; }
        public override string ToString()
        {
            return name_abbreviation;
        }

        public int Id { get => id; set => id = value; }
        public string Abbrev { get => name_abbreviation; set => name_abbreviation = value; }
        public string FullName { get => name_full; set => name_full = value; }
        public List<int> OVD { get => ovd; set => ovd = value; }

        //public List<IAttestObjectInfo> AttestObjects => AllAttestObjects.Where(ao => ao.department.Contains(this.id)).ToList();
        public List<IAttestObjectInfo> AttestObjectInfo(int ovd)
        {
            return AllAttestObjects.Where(ao => ao.department.Contains(this.id) && ao.ovd == ovd).ToList();
        }

        public int? selectedOvd { get; set; }

        public List<IAttestObjectInfo> AttestObjects
        {
            get
            {
                if (selectedOvd != null)
                {
                    return AllAttestObjects.Where(ao => ao.department.Contains(this.id) && ao.ovd == selectedOvd.Value).ToList();
                }
                return AllAttestObjects.Where(ao => ao.department.Contains(this.id)).ToList();
            }
        }
    }
    public interface IDepartment
    {
        int Id { get; set; }
        string Abbrev { get; set; }
        string FullName { get; set; }
        List<int> OVD { get; set; }

        int? selectedOvd { get; set; }

        List<IAttestObjectInfo> AttestObjects { get; }
        //List<IAttestObjectInfo> AttestObjectInfo(int ovd);
    }
}
