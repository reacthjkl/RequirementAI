namespace RequirementAI.Contract.Dto;

public class LowestScoreItemDto
{
    public Guid ItemId { get; set; }

    public string ItemType { get; set; } = null!; // "Persona", "Scenario", "UserStory"

    public string Title { get; set; } = null!;

    public int Score { get; set; }

    public DateTimeOffset EvaluatedAt { get; set; }
}