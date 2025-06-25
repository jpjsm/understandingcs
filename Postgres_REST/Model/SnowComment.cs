using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class SnowComment
{
    public string Source { get; set; } = null!;

    public string Id { get; set; } = null!;

    public string TicketType { get; set; } = null!;

    public DateTime CommentDatetime { get; set; }

    public string CommentAuthor { get; set; } = null!;

    public string CommentType { get; set; } = null!;

    public string Comment { get; set; } = null!;
}
