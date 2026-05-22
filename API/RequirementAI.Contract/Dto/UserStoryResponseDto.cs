using RequirementAI.Contract.Enums;

namespace RequirementAI.Contract.Dto;

public class UserStoryResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid ScenarioId { get; set; }
    public UserStoryStage Stage { get; set; }
}