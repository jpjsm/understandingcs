using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class JiraComment
{
    public string IssueSource { get; set; } = null!;

    public string IssueId { get; set; } = null!;

    public DateTime CommentDatetime { get; set; }

    public string CommentAuthor { get; set; } = null!;

    public string JiraComment1 { get; set; } = null!;
}
