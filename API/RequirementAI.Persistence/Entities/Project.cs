using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Entities;
public class Project: BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public IList<Persona> Personas { get; set; } = new List<Persona>();
    public ProjectStatus Status { get; set; } = ProjectStatus.Incomplete;
}