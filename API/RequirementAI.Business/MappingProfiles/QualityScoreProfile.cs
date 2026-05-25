using AutoMapper;
using RequirementAI.Contract.Dto;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.MappingProfiles;

public class QualityScoreProfile: Profile
{
    public QualityScoreProfile()
    {
        CreateMap<PersonaQualityScore, PersonaQualityScoreDto>();
        CreateMap<PersonaQualityScore, PersonaLlmAnalysisDto>().ReverseMap();
        
        CreateMap<ScenarioQualityScore, ScenarioQualityScoreDto>();
        CreateMap<ScenarioQualityScore, ScenarioLlmAnalysisDto>().ReverseMap();
        
        CreateMap<UserStoryQualityScore, UserStoryQualityScoreDto>();
        CreateMap<UserStoryQualityScore, UserStoryLlmAnalysisDto>().ReverseMap();
        
    }
}