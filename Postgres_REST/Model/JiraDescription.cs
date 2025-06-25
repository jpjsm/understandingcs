using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class JiraDescription
{
    public string IssueSource { get; set; } = null!;

    public string IssueId { get; set; } = null!;

    public string JiraDescription1 { get; set; } = null!;
}
