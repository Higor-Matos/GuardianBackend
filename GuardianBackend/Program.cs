using GuardianBackend.Infrastructure.Middlewares;
using GuardianBackend.Infrastructure.Swagger;
using GuardianBackend.Infrastructure.Extensions;
using NLog.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Services configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure();

// Logging configuration
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog();

// Ocelot configuration
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Create the logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Add services in assembly
builder.Services.AddServicesInAssembly(logger);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseOcelot().Wait();

logger.LogInformation("A aplicação foi iniciada com sucesso.");

app.Run();