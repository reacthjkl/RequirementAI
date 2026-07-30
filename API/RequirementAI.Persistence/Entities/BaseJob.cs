using RequirementAI.Contract.Enums;

namespace RequirementAI.Persistence.Entities;

public abstract class BaseJob : BaseEntity
{
    public Guid ProjectId { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Pending;
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    public string? FinishedBy { get; set; }
    public int TryCount { get; set; }
}
