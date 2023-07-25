using GuardianBackend.Presentation.Extensions;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar os serviços ao contêiner.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sua API", Version = "v1" });

    // Ignorar certos caminhos
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        // Ignora os endpoints de administração e cache do Ocelot
        var pathsToIgnore = new List<string> { "configuration", "outputcache" };
        if (pathsToIgnore.Any(p => apiDesc.RelativePath.StartsWith(p)))
        {
            return false;
        }
        return true;
    });
});


// Configuração do NLog
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddNLog();

// Adicionar Ocelot
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Criar o logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Adicionar serviços na assembly
builder.Services.AddServicesInAssembly(logger);

// Configurar o pipeline de solicitação HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Use Ocelot para lidar com as rotas
app.UseOcelot().Wait();

logger.LogInformation("A aplicação foi iniciada com sucesso.");

app.Run();
