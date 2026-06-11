using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RequirementAI.API.Middleware;
using RequirementAI.Business;
using RequirementAI.Business.MappingProfiles;
using RequirementAI.Contract.Settings;
using RequirementAI.Persistence;
using Serilog;

namespace RequirementAI.API;

public static class AppSetup
{
    public static void SetupLayers(WebApplicationBuilder builder)
    {
        builder.Services.AddBusiness();
        builder.Services.AddPersistence();
    }

    public static void SetupAutoMapper(WebApplicationBuilder builder)
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
            typeof(EdgeCaseProfile),
            typeof(QualityScoreProfile),
            typeof(QualityAnalysisJobProfile)
        );
    }

    public static void SetupEntityFramework(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<RequirementAIContext>(options =>
        {
            if (builder.Environment.IsDevelopment()) options.EnableSensitiveDataLogging();
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
    }

    public static void SetupSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    public static void SetupControllers(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
    }

    public static void SetupCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Development", b =>
            {
                b.WithOrigins("http://localhost:4200", "https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            options.AddPolicy("Staging", b =>
            {
                b.WithOrigins("https://stg.requirements-ai.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            options.AddPolicy("Production", b =>
            {
                b.WithOrigins("https://requirements-ai.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    public static void SetupLogging(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });
    }

    public static void SetupConfiguration(WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("Jwt"));

        builder.Services.Configure<Argon2Settings>(builder.Configuration.GetSection("Argon2"));
    }

    public static void SetupAuthentication(WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
        var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();
    }

    public static void ApplyMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RequirementAIContext>();
        db.Database.Migrate();
    }

    public static void SetupExceptionHandler(WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
    }
}