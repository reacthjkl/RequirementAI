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
                    src.ClarityScore * 0.20m +
                    src.RealismScore * 0.15m +
                    src.GoalClarityScore * 0.25m +
                    src.PainPointsScore * 0.15m +
                    src.RelevanceScore * 0.20m +
                    src.DifferentiationScore * 0.05m,
                    1)));
        
        CreateMap<ScenarioQualityScore, ScenarioQualityScoreDto>();
        CreateMap<ScenarioQualityScore, ScenarioLlmAnalysisDto>();
        CreateMap<ScenarioLlmAnalysisDto, ScenarioQualityScore>()
            .ForMember(dest => dest.OverallScore, opt => opt.MapFrom(src =>
                Math.Round(
                    src.ClarityScore * 0.15m +
                    src.ContextScore * 0.20m +
                    src.TriggerScore * 0.15m +
                    src.FlowCompletenessScore * 0.25m +
                    src.EdgeCasesScore * 0.15m +
                    src.PersonaFitScore * 0.10m,
                    1)));
        
        CreateMap<UserStoryQualityScore, UserStoryQualityScoreDto>() ;
        CreateMap<UserStoryQualityScore, UserStoryLlmAnalysisDto>();
        CreateMap<UserStoryLlmAnalysisDto, UserStoryQualityScore>().ForMember(dest => dest.OverallScore, opt => opt.MapFrom(src =>
            Math.Round(
                src.ClarityScore * 0.15m +
                src.CompletenessScore * 0.20m +
                src.TestabilityScore * 0.25m +
                src.AcceptanceCriteriaScore * 0.20m +
                src.ScopeScore * 0.10m +
                src.BusinessValueScore * 0.07m +
                src.AmbiguityScore * 0.03m,
                1)));
    }
}