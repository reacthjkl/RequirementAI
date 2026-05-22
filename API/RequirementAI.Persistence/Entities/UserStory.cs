using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Persistence.Entities;

public class UserStory: ICreatable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public IList<AcceptanceCriteria> AcceptanceCriteria { get; set; } =  new List<AcceptanceCriteria>();
    public IList<EdgeCase> EdgeCases { get; set; } =  new List<EdgeCase>();
    public Guid ScenarioId { get; set; }
    public Scenario Scenario { get; set; } = null!;
    public UserStoryStage Stage { get; set; } = UserStoryStage.New;
}