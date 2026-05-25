using AutoMapper;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.QualityAnalysis;

public class UserStoryAnalysisMerger(IMapper mapper): IAnalysisMerger<UserStory, UserStoryLlmAnalysisDto>
{
    public void Apply(UserStory entity, UserStoryLlmAnalysisDto dto)
    {
        var scoreEntity = mapper.Map<UserStoryLlmAnalysisDto, UserStoryQualityScore>(dto);
        
        entity.QualityScores.Add(scoreEntity);  
    }
}