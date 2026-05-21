using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Business.Helpers;
using RequirementAI.Business.Interfaces;
using RequirementAI.Business.Providers;
using RequirementAI.Business.Providers.LLM;
using RequirementAI.Business.Services;
using RequirementAI.Business.Services.Auth;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddScoped<IRefinementService, RefinementService>();

        services.AddScoped<IPromptBuilder, PromptBuilder>();
        
        // entity related services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserStoryService, UserStoryService>();
        
        // refinement services
        services.AddScoped<IRefinementService, RefinementService>();
        services.AddScoped<IRefinementMerger<Persona, PersonaForLLMDto>, PersonaRefinementMerger>();
        services.AddScoped<IRefinementMerger<Scenario, ScenarioForLLMDto>, ScenarioRefinementMerger>();
        services.AddScoped<IRefinementMerger<UserStory, UserStoryForLLMDto>, UserStoryRefinementMerger>();
        
        // auth providers
        services.AddScoped<IAuthService, AuthService>();

        // LLM providers
        services.AddScoped<ILLMProvider, OpenAIProvider>();
        
        // helper services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        services.AddScoped<ICookiesHelper, CookiesHelper>();
        services.AddScoped<IPromptBuilder, PromptBuilder>();
        services.AddScoped<IRefinementTaskProvider, RefinementTaskProvider>();
        services.AddHttpClient();
        
        services.AddScoped<ITestService, TestService>();

        return services;
    }
}