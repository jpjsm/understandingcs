using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class JiraIssue
{
    public string IssueSource { get; set; } = null!;

    public string IssueId { get; set; } = null!;

    public string IssueType { get; set; } = null!;

    public string? IssueState { get; set; }

    public string? IssuePriority { get; set; }

    public string? IssueSeverity { get; set; }

    public string IssueTitle { get; set; } = null!;

    public string? IssueLabels { get; set; }

    public string? AssignedTo { get; set; }

    public string? AssignedToRole { get; set; }

    public string? AssignedToOrganization { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? CreatedByRole { get; set; }

    public string? CreatedByOrganization { get; set; }

    public DateTime? ResolutionDatetime { get; set; }

    public string? ResolutionState { get; set; }

    public string? ResolutionDescription { get; set; }

    public DateTime? ClosedDatetime { get; set; }

    public string? ClosedBy { get; set; }

    public string? ClosedByRole { get; set; }

    public string? ClosedByOrganization { get; set; }

    public DateTime? FirstStateDatetime { get; set; }

    public string? IssueProject { get; set; }

    public string? IssueAssignmentGroup { get; set; }

    public string? ComputedSeverity { get; set; }
}
