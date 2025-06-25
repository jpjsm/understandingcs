using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class TimeD
{
    public DateTime? Date { get; set; }

    public string? CalYr { get; set; }

    public string? CalQtr { get; set; }

    public string? CalMnth { get; set; }

    public string? CalMnthNum { get; set; }

    public string? FiscYr { get; set; }

    public string? FiscQtr { get; set; }

    public string? FiscMnthNum { get; set; }

    public string? IrcQtr { get; set; }
}
