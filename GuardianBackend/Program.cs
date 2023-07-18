using GuardianBackend.Presentation.Extensions;
using NLog.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Adicionar os serviços ao contêiner.
builder.Services.AddControllers();
// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot(builder.Configuration);
// Configuração do NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddNLog();

var app = builder.Build();

// Criar o logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Adicionar serviços na assembly
builder.Services.AddServicesInAssembly(logger);

// Configurar o pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Adicionar o Ocelot ao pipeline de middlewares
app.UseOcelot().Wait();

logger.LogInformation("A aplicação foi iniciada com sucesso.");

app.Run();

logger.LogInformation("A aplicação está sendo encerrada.");
