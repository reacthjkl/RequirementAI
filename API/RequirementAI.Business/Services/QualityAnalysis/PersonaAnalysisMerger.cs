using AutoMapper;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class PersonaAnalysisMerger(IMapper mapper): IAnalysisMerger<Persona, PersonaLlmAnalysisDto>
{
    public void Apply(Persona entity, PersonaLlmAnalysisDto dto)
    {
        var scoreEntity = mapper.Map<PersonaLlmAnalysisDto, PersonaQualityScore>(dto);
        
        entity.QualityScores.Add(scoreEntity);        
    }
}