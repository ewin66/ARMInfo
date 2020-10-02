using System.Management;

namespace InfoCollector.SystemInformation.WIN32
{
    public class Win32_NetworkAdapterConfiguration : Win32_Base
    {
        public Win32_NetworkAdapterConfiguration(ManagementObject mo) : base(mo) { }
        public string Caption => mo["Caption"].ToString();
        public string Description => mo["Description"].ToString();
        public string[] DefaultIPGateway => (string[])mo["DefaultIPGateway"];
        public string DHCPServer => mo["DHCPServer"].ToString();
        public string DNSHostName => mo["DNSHostName"].ToString();
        public string[] DNSServerSearchOrder => (string[])mo["DNSServerSearchOrder"];
        public bool FullDNSRegistrationEnabled => (bool)mo["FullDNSRegistrationEnabled"];
        public string[] IPAddress => (string[])mo["IPAddress"];
        public bool IPEnabled => (bool)mo["IPEnabled"];
        public string[] IPSubnet => (string[])mo["IPSubnet"];
        public string MACAddress => mo["MACAddress"].ToString();
        public string ServiceName => mo["ServiceName"].ToString();
        public override string ToString()
        {
            return $@"Host: ";
        }
    }
}
