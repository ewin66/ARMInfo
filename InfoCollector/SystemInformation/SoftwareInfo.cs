using System;
using System.IO;
using System.Linq;

using Microsoft.Win32;

namespace InfoCollector.SystemInformation
{
    public class SoftwareInfo
    {
        private static string GetRegistryInfo(string path, Func<RegistryKey, string> getInfo)
        {
            var regKey = Registry.LocalMachine.OpenSubKey(path);
            if (regKey != null)
            {
                return getInfo(regKey);
            }
            return null;
        }

        public static ISoftwareProduct GetDefenderNSDInfo()
        {
            var key = $@"SOFTWARE\Security Code\Secret Net Studio\Client\Control Center";
            var version = GetRegistryInfo(key, (rk) => rk.GetValue("Version").ToString());

            var programPath = GetRegistryInfo(key, (rk) => rk.GetValue("InstallDir").ToString());

            programPath = programPath?.Substring(0, programPath.IndexOf(@"\Client\") + 8);

            if (File.Exists(programPath + "SnSrv.exe"))
            {
                return new SoftwareProduct { Name = "Secret Net Studio", Version = version };
            }
            else
            {
                return new SoftwareProduct();
            }
        }

        public static ISoftwareProduct GetVipNetInfo()
        {

            var version = GetRegistryInfo(@"SOFTWARE\Infotecs\Setup\Products\InfoTeCS-Client",
        (rk) => string.Join(".",
        new[]  {
            rk.GetValue("VersionMajor").ToString(),
            rk.GetValue("VersionMinor").ToString(),
            rk.GetValue("VersionSpack").ToString(),
            rk.GetValue("VersionBuild").ToString()
        }
            ));

            var monitorPath = GetRegistryInfo(@"SOFTWARE\Infotecs\FeaturesAndComponents\Monitor_Feature",
            (rk) => rk.GetValue("MainProgramPath").ToString());
            if (File.Exists(monitorPath))
            {
                return new SoftwareProduct
                {
                    Name = "VipNet Client",
                    Version = version
                };
            }
            else
            {
                return new SoftwareProduct();
            }
        }

        public static ISoftwareProduct GetAntivirusInfo()
        {
            var key = $@"SOFTWARE\WOW6432Node\KasperskyLab\protected\KES\environment";
            var version = GetRegistryInfo(key,
            (rk) => rk.GetValue("Ins_ProductVersion").ToString());

            var programPath = GetRegistryInfo(key,
            (rk) => rk.GetValue("ARKMON_RESTART_PATH").ToString());
            if (File.Exists(programPath))
            {
                return new SoftwareProduct { Name = "Kaspersky Endpoint Security", Version = version };
            }
            else
            {
                return new SoftwareProduct();
            }
        }

        public static ISoftwareProduct GetCryptoProInfo()
        {
            var key = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData\S-1-5-18\Products\7AB5E7046046FB044ACD63458B5F481C\InstallProperties";
            var version = GetRegistryInfo(key,
            (rk) => rk.GetValue("DisplayVersion").ToString());

            var monitorPath = GetRegistryInfo(key,
            (rk) => rk.GetValue("InstallLocation").ToString() + @"\cpcspi.dll");
            if (File.Exists(monitorPath))
            {
                return new SoftwareProduct { Name = "Crypto Pro", Version = version };
            }
            else
            {
                return new SoftwareProduct();
            }
        }
    }
}
