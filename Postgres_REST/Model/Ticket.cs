using System;
using System.Collections.Generic;

namespace postgres_rest.Model;

public partial class Ticket
{
    public string? TicketSource { get; set; }

    public string? TicketId { get; set; }

    public string? TicketProject { get; set; }

    public string? TicketAssignmentGroup { get; set; }

    public string? TicketType { get; set; }

    public string? TicketState { get; set; }

    public string? AssignedTo { get; set; }

    public string? AssignedToRole { get; set; }

    public string? AssignedToOrganization { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public string? CreatedBy { get; set; }

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

    public string? Priority { get; set; }

    public string? Severity { get; set; }

    public string? ComputedSeverity { get; set; }

    public string? Urgency { get; set; }

    public string? Title { get; set; }

    public string? TicketDescription { get; set; }

    public string? Labels { get; set; }
}
