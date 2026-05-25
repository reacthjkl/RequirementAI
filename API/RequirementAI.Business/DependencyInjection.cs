using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Business.Helpers;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Interfaces.EntityRelated;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Business.Providers;
using RequirementAI.Business.Providers.Auth;
using RequirementAI.Business.Providers.LLM;
using RequirementAI.Business.Services;
using RequirementAI.Business.Services.Auth;
using RequirementAI.Business.Services.EntityRelated;
using RequirementAI.Business.Services.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Contract.Validators.LLMResponseValidators;
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
        
        // refinement services
        services.AddScoped<IRefinementService, RefinementService>();
        services.AddScoped<IRefinementTaskProvider, RefinementTaskProvider>();
        services.AddScoped<IRefinementMerger<Persona, PersonaForLLMDto>, PersonaRefinementMerger>();
        services.AddScoped<IRefinementMerger<Scenario, ScenarioForLLMDto>, ScenarioRefinementMerger>();
        services.AddScoped<IRefinementMerger<UserStory, UserStoryForLLMDto>, UserStoryRefinementMerger>();
        services.AddScoped<IProjectRefinementOrchestrator, ProjectRefinementOrchestrator>();
        services.AddScoped<IProjectRefinementJobProcessor, ProjectRefinementJobProcessor>();
        services.AddScoped<IPromptBuilder, PromptBuilder>();
        services.AddScoped<IRefinementContextBuilder, RefinementContextBuilder>();
        
        // auth providers
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthProvider, AuthProvider>();
        

        // LLM providers
        services.AddScoped<ILLMProvider, OpenAIProvider>();
        
        // helper services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        services.AddScoped<ICookiesHelper, CookiesHelper>();
        services.AddHttpClient();
        
        // validators
        services.AddScoped<IValidator<AcceptanceCriteriaForLLMDto>, AcceptanceCriteriaForLLMDtoValidator>();
        services.AddScoped<IValidator<EdgeCaseForLLMDto>, EdgeCaseForLLMDtoValidator>();
        services.AddScoped<IValidator<PersonaForLLMDto>, PersonaForLLMDtoValidator>();
        services.AddScoped<IValidator<ScenarioForLLMDto>, ScenarioForLLMDtoValidator>();
        services.AddScoped<IValidator<UserStoryForLLMDto>, UserStoryForLLMDtoValidator>();
        
        services.AddHttpContextAccessor();
        
        return services;
    }
}