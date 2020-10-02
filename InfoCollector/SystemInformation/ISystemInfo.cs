namespace InfoCollector.SystemInformation
{
    public interface ISystemInfo
	{
		string HostName { get; }
		string IpAddress { get; }
		string MacAddress { get; }
		string OperationSystem { get; }
		string OperationSystemJson { get; }
		string HDDInfoJson { get; }
		ISoftwareProduct AntivirusInfo { get; }
		ISoftwareProduct DefenderNSDInfo { get; }
		ISoftwareProduct CryptoPROInfo { get; }
		ISoftwareProduct VPNClientInfo { get; }
	}
}
