using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Refinement;

public class ProjectRefinementOrchestrator(
    IProjectRepository projectRepository,
    IRefinementService refinementService,
    IUserStorySplitService userStorySplitService,
    IProjectAnalysisFreshnessService analysisFreshnessService)
    : IProjectRefinementOrchestrator
{
    public async Task Execute(ProjectRefinementJob job, CancellationToken ct)
    {
        await analysisFreshnessService.EnsureRecentAnalysis(job.ProjectId, ct);

        var project = await projectRepository.GetFullProjectById(job.ProjectId, ct);

        var eligibleUserStories = GetEligibleUserStories(project);
        var eligibleUserStorySet = eligibleUserStories.ToHashSet();

        foreach (var persona in project.Personas)
            await RefinePersona(persona, eligibleUserStorySet, job.CustomInstructions, ct);

        await SplitUserStories(eligibleUserStories, job.CustomInstructions, ct);

        await projectRepository.Update(project, ct);
    }

    private static List<UserStory> GetEligibleUserStories(Project project)
    {
        return project.Personas
            .SelectMany(persona => persona.Scenarios)
            .SelectMany(scenario => scenario.UserStories)
            .Where(userStory => userStory.Stage == UserStoryStage.New)
            .ToList();
    }

    private async Task RefinePersona(
        Persona persona,
        HashSet<UserStory> eligibleUserStories,
        string? customInstructions,
        CancellationToken ct)
    {
        await refinementService.RefinePersona(persona, customInstructions, ct);

        foreach (var scenario in persona.Scenarios)
            await RefineScenario(scenario, eligibleUserStories, customInstructions, ct);
    }

    private async Task RefineScenario(
        Scenario scenario,
        HashSet<UserStory> eligibleUserStories,
        string? customInstructions,
        CancellationToken ct)
    {
        await refinementService.RefineScenario(scenario, customInstructions, ct);

        foreach (var userStory in scenario.UserStories.Where(eligibleUserStories.Contains))
            await refinementService.RefineUserStory(userStory, customInstructions, ct);
    }

    private async Task SplitUserStories(
        IEnumerable<UserStory> userStories,
        string? customInstructions,
        CancellationToken ct)
    {
        foreach (var userStory in userStories)
        {
            var splitStories = await userStorySplitService.SplitUserStory(userStory, customInstructions, ct);
            foreach (var newStory in splitStories.Skip(1))
                userStory.Scenario.UserStories.Add(newStory);
        }
    }
}