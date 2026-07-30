using RequirementAI.Contract.Enums;

namespace RequirementAI.Contract.Dto;

public class ProjectWithArtifactsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<ProjectArtifactPersonaDto> Personas { get; set; } = [];
}

public class ProjectArtifactPersonaDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ContextOfUse { get; set; } = null!;
    public string Goals { get; set; } = null!;
    public string Frustrations { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public PersonaQualityScoreDto? LatestEvaluation { get; set; }
    public List<ProjectArtifactScenarioDto> Scenarios { get; set; } = [];
}

public class ProjectArtifactScenarioDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public ScenarioQualityScoreDto? LatestEvaluation { get; set; }
    public List<ProjectArtifactUserStoryDto> UserStories { get; set; } = [];
}

public class ProjectArtifactUserStoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public UserStoryStage Stage { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public UserStoryQualityScoreDto? LatestEvaluation { get; set; }
    public List<ProjectArtifactAcceptanceCriteriaDto> AcceptanceCriteria { get; set; } = [];
    public List<ProjectArtifactEdgeCaseDto> EdgeCases { get; set; } = [];
}

public class ProjectArtifactAcceptanceCriteriaDto
{
    public Guid Id { get; set; }
    public string Wording { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ProjectArtifactEdgeCaseDto
{
    public Guid Id { get; set; }
    public string Preconditions { get; set; } = null!;
    public string TriggerAction { get; set; } = null!;
    public string ExpectedBehavior { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
