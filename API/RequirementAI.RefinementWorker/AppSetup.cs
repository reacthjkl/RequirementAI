using Microsoft.EntityFrameworkCore;
using RequirementAI.Business;
using RequirementAI.Business.MappingProfiles;
using RequirementAI.Business.Services.Refinement;
using RequirementAI.Persistence;
using Serilog;

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
        builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = builder.Configuration["AutoMapperOptions:LicenseKey"];
            },
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

    public static void SetupLogging(HostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Services.AddSerilog((services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
        
    }
}