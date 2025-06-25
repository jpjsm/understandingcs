using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class EmployeeInfo
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? NtId { get; set; }

    public string? ManagerSysId { get; set; }

    public string? L5Name { get; set; }
}
