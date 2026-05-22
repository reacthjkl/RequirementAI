using RequirementAI.Contract.Enums;

namespace RequirementAI.Contract.Dto;

public class ProjectResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public ProjectStatus Status { get; set; }
    public RefinementStatus RefinementStatus { get; set; }
}