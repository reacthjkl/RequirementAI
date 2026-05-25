
namespace RequirementAI.Persistence.Entities;

public class Persona: BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ContextOfUse { get; set; } = null!;
    public string Goals { get; set; } = null!;
    public string Frustrations { get; set; } = null!;
    public IList<Scenario> Scenarios { get; set; } = new List<Scenario>();
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public List<PersonaQualityScore> QualityScores { get; set; } = [];
}