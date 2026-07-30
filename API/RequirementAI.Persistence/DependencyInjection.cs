using Microsoft.Extensions.DependencyInjection;
using RequirementAI.Persistence.Interfaces;
using RequirementAI.Persistence.Repositories;

namespace RequirementAI.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // repositories
        services.AddScoped<IAcceptanceCriteriaRepository, AcceptanceCriteriaRepository>();
        services.AddScoped<IEdgeCaseRepository, EdgeCaseRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IPersonaRepository, PersonaRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IScenarioRepository, ScenarioRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserStoryRepository, UserStoryRepository>();
        services.AddScoped<IProjectRefinementJobRepository, ProjectRefinementJobRepository>();
        services.AddScoped<IQualityAnalysisJobRepository, QualityAnalysisJobRepository>();
        services.AddScoped<IQualityScoreRepository, QualityScoreRepository>();

        return services;
    }
}