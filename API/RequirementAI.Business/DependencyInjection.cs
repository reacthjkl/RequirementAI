using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Business.Helpers;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Business.Interfaces.QualityAnalysis;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Business.Providers;
using RequirementAI.Business.Providers.Auth;
using RequirementAI.Business.Providers.LLM;
using RequirementAI.Business.Services;
using RequirementAI.Business.Services.Auth;
using RequirementAI.Business.Services.EntityRelated;
using RequirementAI.Business.Services.QualityAnalysis;
using RequirementAI.Business.Services.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Validators.LLMResponseValidators.Analysis;
using RequirementAI.Contract.Validators.LLMResponseValidators.Refinement;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        // entity related services
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProjectStatusEnricher, ProjectStatusEnricher>();
        services.AddScoped<IPersonaService, PersonaService>();
        services.AddScoped<IScenarioService, ScenarioService>();
        services.AddScoped<IUserStoryService, UserStoryService>();
        services.AddScoped<IEdgeCaseService, EdgeCaseService>();
        services.AddScoped<IAcceptanceCriteriaService, AcceptanceCriteriaService>();
        services.AddScoped<IQualityScoreService, QualityScoreService>();
        services.AddScoped<IQualityAnalysisJobService, QualityAnalysisJobService>();

        // refinement services
        services.AddScoped<IRefinementService, RefinementService>();
        services.AddScoped<IRefinementTaskProvider, RefinementTaskProvider>();
        services.AddScoped<IRefinementMerger<Persona, PersonaForLLMDto>, PersonaRefinementMerger>();
        services.AddScoped<IRefinementMerger<Scenario, ScenarioForLLMDto>, ScenarioRefinementMerger>();
        services.AddScoped<IRefinementMerger<UserStory, UserStoryForLLMDto>, UserStoryRefinementMerger>();
        services.AddSingleton<IUserStoryLanguageValidator, UserStoryLanguageValidator>();
        services.AddScoped<IUserStorySplitService, UserStorySplitService>();
        services.AddScoped<IProjectAnalysisFreshnessService, ProjectAnalysisFreshnessService>();
        services.AddScoped<IProjectRefinementOrchestrator, ProjectRefinementOrchestrator>();
        services.AddScoped<IProjectRefinementJobProcessor, ProjectRefinementJobProcessor>();

        // analysis services
        services.AddScoped<IAnalysisJobProcessor, AnalysisJobProcessor>();
        services.AddScoped<IAnalysisJobOrchestrator, AnalysisAnalysisJobOrchestrator>();
        services.AddScoped<IQualityAnalysisService, QualityAnalysisService>();
        services.AddScoped<IAnalysisTaskProvider, AnalysisTaskProvider>();
        services.AddScoped<IAnalysisMerger<Persona, PersonaLlmAnalysisDto>, PersonaAnalysisMerger>();
        services.AddScoped<IAnalysisMerger<Scenario, ScenarioLlmAnalysisDto>, ScenarioAnalysisMerger>();
        services.AddScoped<IAnalysisMerger<UserStory, UserStoryLlmAnalysisDto>, UserStoryAnalysisMerger>();

        // auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthProvider, AuthProvider>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICookiesHelper, CookiesHelper>();

        // LLM providers
        services.AddSingleton<ILLMProviderAdapter, OpenAIProvider>();
        services.AddSingleton<ILLMProviderAdapter, AnthropicProvider>();
        services.AddSingleton<ILLMProviderAdapter, MoonshotAIProvider>();
        services.AddSingleton<ILLMProviderAdapter, GoogleProvider>();
        services.AddSingleton<ILLMRouteResolver, LLMRouteResolver>();
        services.AddScoped<ILLMProvider, RoutingLLMProvider>();

        // helper services
        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

        // builders
        services.AddScoped<IItemContextBuilder, ItemContextBuilder>();
        services.AddScoped<IPromptBuilder, PromptBuilder>();

        services.AddHttpClient();

        // validators
        services.AddScoped<IValidator<AcceptanceCriteriaForLLMDto>, AcceptanceCriteriaForLLMDtoValidator>();
        services.AddScoped<IValidator<EdgeCaseForLLMDto>, EdgeCaseForLLMDtoValidator>();
        services.AddScoped<IValidator<PersonaForLLMDto>, PersonaForLLMDtoValidator>();
        services.AddScoped<IValidator<ScenarioForLLMDto>, ScenarioForLLMDtoValidator>();
        services.AddScoped<IValidator<UserStoryForLLMDto>, UserStoryForLLMDtoValidator>();
        services.AddScoped<IValidator<UserStorySplitResultDto>, UserStorySplitResultDtoValidator>();
        services.AddScoped<IValidator<PersonaLlmAnalysisDto>, PersonaLlmAnalysisDtoValidator>();
        services.AddScoped<IValidator<ScenarioLlmAnalysisDto>, ScenarioLlmAnalysisDtoValidator>();
        services.AddScoped<IValidator<UserStoryLlmAnalysisDto>, UserStoryLlmAnalysisDtoValidator>();

        services.AddHttpContextAccessor();

        return services;
    }
}
