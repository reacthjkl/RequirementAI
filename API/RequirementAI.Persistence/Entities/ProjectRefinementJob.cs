
namespace RequirementAI.Persistence.Entities;

public class ProjectRefinementJob: BaseJob
{
    public Guid ProjectId { get; set; }
    public string? CustomInstructions { get; set; }
}