using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Entities;

public class EdgeCase: ICreatable
{
    public Guid Id { get; set; }
    public string Preconditions { get; set; } = null!;
    public string TriggerAction { get; set; } = null!;
    public string ExpectedBehavior { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid UserStoryId { get; set; }
    public UserStory UserStory { get; set; } = null!;
} 