namespace RequirementAI.Contract.Dto;

public class UserStoryForCreationDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid ScenarioId { get; set; }
}