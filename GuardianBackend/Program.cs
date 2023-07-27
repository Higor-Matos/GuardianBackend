using GuardianBackend.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

StartupConfiguration.ConfigureLogging(builder);
StartupConfiguration.ConfigureDatabase(builder);
StartupConfiguration.ConfigureServices(builder);
StartupConfiguration.ConfigureOcelot(builder);

var app = builder.Build();

// Configure the HTTP request pipeline
StartupConfiguration.ConfigureMiddleware(app);

app.Run();