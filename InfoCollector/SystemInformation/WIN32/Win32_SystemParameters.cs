using System.Collections.Generic;
using System.Management;

namespace InfoCollector.SystemInformation.WIN32
{
    public class Win32_SystemParameters<T> where T : Win32_Base
    {
        public IEnumerable<Win32_Base> GetInfo()
        {
            foreach (ManagementObject mo in new ManagementObjectSearcher($"SELECT * FROM {typeof(T).Name}").Get())
            {
                var construct = typeof(T).GetConstructor(new[] { mo.GetType() });
                var instance = construct.Invoke(new[] { mo });
                yield return (Win32_Base)instance;
            }
        }
    }
}
