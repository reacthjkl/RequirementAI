using RequirementAI.API;

var builder = WebApplication.CreateBuilder(args);

AppSetup.SetupConfiguration(builder);

AppSetup.SetupLogging(builder);

AppSetup.SetupCors(builder);

AppSetup.SetupAutoMapper(builder);

AppSetup.SetupSwagger(builder);

AppSetup.SetupEntityFramework(builder);

AppSetup.SetupAuthentication(builder);

AppSetup.SetupHttpContextAccessor(builder);

AppSetup.SetupServices(builder);

AppSetup.SetupRepositories(builder);

AppSetup.SetupControllers(builder);

var app = builder.Build();

AppSetup.ApplyMigrations(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(app.Environment.EnvironmentName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();