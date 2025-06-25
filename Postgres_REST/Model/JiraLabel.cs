using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class JiraLabel
{
    public string IssueSource { get; set; } = null!;

    public string IssueId { get; set; } = null!;

    public string JiraLabel1 { get; set; } = null!;
}
