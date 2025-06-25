using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class SnowStateTransition
{
    public string Source { get; set; } = null!;

    public string Id { get; set; } = null!;

    public int TransitionSequence { get; set; }

    public DateTime TransitionDatetime { get; set; }

    public string? State { get; set; }

    public string? Type { get; set; }
}
