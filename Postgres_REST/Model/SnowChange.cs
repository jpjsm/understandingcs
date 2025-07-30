using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class SnowChange
{
    public string Source { get; set; } = null!;

    public string Id { get; set; } = null!;

    public string? State { get; set; }

    public string? Priority { get; set; }

    public string? Severity { get; set; }

    public string Title { get; set; } = null!;

    public string? Category { get; set; }

    public string? AssignedTo { get; set; }

    public string? AssignedToRole { get; set; }

    public string? AssignedToOrganization { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? CreatedByRole { get; set; }

    public string? CreatedByOrganization { get; set; }

    public DateTime? ClosedDatetime { get; set; }

    public string? ClosedBy { get; set; }

    public string? ClosedByRole { get; set; }

    public string? ClosedByOrganization { get; set; }

    public DateTime? FirstStateDatetime { get; set; }

    public string? Urgency { get; set; }

    public string? AssignGroup { get; set; }

    public string? Domain { get; set; }

    public string? Product { get; set; }
}
