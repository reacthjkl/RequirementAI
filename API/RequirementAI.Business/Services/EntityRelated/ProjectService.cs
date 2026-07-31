using AutoMapper;
using System.Text.RegularExpressions;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.EntityRelated;

public class ProjectService(
    IProjectRepository projectRepository,
    IJobRepository<ProjectRefinementJob> projectRefinementJobRepository,
    IJobRepository<QualityAnalysisJob> qualityAnalysisJobRepository,
    IProjectStatusEnricher projectStatusEnricher,
    IMapper mapper)
    : IProjectService
{
    private static readonly Regex WordRegex = new(@"[\p{L}\p{N}]+", RegexOptions.Compiled);

    public async Task<ProjectResponseDto> GetById(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(id, organizationId, ct);

        var dto = mapper.Map<ProjectResponseDto>(entity);

        await projectStatusEnricher.EnrichAsync(dto, ct);

        return dto;
    }

    public async Task<ProjectWithArtifactsDto> GetWithArtifacts(Guid id, Guid organizationId, CancellationToken ct)
    {
        var project = await projectRepository.GetFullProjectById(id, organizationId, ct);

        return new ProjectWithArtifactsDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Personas = project.Personas
                .OrderBy(x => x.CreatedAt)
                .ThenBy(x => x.Id)
                .Select(persona => new ProjectArtifactPersonaDto
                {
                    Id = persona.Id,
                    Name = persona.Name,
                    Description = persona.Description,
                    ContextOfUse = persona.ContextOfUse,
                    Goals = persona.Goals,
                    Frustrations = persona.Frustrations,
                    CreatedAt = persona.CreatedAt,
                    UpdatedAt = persona.UpdatedAt,
                    LatestEvaluation = mapper.Map<PersonaQualityScoreDto?>(
                        persona.QualityScores.FirstOrDefault()),
                    Scenarios = persona.Scenarios
                        .OrderBy(x => x.CreatedAt)
                        .ThenBy(x => x.Id)
                        .Select(scenario => new ProjectArtifactScenarioDto
                        {
                            Id = scenario.Id,
                            Title = scenario.Title,
                            Content = scenario.Content,
                            CreatedAt = scenario.CreatedAt,
                            UpdatedAt = scenario.UpdatedAt,
                            LatestEvaluation = mapper.Map<ScenarioQualityScoreDto?>(
                                scenario.QualityScores.FirstOrDefault()),
                            UserStories = scenario.UserStories
                                .OrderBy(x => x.CreatedAt)
                                .ThenBy(x => x.Id)
                                .Select(userStory => new ProjectArtifactUserStoryDto
                                {
                                    Id = userStory.Id,
                                    Title = userStory.Title,
                                    Description = userStory.Description,
                                    Stage = userStory.Stage,
                                    CreatedAt = userStory.CreatedAt,
                                    UpdatedAt = userStory.UpdatedAt,
                                    LatestEvaluation = mapper.Map<UserStoryQualityScoreDto?>(
                                        userStory.QualityScores.FirstOrDefault()),
                                    AcceptanceCriteria = userStory.AcceptanceCriteria
                                        .OrderBy(x => x.CreatedAt)
                                        .ThenBy(x => x.Id)
                                        .Select(criterion => new ProjectArtifactAcceptanceCriteriaDto
                                        {
                                            Id = criterion.Id,
                                            Wording = criterion.Wording,
                                            CreatedAt = criterion.CreatedAt,
                                            UpdatedAt = criterion.UpdatedAt
                                        })
                                        .ToList(),
                                    EdgeCases = userStory.EdgeCases
                                        .OrderBy(x => x.CreatedAt)
                                        .ThenBy(x => x.Id)
                                        .Select(edgeCase => new ProjectArtifactEdgeCaseDto
                                        {
                                            Id = edgeCase.Id,
                                            Preconditions = edgeCase.Preconditions,
                                            TriggerAction = edgeCase.TriggerAction,
                                            ExpectedBehavior = edgeCase.ExpectedBehavior,
                                            CreatedAt = edgeCase.CreatedAt,
                                            UpdatedAt = edgeCase.UpdatedAt
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList()
        };
    }

    public async Task<ProjectWordCountDto> GetWordCounts(Guid id, Guid organizationId, CancellationToken ct)
    {
        var project = await projectRepository.GetFullProjectById(id, organizationId, ct);

        var personaCounts = project.Personas
            .OrderBy(x => x.CreatedAt)
            .Select(persona => new ArtifactWordCountDto
            {
                Id = persona.Id,
                Title = persona.Name,
                Words = CountWords(
                    persona.Name,
                    persona.Description,
                    persona.ContextOfUse,
                    persona.Goals,
                    persona.Frustrations)
            })
            .ToList();

        var scenarioCounts = project.Personas
            .SelectMany(persona => persona.Scenarios)
            .OrderBy(x => x.CreatedAt)
            .Select(scenario => new ArtifactWordCountDto
            {
                Id = scenario.Id,
                Title = scenario.Title,
                Words = CountWords(scenario.Title, scenario.Content)
            })
            .ToList();

        var userStories = project.Personas
            .SelectMany(persona => persona.Scenarios)
            .SelectMany(scenario => scenario.UserStories)
            .ToList();

        var userStoryCounts = userStories
            .OrderBy(x => x.CreatedAt)
            .Select(userStory => new ArtifactWordCountDto
            {
                Id = userStory.Id,
                Title = userStory.Title,
                Words = CountWords(userStory.Description)
            })
            .ToList();

        var projectWords = CountWords(project.Name, project.Description);
        var userStoryTotalWords = userStories.Sum(userStory => CountWords(
            userStory.Title,
            userStory.Description,
            string.Join(" ", userStory.AcceptanceCriteria.Select(x => x.Wording)),
            string.Join(" ", userStory.EdgeCases.Select(x =>
                JoinText(x.Preconditions, x.TriggerAction, x.ExpectedBehavior)))));
        var totalWords = projectWords
                         + personaCounts.Sum(x => x.Words)
                         + scenarioCounts.Sum(x => x.Words)
                         + userStoryTotalWords;

        return new ProjectWordCountDto
        {
            ProjectId = project.Id,
            ProjectName = project.Name,
            TotalWords = totalWords,
            AverageWordsPerPersona = AverageWords(personaCounts),
            AverageWordsPerScenario = AverageWords(scenarioCounts),
            AverageWordsPerUserStory = AverageWords(userStoryCounts),
            WordsPerPersona = personaCounts,
            WordsPerScenario = scenarioCounts,
            WordsPerUserStory = userStoryCounts
        };
    }

    public async Task<ProjectUserStoryDetailCountDto> GetUserStoryDetailCounts(
        Guid id,
        Guid organizationId,
        CancellationToken ct)
    {
        var counts = await projectRepository.GetUserStoryDetailCounts(id, organizationId, ct);

        return new ProjectUserStoryDetailCountDto
        {
            ProjectId = counts.ProjectId,
            UserStoryCount = counts.UserStoryCount,
            TotalAcceptanceCriteria = counts.TotalAcceptanceCriteria,
            TotalEdgeCases = counts.TotalEdgeCases,
            AverageAcceptanceCriteriaPerUserStory = AveragePerUserStory(
                counts.TotalAcceptanceCriteria,
                counts.UserStoryCount),
            AverageEdgeCasesPerUserStory = AveragePerUserStory(
                counts.TotalEdgeCases,
                counts.UserStoryCount)
        };
    }

    public async Task<List<ProjectResponseDto>> GetByOrganizationId(Guid organizationId, CancellationToken ct)
    {
        var entities = await projectRepository.GetByOrganization(organizationId, ct);

        var dtos = mapper.Map<List<ProjectResponseDto>>(entities);

        await projectStatusEnricher.EnrichRangeAsync(dtos, ct);

        return dtos;
    }

    public async Task<ProjectResponseDto> Create(ProjectForCreationDto project, Guid organizationId,
        CancellationToken ct)
    {
        var entity = mapper.Map<Project>(project);

        entity.OrganizationId = organizationId;

        var created = await projectRepository.Create(entity, ct);

        return mapper.Map<ProjectResponseDto>(created);
    }

    public async Task Update(ProjectForUpdateDto project, Guid organizationId, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(project.Id, organizationId, ct);

        mapper.Map(project, entity);

        await projectRepository.Update(entity, ct);
    }

    public async Task<Guid> Refine(Guid projectId, Guid organizationId, RefineRequestDto request, CancellationToken ct)
    {
        await projectRepository.GetById(projectId, organizationId, ct);

        var job = await projectRefinementJobRepository.Create(
            new ProjectRefinementJob
            {
                ProjectId = projectId,
                CustomInstructions = request.customInstructions
            },
            ct);

        return job.Id;
    }

    public async Task<Guid> Analyze(Guid projectId, Guid organizationId, CancellationToken ct)
    {
        await projectRepository.GetById(projectId, organizationId, ct);

        var job = await qualityAnalysisJobRepository.Create(
            new QualityAnalysisJob
            {
                ProjectId = projectId
            },
            ct);

        return job.Id;
    }

    public async Task Delete(Guid id, Guid organizationId, CancellationToken ct)
    {
        var entity = await projectRepository.GetById(id, organizationId, ct);
        await projectRepository.Delete(entity, ct);
    }

    private static int CountWords(params string?[] values)
    {
        return WordRegex.Matches(JoinText(values)).Count;
    }

    private static string JoinText(params string?[] values)
    {
        return string.Join(" ", values.Where(value => !string.IsNullOrWhiteSpace(value)));
    }

    private static decimal AverageWords(IReadOnlyCollection<ArtifactWordCountDto> counts)
    {
        return counts.Count == 0
            ? 0
            : Math.Round((decimal)counts.Average(x => x.Words), 2);
    }

    private static decimal AveragePerUserStory(int total, int userStoryCount)
    {
        return userStoryCount == 0
            ? 0.00m
            : Math.Round(
                (decimal)total / userStoryCount,
                2,
                MidpointRounding.AwayFromZero);
    }
}
