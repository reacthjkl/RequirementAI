using RequirementAI.RefinementWorker;

var builder = Host.CreateApplicationBuilder(args);

AppSetup.SetupServices(builder);

AppSetup.SetupLayers(builder);

AppSetup.SetupEntityFramework(builder);

AppSetup.SetupAutoMapper(builder);

var host = builder.Build();

host.Run();