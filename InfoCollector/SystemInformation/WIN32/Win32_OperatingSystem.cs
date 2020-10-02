using System;
using System.Management;

namespace InfoCollector.SystemInformation.WIN32
{
    public class Win32_OperatingSystem : Win32_Base
    {
        public Win32_OperatingSystem(ManagementObject mo) : base(mo) { }
        public string BootDevice => mo["BootDevice"].ToString();
        public string BuildNumber => mo["BuildNumber"].ToString();
        public string BuildType => mo["BuildType"].ToString();
        public string Caption => mo["Caption"].ToString();
        public string CodeSet => mo["CodeSet"].ToString();
        public string CountryCode => mo["CountryCode"].ToString();
        public string CreationClassName => mo["CreationClassName"].ToString();
        public string CSCreationClassName => mo["CSCreationClassName"].ToString();
        public string CSDVersion => mo["CSDVersion"].ToString();
        public string CSName => mo["CSName"].ToString();
        public short CurrentTimeZone => (short)mo["CurrentTimeZone"];
        public bool DataExecutionPrevention_Available => (bool)mo["DataExecutionPrevention_Available"];
        public bool DataExecutionPrevention_32BitApplications => (bool)mo["DataExecutionPrevention_32BitApplications"];
        public bool DataExecutionPrevention_Drivers => (bool)mo["DataExecutionPrevention_Drivers"];
        public byte DataExecutionPrevention_SupportPolicy => (byte)mo["DataExecutionPrevention_SupportPolicy"];
        public bool Debug => (bool)mo["Debug"];
        public string Description => mo["Description"].ToString();
        public bool Distributed => (bool)mo["Distributed"];
        public uint EncryptionLevel => (uint)mo["EncryptionLevel"];
        public byte ForegroundApplicationBoost => (byte)mo["ForegroundApplicationBoost"];
        public ulong FreePhysicalMemory => (ulong)mo["FreePhysicalMemory"];
        public ulong FreeSpaceInPagingFiles => (ulong)mo["FreeSpaceInPagingFiles"];
        public ulong FreeVirtualMemory => (ulong)mo["FreeVirtualMemory"];
        public object InstallDate => mo["InstallDate"]; //DateTime
        public uint LargeSystemCache => (uint)mo["LargeSystemCache"];
        public object LastBootUpTime => mo["LastBootUpTime"]; //DateTime
        public object LocalDateTime => mo["LocalDateTime"]; //DateTime
        public string Locale => mo["Locale"].ToString();
        public string Manufacturer => mo["Manufacturer"].ToString();
        public uint MaxNumberOfProcesses => (uint)mo["MaxNumberOfProcesses"];
        public ulong MaxProcessMemorySize => (ulong)mo["MaxProcessMemorySize"];
        public string[] MUILanguages => (string[])mo["MUILanguages"];
        public string Name => mo["Name"].ToString();
        public uint NumberOfLicensedUsers => (uint)mo["NumberOfLicensedUsers"];
        public uint NumberOfProcesses => (uint)mo["NumberOfProcesses"];
        public uint NumberOfUsers => (uint)mo["NumberOfUsers"];
        public uint OperatingSystemSKU => (uint)mo["OperatingSystemSKU"];
        public string Organization => mo["Organization"].ToString();
        public string OSArchitecture => mo["OSArchitecture"].ToString();
        public uint OSLanguage => (uint)mo["OSLanguage"];
        public uint OSProductSuite => (uint)mo["OSProductSuite"];
        public ushort OSType => (ushort)mo["OSType"];
        public string OtherTypeDescription => mo["OtherTypeDescription"].ToString();
        public Boolean PAEEnabled => (Boolean)mo["PAEEnabled"];
        public string PlusProductID => mo["PlusProductID"].ToString();
        public string PlusVersionNumber => mo["PlusVersionNumber"].ToString();
        public bool PortableOperatingSystem => (bool)mo["PortableOperatingSystem"];
        public bool Primary => (bool)mo["Primary"];
        public uint ProductType => (uint)mo["ProductType"];
        public string RegisteredUser => mo["RegisteredUser"].ToString();
        public string SerialNumber => mo["SerialNumber"].ToString();
        public ushort ServicePackMajorVersion => (ushort)mo["ServicePackMajorVersion"];
        public ushort ServicePackMinorVersion => (ushort)mo["ServicePackMinorVersion"];
        public ulong SizeStoredInPagingFiles => (ulong)mo["SizeStoredInPagingFiles"];
        public string Status => mo["Status"].ToString();
        public uint SuiteMask => (uint)mo["SuiteMask"];
        public string SystemDevice => mo["SystemDevice"].ToString();
        public string SystemDirectory => mo["SystemDirectory"].ToString();
        public string SystemDrive => mo["SystemDrive"].ToString();
        public ulong TotalSwapSpaceSize => (ulong)mo["TotalSwapSpaceSize"];
        public ulong TotalVirtualMemorySize => (ulong)mo["TotalVirtualMemorySize"];
        public ulong TotalVisibleMemorySize => (ulong)mo["TotalVisibleMemorySize"];
        public string Version => mo["Version"].ToString();
        public string WindowsDirectory => mo["WindowsDirectory"].ToString();
        public byte QuantumLength => (byte)mo["QuantumLength"];
        public byte QuantumType => (byte)mo["QuantumType"];
    }
}
