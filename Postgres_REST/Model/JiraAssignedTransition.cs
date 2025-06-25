using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class JiraAssignedTransition
{
    public string IssueSource { get; set; } = null!;

    public string IssueId { get; set; } = null!;

    public int TransitionSequence { get; set; }

    public DateTime TransitionDatetime { get; set; }

    public string AssignedFrom { get; set; } = null!;

    public string AssignedTo { get; set; } = null!;
}
