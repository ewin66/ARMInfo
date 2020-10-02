using System.Management;

namespace InfoCollector.SystemInformation.WIN32
{
    public class Win32_DiskDrive : Win32_Base
    {
        public Win32_DiskDrive(ManagementObject mo) : base(mo) { }
        public string Caption => mo["Caption"].ToString();
        public string Description => mo["Description"].ToString();
        public string FirmwareRevision => mo["FirmwareRevision"].ToString();
        public string DeviceID => mo["DeviceID"].ToString();
        public string MediaType => mo["MediaType"].ToString();
        public string Model => mo["Model"].ToString();
        public string Name => mo["Name"].ToString();

        public string PNPDeviceID => mo["PNPDeviceID"].ToString();
        public string SerialNumber => mo["SerialNumber"].ToString();

        public string Status => mo["Status"].ToString();
        public string SystemName => mo["SystemName"].ToString();

        public long Size => long.Parse(mo["Size"].ToString()) / 1000 / 1000 / 1000;
    }
}
