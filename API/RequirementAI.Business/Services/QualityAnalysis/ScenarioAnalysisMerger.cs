using AutoMapper;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class ScenarioAnalysisMerger(IMapper mapper): IAnalysisMerger<Scenario, ScenarioLlmAnalysisDto>
{
    public void Apply(Scenario entity, ScenarioLlmAnalysisDto dto)
    {
        var scoreEntity = mapper.Map<ScenarioLlmAnalysisDto, ScenarioQualityScore>(dto);
        
        entity.QualityScores.Add(scoreEntity);      
    }
}