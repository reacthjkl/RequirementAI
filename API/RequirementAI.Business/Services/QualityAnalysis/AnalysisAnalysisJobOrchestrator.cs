using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Persistence.Entities;
using RequirementAI.Persistence.Interfaces;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class AnalysisAnalysisJobOrchestrator(
    IProjectRepository projectRepository,
    IQualityAnalysisService analysisService) : IAnalysisJobOrchestrator
{
    public async Task Execute(QualityAnalysisJob job, CancellationToken ct)
    {
        var project = await projectRepository.GetFullProjectById(job.ProjectId, ct);

        foreach (var persona in project.Personas)
        {
            await analysisService.AnalyzePersona(persona, ct);

            foreach (var scenario in persona.Scenarios)
            {
                await analysisService.AnalyzeScenario(scenario, ct);

                foreach (var userStory in scenario.UserStories) await analysisService.AnalyzeUserStory(userStory, ct);
            }
        }

        await projectRepository.Update(project, ct);
    }
}