using RequirementAI.RefinementWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

AppSetup.SetupLayers(builder);

AppSetup.SetupEntityFramework(builder);

AppSetup.SetupAutoMapper(builder);

var host = builder.Build();

host.Run();