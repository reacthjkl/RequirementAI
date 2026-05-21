namespace RequirementAI.Contract.Dto;

public class EdgeCaseResponseDto
{
    public Guid Id { get; set; }
    public string Preconditions { get; set; } = null!;
    public string TriggerAction { get; set; } = null!;
    public string ExpectedBehavior { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid UserStoryId { get; set; }
}