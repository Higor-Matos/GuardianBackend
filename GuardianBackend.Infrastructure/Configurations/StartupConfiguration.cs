using GuardianBackend.Infrastructure.Data;
using GuardianBackend.Infrastructure.Middlewares;
using GuardianBackend.Infrastructure.ReflectionDI.Extensions;
using GuardianBackend.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NLog.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Reflection;

namespace GuardianBackend.Infrastructure.Configurations
{
    public static class StartupConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure();

            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<LoggingAnchor>>();
            var assemblies = new[]
            {
                Assembly.Load("GuardianBackend.Domain"),
                Assembly.Load("GuardianBackend.Services"),
                Assembly.Load("GuardianBackend.Repository")
            };
            builder.Services.AddAutoDI(logger, assemblies);
        }

        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            if (builder.Environment.IsDevelopment())
            {
                builder.Logging.SetMinimumLevel(LogLevel.Trace);
            }
            else
            {
                builder.Logging.SetMinimumLevel(LogLevel.Information);
            }
            builder.Logging.AddNLog();
        }


        public static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<GuardianDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 21))));
        }

        public static void ConfigureOcelot(WebApplicationBuilder builder)
        {
            builder.Services.AddOcelot(builder.Configuration);
        }

        public static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GuardianBackend API v1");
                    c.RoutePrefix = "swagger";
                });
            }
            else
            {
                app.UseMiddleware<ErrorHandlingMiddleware>();
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            app.UseOcelot().Wait();

            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("A aplicação foi iniciada com sucesso.");
        }
    }
}