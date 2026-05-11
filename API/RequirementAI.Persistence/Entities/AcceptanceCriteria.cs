using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Entities;

public class AcceptanceCriteria: ICreatable
{
    public Guid Id { get; set; }
    public string Wording { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid UserStoryId { get; set; }
    public UserStory UserStory { get; set; } = null!;
}