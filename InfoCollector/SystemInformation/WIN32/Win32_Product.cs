using System.Management;

namespace InfoCollector.SystemInformation.WIN32
{
    public class Win32_Product : Win32_Base
    {
        public Win32_Product(ManagementObject mo) : base(mo) { }

        public string Caption => mo["Caption"].ToString();
        public string Description => mo["Description"].ToString();
        public string InstallLocation => mo["InstallLocation"].ToString();
        public string InstallSource => mo["InstallSource"].ToString();
        public string LocalPackage => mo["LocalPackage"].ToString();
        public string Name => mo["Name"].ToString();

        public string Version => mo["Version"].ToString();
    }
}
