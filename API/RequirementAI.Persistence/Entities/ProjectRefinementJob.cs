using RequirementAI.Contract.Enums;

namespace RequirementAI.Persistence.Entities;

public class ProjectRefinementJob
{
    public Guid Id  { get; set; }
    public Guid ProjectId { get; set; }
    public string? CustomInstructions { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Pending;
    public string? ErrorMessage { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
}