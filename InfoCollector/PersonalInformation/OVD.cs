using System.Collections.Generic;
using System.Linq;

namespace InfoCollector.PersonalInformation
{
    public class OVD : IOVDInfo
    {
        public static List<IDepartment> AllDepartments { get; set; }
        public static List<IAttestObjectInfo> AllObjects { get; set; }
        public int id { get; set; }
        public string name_abbreviation { get; set; }
        public string name_full { get; set; }
        public List<IDepartment> Departments
        {
            get
            {
                var result = AllDepartments.Where(x => x.OVD.Contains(this.id)).ToList();
                result.ForEach(x => x.selectedOvd = this.id);
                return result;
            }
        }
        public List<IAttestObjectInfo> AttestObjects { get => AllObjects.Where(x => x.ovd == this.id).Cast<IAttestObjectInfo>().ToList(); }

        public int Id { get => id; set => id = value; }
        public string Abbrev { get => name_abbreviation; set => name_abbreviation = value; }
        public string FullName { get => name_full; set => name_full = value; }
    }
}
