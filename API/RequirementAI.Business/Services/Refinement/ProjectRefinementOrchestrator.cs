using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Enums;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.Refinement;

public class ProjectRefinementOrchestrator(IProjectRepository projectRepository, IRefinementService refinementService): IProjectRefinementOrchestrator
{
    public async Task Execute(ProjectRefinementJob job, CancellationToken ct)
    {
        var project = await projectRepository.GetFullProjectById(job.ProjectId, ct);
        
        foreach (var persona in project.Personas)
        {
            await refinementService.RefinePersona(persona, job.CustomInstructions, ct);

            foreach (var scenario in persona.Scenarios)
            {
                await refinementService.RefineScenario(scenario, job.CustomInstructions, ct);

                foreach (var userStory in scenario.UserStories)
                {
                    await refinementService.RefineUserStory(userStory, job.CustomInstructions, ct);
                }
            }
        }

        await projectRepository.Update(project, ct);
    }
}