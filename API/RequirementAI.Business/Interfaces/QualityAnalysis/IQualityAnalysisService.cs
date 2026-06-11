using RequirementAI.Contract.Dto;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Interfaces.QualityAnalysis;

public interface IQualityAnalysisService
{
    Task<Persona> AnalyzePersona(Persona persona, CancellationToken ct);
    Task<Scenario> AnalyzeScenario(Scenario scenario, CancellationToken ct);
    Task<UserStory> AnalyzeUserStory(UserStory userStory, CancellationToken ct);
}