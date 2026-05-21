using Microsoft.EntityFrameworkCore;
using RequirementAI.Business;
using RequirementAI.Business.MappingProfiles;
using RequirementAI.Business.Services.Refinement;
using RequirementAI.Persistence;

namespace RequirementAI.RefinementWorker;

public static class AppSetup
{
    public static void SetupLayers(HostApplicationBuilder builder)
    {
        builder.Services.AddBusiness();
        builder.Services.AddPersistence();
    }

    public static void SetupEntityFramework(HostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<RequirementAIContext>(options =>
        {
            if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });    
    }

    public static void SetupAutoMapper(HostApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(_ => { },
            typeof(UserProfile),
            typeof(PersonaProfile),
            typeof(ScenarioProfile),
            typeof(UserStoryProfile),
            typeof(AcceptanceCriteriaProfile),
            typeof(EdgeCaseProfile)
        );    
    }

    public static void SetupServices(HostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddScoped<IProjectRefinementJobProcessor, ProjectRefinementJobProcessor>();    }
}