using GuardianBackend.Presentation.Extensions;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Adicionar os servi�os ao cont�iner.
builder.Services.AddControllers();
// Saiba mais sobre como configurar o Swagger/OpenAPI em https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configura��o do NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddNLog();

var app = builder.Build();

// Criar o logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Adicionar servi�os na assembly
builder.Services.AddServicesInAssembly(logger);

// Configurar o pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

logger.LogInformation("A aplica��o foi iniciada com sucesso.");

app.Run();