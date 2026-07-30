using RequirementAI.Contract.Enums;

namespace RequirementAI.Contract.Dto;

public abstract class BaseJobDto
{
    public JobStatus Status { get; set; } 
    public string? ErrorMessage { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }
    public string? FinishedBy { get; set; }
}
