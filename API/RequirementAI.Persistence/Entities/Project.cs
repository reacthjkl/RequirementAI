using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Entities;



public class Project: ICreatable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public IList<Persona> Personas { get; set; } = new List<Persona>();
    public IList<UserStory> UserStories { get; set; } = new List<UserStory>();
}