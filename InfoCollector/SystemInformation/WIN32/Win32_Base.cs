using System.Linq;
using System.Management;
using System.Text;

namespace InfoCollector.SystemInformation.WIN32
{
    public abstract class Win32_Base
    {
        protected ManagementObject mo { get; set; }
        public Win32_Base(ManagementObject mo)
        {
            this.mo = mo;
        }
    }
}
