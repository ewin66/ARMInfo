//using static System.Runtime.CompilerServices.IsExternalInit;

namespace InfoCollector.SystemInformation
{
    public interface ISoftwareProduct
    {
        string Name { get; }
        string Version { get; }
    }
    public class SoftwareProduct : ISoftwareProduct
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public override string ToString()
        {
            return (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Version)) ? "Не установлено" : $"{Name} {Version}";
        }
    }
}