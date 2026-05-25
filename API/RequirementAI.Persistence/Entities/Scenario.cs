
namespace RequirementAI.Persistence.Entities;

public class Scenario: BaseEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public IList<UserStory> UserStories { get; set; } = new List<UserStory>();
    public Guid PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;
}