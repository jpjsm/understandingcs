using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class AssignedTransition
{
    public string? TicketSource { get; set; }

    public string? TicketId { get; set; }

    public int? TransitionSequence { get; set; }

    public DateTime? TransitionDatetime { get; set; }

    public string? AssignedFrom { get; set; }

    public string? AssignedTo { get; set; }
}
