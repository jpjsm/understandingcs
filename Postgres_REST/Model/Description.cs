using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class Description
{
    public string? TicketSource { get; set; }

    public string? TicketId { get; set; }

    public string? Description1 { get; set; }
}
