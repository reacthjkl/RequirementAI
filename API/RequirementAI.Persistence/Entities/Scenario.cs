using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Entities;

public class Scenario: ICreatable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public IList<UserStory> UserStories { get; set; } = new List<UserStory>();
    public Guid PersonaId { get; set; }
    public Persona Persona { get; set; } = null!;
}