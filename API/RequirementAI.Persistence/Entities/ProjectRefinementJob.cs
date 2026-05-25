using RequirementAI.Contract.Enums;

namespace RequirementAI.Persistence.Entities;

public class ProjectRefinementJob: BaseEntity
{
    public Guid ProjectId { get; set; }
    public string? CustomInstructions { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Pending;
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
}