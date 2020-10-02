using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace InfoCollector.PersonalInformation
{
    public interface IOVDInfo
    {
        int Id { get; }
        string Abbrev { get; }
        string FullName { get; }
        List<IDepartment> Departments { get; }
        List<IAttestObjectInfo> AttestObjects { get; }
    }
}
