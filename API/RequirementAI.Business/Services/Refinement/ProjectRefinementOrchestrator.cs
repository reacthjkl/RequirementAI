using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Refinement;

public class ProjectRefinementOrchestrator(
    IProjectRepository projectRepository,
    IRefinementService refinementService,
    IUserStorySplitService userStorySplitService)
    : IProjectRefinementOrchestrator
{
    public async Task Execute(ProjectRefinementJob job, CancellationToken ct)
    {
        var project = await projectRepository.GetFullProjectById(job.ProjectId, ct);
        var eligibleUserStories = project.Personas
            .SelectMany(persona => persona.Scenarios)
            .SelectMany(scenario => scenario.UserStories)
            .Where(userStory => userStory.Stage == UserStoryStage.New)
            .ToList();
        var eligibleUserStorySet = eligibleUserStories.ToHashSet();

        foreach (var persona in project.Personas)
        {
            await refinementService.RefinePersona(persona, job.CustomInstructions, ct);

            foreach (var scenario in persona.Scenarios)
            {
                await refinementService.RefineScenario(scenario, job.CustomInstructions, ct);

                foreach (var userStory in scenario.UserStories.Where(eligibleUserStorySet.Contains))
                    await refinementService.RefineUserStory(userStory, job.CustomInstructions, ct);
            }
        }

        foreach (var userStory in eligibleUserStories)
        {
            var splitStories = await userStorySplitService.SplitUserStory(userStory, job.CustomInstructions, ct);
            foreach (var newStory in splitStories.Skip(1))
                userStory.Scenario.UserStories.Add(newStory);
        }

        await projectRepository.Update(project, ct);
    }
}
