using RequirementAI.API;

var builder = WebApplication.CreateBuilder(args);

AppSetup.SetupConfiguration(builder);

AppSetup.SetupLogging(builder);

AppSetup.SetupCors(builder);

AppSetup.SetupAutoMapper(builder);

AppSetup.SetupSwagger(builder);

AppSetup.SetupEntityFramework(builder);

AppSetup.SetupAuthentication(builder);

AppSetup.SetupServices(builder);

AppSetup.SetupAuthProviders(builder);

AppSetup.SetupRepositories(builder);

AppSetup.SetupExceptionHandler(builder);

AppSetup.SetupControllers(builder);

AppSetup.SetupHttpContextAccessor(builder);

var app = builder.Build();

AppSetup.ApplyMigrations(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors(app.Environment.EnvironmentName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();