using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace postgres_rest.Model;

public partial class BusinessReporterContext : DbContext
{
    public BusinessReporterContext()
    {
    }

    public BusinessReporterContext(DbContextOptions<BusinessReporterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AssignedTransition> AssignedTransitions { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Description> Descriptions { get; set; }

    public virtual DbSet<EmployeeInfo> EmployeeInfos { get; set; }

    public virtual DbSet<JiraAssignedTransition> JiraAssignedTransitions { get; set; }

    public virtual DbSet<JiraComment> JiraComments { get; set; }

    public virtual DbSet<JiraDescription> JiraDescriptions { get; set; }

    public virtual DbSet<JiraIssue> JiraIssues { get; set; }

    public virtual DbSet<JiraLabel> JiraLabels { get; set; }

    public virtual DbSet<JiraStateTransition> JiraStateTransitions { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<SnowAssignedTransition> SnowAssignedTransitions { get; set; }

    public virtual DbSet<SnowChange> SnowChanges { get; set; }

    public virtual DbSet<SnowComment> SnowComments { get; set; }

    public virtual DbSet<SnowDescription> SnowDescriptions { get; set; }

    public virtual DbSet<SnowIncident> SnowIncidents { get; set; }

    public virtual DbSet<SnowStateTransition> SnowStateTransitions { get; set; }

    public virtual DbSet<StateTransition> StateTransitions { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TimeD> TimeDs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=oppy-db-01.cec.delllabs.net;Database=business_reporter;Port=5432;User Id=oppy;Password=Mars2004-2018;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssignedTransition>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("assigned_transitions");

            entity.Property(e => e.AssignedFrom)
                .HasColumnType("character varying")
                .HasColumnName("assigned_from");
            entity.Property(e => e.AssignedTo)
                .HasMaxLength(64)
                .HasColumnName("assigned_to");
            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .HasColumnName("ticket_id");
            entity.Property(e => e.TicketSource)
                .HasMaxLength(50)
                .HasColumnName("ticket_source");
            entity.Property(e => e.TransitionDatetime).HasColumnName("transition_datetime");
            entity.Property(e => e.TransitionSequence).HasColumnName("transition_sequence");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("comments");

            entity.Property(e => e.Comment1).HasColumnName("comment");
            entity.Property(e => e.CommentAuthor)
                .HasMaxLength(64)
                .HasColumnName("comment_author");
            entity.Property(e => e.CommentDatetime).HasColumnName("comment_datetime");
            entity.Property(e => e.CommentType).HasColumnName("comment_type");
            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .HasColumnName("ticket_id");
            entity.Property(e => e.TicketSource)
                .HasMaxLength(50)
                .HasColumnName("ticket_source");
        });

        modelBuilder.Entity<Description>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("descriptions");

            entity.Property(e => e.Description1).HasColumnName("description");
            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .HasColumnName("ticket_id");
            entity.Property(e => e.TicketSource)
                .HasMaxLength(50)
                .HasColumnName("ticket_source");
        });

        modelBuilder.Entity<EmployeeInfo>(entity =>
        {
            entity.HasKey(e => new { e.EmployeeId, e.FullName }).HasName("employee_info_pkey");

            entity.ToTable("employee_info");

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(64)
                .HasColumnName("full_name");
            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnName("email");
            entity.Property(e => e.L5Name)
                .HasMaxLength(64)
                .HasColumnName("l5_name");
            entity.Property(e => e.ManagerSysId)
                .HasMaxLength(256)
                .HasColumnName("manager_sys_id");
            entity.Property(e => e.NtId)
                .HasMaxLength(64)
                .HasColumnName("nt_id");
        });

        modelBuilder.Entity<JiraAssignedTransition>(entity =>
        {
            entity.HasKey(e => new { e.IssueSource, e.IssueId, e.TransitionSequence }).HasName("jira_assigned_transitions_pkey");

            entity.ToTable("jira_assigned_transitions");

            entity.Property(e => e.IssueSource)
                .HasMaxLength(50)
                .HasColumnName("issue_source");
            entity.Property(e => e.IssueId)
                .HasMaxLength(50)
                .HasColumnName("issue_id");
            entity.Property(e => e.TransitionSequence).HasColumnName("transition_sequence");
            entity.Property(e => e.AssignedFrom)
                .HasMaxLength(64)
                .HasColumnName("assigned_from");
            entity.Property(e => e.AssignedTo)
                .HasMaxLength(64)
                .HasColumnName("assigned_to");
            entity.Property(e => e.TransitionDatetime).HasColumnName("transition_datetime");
        });

        modelBuilder.Entity<JiraComment>(entity =>
        {
            entity.HasKey(e => new { e.IssueSource, e.IssueId, e.CommentDatetime, e.CommentAuthor }).HasName("jira_comments_pkey");

            entity.ToTable("jira_comments");

            entity.Property(e => e.IssueSource)
                .HasMaxLength(50)
                .HasColumnName("issue_source");
            entity.Property(e => e.IssueId)
                .HasMaxLength(50)
                .HasColumnName("issue_id");
            entity.Property(e => e.CommentDatetime).HasColumnName("comment_datetime");
            entity.Property(e => e.CommentAuthor)
                .HasMaxLength(64)
                .HasColumnName("comment_author");
            entity.Property(e => e.JiraComment1).HasColumnName("jira_comment");
        });

        modelBuilder.Entity<JiraDescription>(entity =>
        {
            entity.HasKey(e => new { e.IssueSource, e.IssueId }).HasName("jira_descriptions_pkey");

            entity.ToTable("jira_descriptions");

            entity.Property(e => e.IssueSource)
                .HasMaxLength(50)
                .HasColumnName("issue_source");
            entity.Property(e => e.IssueId)
                .HasMaxLength(50)
                .HasColumnName("issue_id");
            entity.Property(e => e.JiraDescription1).HasColumnName("jira_description");
        });

        modelBuilder.Entity<JiraIssue>(entity =>
        {
            entity.HasKey(e => new { e.IssueSource, e.IssueId }).HasName("jira_issues_pkey");

            entity.ToTable("jira_issues");

            entity.Property(e => e.IssueSource)
                .HasMaxLength(50)
                .HasColumnName("issue_source");
            entity.Property(e => e.IssueId)
                .HasMaxLength(50)
                .HasColumnName("issue_id");
            entity.Property(e => e.AssignedTo)
                .HasMaxLength(64)
                .HasColumnName("assigned_to");
            entity.Property(e => e.AssignedToOrganization)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_organization");
            entity.Property(e => e.AssignedToRole)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_role");
            entity.Property(e => e.ClosedBy)
                .HasMaxLength(64)
                .HasColumnName("closed_by");
            entity.Property(e => e.ClosedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("closed_by_organization");
            entity.Property(e => e.ClosedByRole)
                .HasMaxLength(64)
                .HasColumnName("closed_by_role");
            entity.Property(e => e.ClosedDatetime).HasColumnName("closed_datetime");
            entity.Property(e => e.ComputedSeverity)
                .HasMaxLength(64)
                .HasColumnName("computed_severity");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(64)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("created_by_organization");
            entity.Property(e => e.CreatedByRole)
                .HasMaxLength(64)
                .HasColumnName("created_by_role");
            entity.Property(e => e.CreatedDatetime).HasColumnName("created_datetime");
            entity.Property(e => e.FirstStateDatetime).HasColumnName("first_state_datetime");
            entity.Property(e => e.IssueAssignmentGroup)
                .HasMaxLength(64)
                .HasColumnName("issue_assignment_group");
            entity.Property(e => e.IssueLabels).HasColumnName("issue_labels");
            entity.Property(e => e.IssuePriority)
                .HasMaxLength(64)
                .HasColumnName("issue_priority");
            entity.Property(e => e.IssueProject)
                .HasMaxLength(64)
                .HasColumnName("issue_project");
            entity.Property(e => e.IssueSeverity)
                .HasMaxLength(64)
                .HasColumnName("issue_severity");
            entity.Property(e => e.IssueState)
                .HasMaxLength(64)
                .HasColumnName("issue_state");
            entity.Property(e => e.IssueTitle).HasColumnName("issue_title");
            entity.Property(e => e.IssueType)
                .HasMaxLength(50)
                .HasColumnName("issue_type");
            entity.Property(e => e.ResolutionDatetime).HasColumnName("resolution_datetime");
            entity.Property(e => e.ResolutionDescription).HasColumnName("resolution_description");
            entity.Property(e => e.ResolutionState)
                .HasMaxLength(64)
                .HasColumnName("resolution_state");
        });

        modelBuilder.Entity<JiraLabel>(entity =>
        {
            entity.HasKey(e => new { e.IssueSource, e.IssueId, e.JiraLabel1 }).HasName("jira_labels_pkey");

            entity.ToTable("jira_labels");

            entity.Property(e => e.IssueSource)
                .HasMaxLength(50)
                .HasColumnName("issue_source");
            entity.Property(e => e.IssueId)
                .HasMaxLength(50)
                .HasColumnName("issue_id");
            entity.Property(e => e.JiraLabel1)
                .HasMaxLength(2048)
                .HasColumnName("jira_label");
        });

        modelBuilder.Entity<JiraStateTransition>(entity =>
        {
            entity.HasKey(e => new { e.IssueSource, e.IssueId, e.TransitionSequence }).HasName("jira_state_transitions_pkey");

            entity.ToTable("jira_state_transitions");

            entity.Property(e => e.IssueSource)
                .HasMaxLength(50)
                .HasColumnName("issue_source");
            entity.Property(e => e.IssueId)
                .HasMaxLength(50)
                .HasColumnName("issue_id");
            entity.Property(e => e.TransitionSequence).HasColumnName("transition_sequence");
            entity.Property(e => e.StateFrom)
                .HasMaxLength(64)
                .HasColumnName("state_from");
            entity.Property(e => e.StateTo)
                .HasMaxLength(64)
                .HasColumnName("state_to");
            entity.Property(e => e.TransitionBy)
                .HasMaxLength(64)
                .HasColumnName("transition_by");
            entity.Property(e => e.TransitionDatetime).HasColumnName("transition_datetime");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("labels");

            entity.Property(e => e.Labels)
                .HasColumnType("character varying")
                .HasColumnName("labels");
            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .HasColumnName("ticket_id");
            entity.Property(e => e.TicketSource)
                .HasMaxLength(50)
                .HasColumnName("ticket_source");
        });

        modelBuilder.Entity<SnowAssignedTransition>(entity =>
        {
            entity.HasKey(e => new { e.Source, e.Id, e.TransitionSequence }).HasName("snow_changes_assigned_transitions_pkey");

            entity.ToTable("snow_assigned_transitions");

            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasColumnName("source");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.TransitionSequence).HasColumnName("transition_sequence");
            entity.Property(e => e.AssignedTo)
                .HasMaxLength(64)
                .HasColumnName("assigned_to");
            entity.Property(e => e.TransitionDatetime).HasColumnName("transition_datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(64)
                .HasColumnName("type");
        });

        modelBuilder.Entity<SnowChange>(entity =>
        {
            entity.HasKey(e => new { e.Source, e.Id }).HasName("snow_change_pkey");

            entity.ToTable("snow_changes");

            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasColumnName("source");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.AssignGroup)
                .HasMaxLength(64)
                .HasColumnName("assign_group");
            entity.Property(e => e.AssignedTo)
                .HasMaxLength(64)
                .HasColumnName("assigned_to");
            entity.Property(e => e.AssignedToOrganization)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_organization");
            entity.Property(e => e.AssignedToRole)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_role");
            entity.Property(e => e.Category)
                .HasMaxLength(64)
                .HasColumnName("category");
            entity.Property(e => e.ClosedBy)
                .HasMaxLength(64)
                .HasColumnName("closed_by");
            entity.Property(e => e.ClosedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("closed_by_organization");
            entity.Property(e => e.ClosedByRole)
                .HasMaxLength(64)
                .HasColumnName("closed_by_role");
            entity.Property(e => e.ClosedDatetime).HasColumnName("closed_datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(64)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("created_by_organization");
            entity.Property(e => e.CreatedByRole)
                .HasMaxLength(64)
                .HasColumnName("created_by_role");
            entity.Property(e => e.CreatedDatetime).HasColumnName("created_datetime");
            entity.Property(e => e.Domain)
                .HasMaxLength(64)
                .HasColumnName("domain");
            entity.Property(e => e.FirstStateDatetime).HasColumnName("first_state_datetime");
            entity.Property(e => e.Priority)
                .HasMaxLength(64)
                .HasColumnName("priority");
            entity.Property(e => e.Product)
                .HasMaxLength(256)
                .HasColumnName("product");
            entity.Property(e => e.Severity)
                .HasMaxLength(64)
                .HasColumnName("severity");
            entity.Property(e => e.State)
                .HasMaxLength(64)
                .HasColumnName("state");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Urgency)
                .HasMaxLength(64)
                .HasColumnName("urgency");
        });

        modelBuilder.Entity<SnowComment>(entity =>
        {
            entity.HasKey(e => new { e.Source, e.Id, e.TicketType, e.CommentType }).HasName("snow_comments_pkey");

            entity.ToTable("snow_comments");

            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasColumnName("source");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.TicketType)
                .HasMaxLength(64)
                .HasColumnName("ticket_type");
            entity.Property(e => e.CommentType).HasColumnName("comment_type");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CommentAuthor)
                .HasMaxLength(64)
                .HasColumnName("comment_author");
            entity.Property(e => e.CommentDatetime).HasColumnName("comment_datetime");
        });

        modelBuilder.Entity<SnowDescription>(entity =>
        {
            entity.HasKey(e => new { e.Source, e.Id, e.TicketType }).HasName("snow_desc_pkey");

            entity.ToTable("snow_descriptions");

            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasColumnName("source");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.TicketType)
                .HasMaxLength(64)
                .HasColumnName("ticket_type");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<SnowIncident>(entity =>
        {
            entity.HasKey(e => new { e.Source, e.Id }).HasName("snow_incident_pkey");

            entity.ToTable("snow_incidents");

            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasColumnName("source");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.AssignGroup)
                .HasMaxLength(64)
                .HasColumnName("assign_group");
            entity.Property(e => e.AssignedTo)
                .HasMaxLength(64)
                .HasColumnName("assigned_to");
            entity.Property(e => e.AssignedToOrganization)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_organization");
            entity.Property(e => e.AssignedToRole)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_role");
            entity.Property(e => e.Category)
                .HasMaxLength(64)
                .HasColumnName("category");
            entity.Property(e => e.CausalConfigurationItem)
                .HasMaxLength(64)
                .HasColumnName("causal_configuration_item");
            entity.Property(e => e.ClosedBy)
                .HasMaxLength(64)
                .HasColumnName("closed_by");
            entity.Property(e => e.ClosedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("closed_by_organization");
            entity.Property(e => e.ClosedByRole)
                .HasMaxLength(64)
                .HasColumnName("closed_by_role");
            entity.Property(e => e.ClosedDatetime).HasColumnName("closed_datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(64)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("created_by_organization");
            entity.Property(e => e.CreatedByRole)
                .HasMaxLength(64)
                .HasColumnName("created_by_role");
            entity.Property(e => e.CreatedDatetime).HasColumnName("created_datetime");
            entity.Property(e => e.Domain)
                .HasMaxLength(64)
                .HasColumnName("domain");
            entity.Property(e => e.FirstStateDatetime).HasColumnName("first_state_datetime");
            entity.Property(e => e.Location)
                .HasMaxLength(64)
                .HasColumnName("location");
            entity.Property(e => e.Priority)
                .HasMaxLength(64)
                .HasColumnName("priority");
            entity.Property(e => e.Product)
                .HasMaxLength(256)
                .HasColumnName("product");
            entity.Property(e => e.ResolutionDescription).HasColumnName("resolution_description");
            entity.Property(e => e.ResolutionState)
                .HasMaxLength(64)
                .HasColumnName("resolution_state");
            entity.Property(e => e.ResolvedDatetime).HasColumnName("resolved_datetime");
            entity.Property(e => e.Severity)
                .HasMaxLength(64)
                .HasColumnName("severity");
            entity.Property(e => e.State)
                .HasMaxLength(64)
                .HasColumnName("state");
            entity.Property(e => e.Subcategory)
                .HasMaxLength(128)
                .HasColumnName("subcategory");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Urgency)
                .HasMaxLength(64)
                .HasColumnName("urgency");
        });

        modelBuilder.Entity<SnowStateTransition>(entity =>
        {
            entity.HasKey(e => new { e.Source, e.Id, e.TransitionSequence }).HasName("snow_changes_state_transitions_pkey");

            entity.ToTable("snow_state_transitions");

            entity.Property(e => e.Source)
                .HasMaxLength(50)
                .HasColumnName("source");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.TransitionSequence).HasColumnName("transition_sequence");
            entity.Property(e => e.State)
                .HasMaxLength(64)
                .HasColumnName("state");
            entity.Property(e => e.TransitionDatetime).HasColumnName("transition_datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(64)
                .HasColumnName("type");
        });

        modelBuilder.Entity<StateTransition>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("state_transitions");

            entity.Property(e => e.StateFrom)
                .HasColumnType("character varying")
                .HasColumnName("state_from");
            entity.Property(e => e.StateTo)
                .HasMaxLength(64)
                .HasColumnName("state_to");
            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .HasColumnName("ticket_id");
            entity.Property(e => e.TicketSource)
                .HasMaxLength(50)
                .HasColumnName("ticket_source");
            entity.Property(e => e.TransitionDatetime).HasColumnName("transition_datetime");
            entity.Property(e => e.TransitionSequence).HasColumnName("transition_sequence");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("tickets");

            entity.Property(e => e.AssignedTo)
                .HasMaxLength(64)
                .HasColumnName("assigned_to");
            entity.Property(e => e.AssignedToOrganization)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_organization");
            entity.Property(e => e.AssignedToRole)
                .HasMaxLength(64)
                .HasColumnName("assigned_to_role");
            entity.Property(e => e.ClosedBy)
                .HasMaxLength(64)
                .HasColumnName("closed_by");
            entity.Property(e => e.ClosedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("closed_by_organization");
            entity.Property(e => e.ClosedByRole)
                .HasMaxLength(64)
                .HasColumnName("closed_by_role");
            entity.Property(e => e.ClosedDatetime).HasColumnName("closed_datetime");
            entity.Property(e => e.ComputedSeverity)
                .HasColumnType("character varying")
                .HasColumnName("computed_severity");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(64)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedByOrganization)
                .HasMaxLength(64)
                .HasColumnName("created_by_organization");
            entity.Property(e => e.CreatedByRole)
                .HasMaxLength(64)
                .HasColumnName("created_by_role");
            entity.Property(e => e.CreatedDatetime).HasColumnName("created_datetime");
            entity.Property(e => e.FirstStateDatetime).HasColumnName("first_state_datetime");
            entity.Property(e => e.Labels).HasColumnName("labels");
            entity.Property(e => e.Priority)
                .HasMaxLength(64)
                .HasColumnName("priority");
            entity.Property(e => e.ResolutionDatetime).HasColumnName("resolution_datetime");
            entity.Property(e => e.ResolutionDescription).HasColumnName("resolution_description");
            entity.Property(e => e.ResolutionState)
                .HasColumnType("character varying")
                .HasColumnName("resolution_state");
            entity.Property(e => e.Severity)
                .HasMaxLength(64)
                .HasColumnName("severity");
            entity.Property(e => e.TicketAssignmentGroup)
                .HasMaxLength(64)
                .HasColumnName("ticket_assignment_group");
            entity.Property(e => e.TicketDescription).HasColumnName("ticket_description");
            entity.Property(e => e.TicketId)
                .HasMaxLength(50)
                .HasColumnName("ticket_id");
            entity.Property(e => e.TicketProject)
                .HasColumnType("character varying")
                .HasColumnName("ticket_project");
            entity.Property(e => e.TicketSource)
                .HasMaxLength(50)
                .HasColumnName("ticket_source");
            entity.Property(e => e.TicketState)
                .HasMaxLength(64)
                .HasColumnName("ticket_state");
            entity.Property(e => e.TicketType)
                .HasColumnType("character varying")
                .HasColumnName("ticket_type");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Urgency)
                .HasColumnType("character varying")
                .HasColumnName("urgency");
        });

        modelBuilder.Entity<TimeD>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("time_d");

            entity.Property(e => e.CalMnth)
                .HasMaxLength(20)
                .HasColumnName("cal_mnth");
            entity.Property(e => e.CalMnthNum)
                .HasMaxLength(20)
                .HasColumnName("cal_mnth_num");
            entity.Property(e => e.CalQtr)
                .HasMaxLength(20)
                .HasColumnName("cal_qtr");
            entity.Property(e => e.CalYr)
                .HasMaxLength(20)
                .HasColumnName("cal_yr");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.FiscMnthNum)
                .HasMaxLength(20)
                .HasColumnName("fisc_mnth_num");
            entity.Property(e => e.FiscQtr)
                .HasMaxLength(20)
                .HasColumnName("fisc_qtr");
            entity.Property(e => e.FiscYr)
                .HasMaxLength(20)
                .HasColumnName("fisc_yr");
            entity.Property(e => e.IrcQtr)
                .HasMaxLength(20)
                .HasColumnName("irc_qtr");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
