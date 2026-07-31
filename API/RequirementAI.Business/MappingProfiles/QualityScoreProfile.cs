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
        CreateMap<PersonaQualityScore, PersonaLlmAnalysisDto>();
        CreateMap<PersonaLlmAnalysisDto, PersonaQualityScore>()
            .ForMember(dest => dest.OverallScore, opt => opt.MapFrom(src =>
                Math.Round(
                    (
                        src.ClarityScore +
                        src.RealismScore +
                        src.GoalClarityScore +
                        src.PainPointsScore +
                        src.RelevanceScore +
                        src.DifferentiationScore
                    ) / 6m,
                    2,
                    MidpointRounding.AwayFromZero)));
        
        CreateMap<ScenarioQualityScore, ScenarioQualityScoreDto>();
        CreateMap<ScenarioQualityScore, ScenarioLlmAnalysisDto>();
        CreateMap<ScenarioLlmAnalysisDto, ScenarioQualityScore>()
            .ForMember(dest => dest.OverallScore, opt => opt.MapFrom(src =>
                Math.Round(
                    (
                        src.ClarityScore +
                        src.ContextScore +
                        src.TriggerScore +
                        src.FlowCompletenessScore +
                        src.EdgeCasesScore +
                        src.PersonaFitScore
                    ) / 6m,
                    2,
                    MidpointRounding.AwayFromZero)));
        
        CreateMap<UserStoryQualityScore, UserStoryQualityScoreDto>() ;
        CreateMap<UserStoryQualityScore, UserStoryLlmAnalysisDto>();
        CreateMap<UserStoryLlmAnalysisDto, UserStoryQualityScore>()
            .ForMember(dest => dest.OverallScore, opt => opt.MapFrom(src =>
                Math.Round(
                    (
                        src.ClarityScore +
                        src.CompletenessScore +
                        src.TestabilityScore +
                        src.AcceptanceCriteriaScore +
                        src.ScopeScore +
                        src.BusinessValueScore +
                        src.AmbiguityScore
                    ) / 7m,
                    2,
                    MidpointRounding.AwayFromZero)));
    }
}
