
namespace RequirementAI.Persistence.Entities;

public class AcceptanceCriteria: BaseEntity
{
    public string Wording { get; set; } = null!;
    public Guid UserStoryId { get; set; }
    public UserStory UserStory { get; set; } = null!;
}