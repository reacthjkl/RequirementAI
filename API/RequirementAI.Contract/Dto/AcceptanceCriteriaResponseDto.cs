namespace RequirementAI.Contract.Dto;

public class AcceptanceCriteriaResponseDto
{
    public Guid Id { get; set; }
    public string Wording { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid UserStoryId { get; set; }
}