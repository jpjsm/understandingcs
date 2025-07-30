using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class Label
{
    public string? TicketSource { get; set; }

    public string? TicketId { get; set; }

    public string? Labels { get; set; }
}
