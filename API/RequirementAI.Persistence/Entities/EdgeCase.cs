
namespace RequirementAI.Persistence.Entities;

public class EdgeCase: BaseEntity
{
    public string Preconditions { get; set; } = null!;
    public string TriggerAction { get; set; } = null!;
    public string ExpectedBehavior { get; set; } = null!;
    public Guid UserStoryId { get; set; }
    public UserStory UserStory { get; set; } = null!;
} 