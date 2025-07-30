using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class StateTransition
{
    public string? TicketSource { get; set; }

    public string? TicketId { get; set; }

    public int? TransitionSequence { get; set; }

    public DateTime? TransitionDatetime { get; set; }

    public string? StateFrom { get; set; }

    public string? StateTo { get; set; }
}
