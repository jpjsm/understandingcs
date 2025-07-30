using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class JiraStateTransition
{
    public string IssueSource { get; set; } = null!;

    public string IssueId { get; set; } = null!;

    public int TransitionSequence { get; set; }

    public DateTime TransitionDatetime { get; set; }

    public string TransitionBy { get; set; } = null!;

    public string StateFrom { get; set; } = null!;

    public string StateTo { get; set; } = null!;
}
