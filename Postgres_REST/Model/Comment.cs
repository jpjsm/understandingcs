using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class Comment
{
    public string? TicketSource { get; set; }

    public string? TicketId { get; set; }

    public DateTime? CommentDatetime { get; set; }

    public string? CommentAuthor { get; set; }

    public string? CommentType { get; set; }

    public string? Comment1 { get; set; }
}
