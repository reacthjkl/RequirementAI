using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Entities;

public class Organization: ICreatable
{
    public Guid Id  { get; set; }
    public string Name { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public IList<User> Users { get; set; } = new List<User>();
    public IList<Project> Projects { get; set; } = new List<Project>();
}