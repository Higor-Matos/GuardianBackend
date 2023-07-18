using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Adicione os serviços ao contêiner.
builder.Services.AddControllers();
// Saiba mais sobre a configuração do Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot(builder.Configuration);

// Configuração do NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddNLog();

var app = builder.Build();

// Configura o pipeline de solicitações HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Adicione o Ocelot ao pipeline de middleware
app.UseOcelot().Wait();

// Cria o logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("O aplicativo foi iniciado.");

app.Run();

logger.LogInformation("O aplicativo está sendo encerrado.");
