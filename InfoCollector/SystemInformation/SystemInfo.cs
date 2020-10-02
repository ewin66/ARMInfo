using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfoCollector.SystemInformation.WIN32;

using Newtonsoft.Json;

namespace InfoCollector.SystemInformation
{
    public class SystemInfo : ISystemInfo
    {
        #region IP & MAC
        public struct MacIpPair
        {
            public string MacAddress;
            public string IpAddress;
        }
        public IEnumerable<MacIpPair> GetIPMacPairs()
        {
            var nacs = new Win32_SystemParameters<Win32_NetworkAdapterConfiguration>().GetInfo();
            foreach (Win32_NetworkAdapterConfiguration nac in nacs)
            {
                if (nac.IPAddress != null)
                    yield return new MacIpPair { IpAddress = nac.IPAddress.First(), MacAddress = nac.MACAddress };
            }
        }

        #endregion
        public string HostName
        {
            get
            {
                return Environment.MachineName;
            }
        }
        public static string IpFilter { get; set; }
        public string IpAddress
        {
            get => (string.IsNullOrEmpty(IpFilter)) ? GetIPMacPairs().First().IpAddress : GetIPMacPairs().First(x => x.IpAddress.Contains(IpFilter)).IpAddress;
        }
        public string MacAddress
        {
            get => (string.IsNullOrEmpty(IpFilter)) ? GetIPMacPairs().First().MacAddress : GetIPMacPairs().First(x => x.IpAddress.Contains(IpFilter)).MacAddress;
        }
        public string OperationSystem
        {
            get
            {
                var os = ((Win32_OperatingSystem)new Win32_SystemParameters<Win32_OperatingSystem>().GetInfo().First());
                return $"{os.Caption} {os.OSArchitecture};\n{WindowsLicenseInfo.GetWindowsProductKey()}";
            }
        }
        public string OperationSystemJson
        {
            get
            {
                var os = ((Win32_OperatingSystem)new Win32_SystemParameters<Win32_OperatingSystem>().GetInfo().First());
                return JsonConvert.SerializeObject(new { Name = os.Caption, Architecture = os.OSArchitecture, LicenseKey = WindowsLicenseInfo.GetWindowsProductKey() });
            }
        }
        public string HDDInfo
        {
            get
            {
                StringBuilder str = new StringBuilder();
                foreach (Win32_DiskDrive hdd in new Win32_SystemParameters<Win32_DiskDrive>().GetInfo())
                {
                    str.AppendLine(string.Join("\n", new[] { $"HDD: {hdd.Caption.Trim()}; SN: {hdd.SerialNumber.Trim()}; {hdd.Size} GB"}));
                }
                return str.ToString().Remove(str.Length - 2);
            }
        }

        public string HDDInfoJson
        {
            get
            {
                StringBuilder str = new StringBuilder();
                List<object> hddList = new List<object>();
                foreach (Win32_DiskDrive hdd in new Win32_SystemParameters<Win32_DiskDrive>().GetInfo())
                {
                    hddList.Add(new { HDD = hdd.Caption.Trim(), SerialNumber = hdd.SerialNumber.Trim(), Size = hdd.Size });
                }
                return JsonConvert.SerializeObject(hddList);
            }
        }
        public ISoftwareProduct AntivirusInfo => SoftwareInfo.GetAntivirusInfo();
        public ISoftwareProduct DefenderNSDInfo => SoftwareInfo.GetDefenderNSDInfo();
        public ISoftwareProduct CryptoPROInfo => SoftwareInfo.GetCryptoProInfo();
        public ISoftwareProduct VPNClientInfo => SoftwareInfo.GetVipNetInfo();
    }





}
