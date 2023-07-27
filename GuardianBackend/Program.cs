using FluentAssertions.Common;
using GuardianBackend.Domain.Interfaces;
using GuardianBackend.Infrastructure.Configurations;
using GuardianBackend.Repository;
using GuardianBackend.Services;

var builder = WebApplication.CreateBuilder(args);

StartupConfiguration.ConfigureLogging(builder);
StartupConfiguration.ConfigureDatabase(builder);
StartupConfiguration.ConfigureServices(builder);
StartupConfiguration.ConfigureOcelot(builder);


// Add your services here using the services collection of the WebApplicationBuilder
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline
StartupConfiguration.ConfigureMiddleware(app);

app.Run();