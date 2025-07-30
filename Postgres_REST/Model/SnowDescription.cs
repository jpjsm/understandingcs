using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class SnowDescription
{
    public string Source { get; set; } = null!;

    public string Id { get; set; } = null!;

    public string TicketType { get; set; } = null!;

    public string Description { get; set; } = null!;
}
