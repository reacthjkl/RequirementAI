
namespace RequirementAI.Persistence.Entities;

public class Organization: BaseEntity
{
    public string Name { get; set; } = null!;
    public IList<User> Users { get; set; } = new List<User>();
    public IList<Project> Projects { get; set; } = new List<Project>();
}