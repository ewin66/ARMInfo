namespace InfoCollector.PersonalInformation
{
    public class PersonalInfo : IPersonalInfo
    {
        public string FullName { get; set; }
        public string InventoryNumber { get; set; }
        public bool IsUnknownInventoryNumber { get; set; }
        public string Room { get; set; }
        public IAttestObjectInfo AttestObjectInfo { get; set; }
        public IDepartment Department { get; set; }
        public PersonalInfo()
        {
            this.AttestObjectInfo = new AttestObjectInfo();
        }


        public PersonalInfo(AttestObjectInfo aoi)
        {
            this.AttestObjectInfo = aoi;
        }
    }
}
