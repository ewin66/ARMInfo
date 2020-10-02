namespace InfoCollector.PersonalInformation
{
    public interface IPersonalInfo
    {
        IAttestObjectInfo AttestObjectInfo { get; set; }
        IDepartment Department { get; set; }
        string FullName { get; set; }
        string InventoryNumber { get; set; }
        bool IsUnknownInventoryNumber { get; set; }
        string Room { get; set; }
    }
}
