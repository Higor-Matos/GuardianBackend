using GuardianBackend.Infrastructure.Data;
using GuardianBackend.Infrastructure.ReflectionDI;
using GuardianBackend.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NLog.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Reflection;
using GuardianBackend.Infrastructure.ReflectionDI.Extensions;
using GuardianBackend.Domain.Interfaces;

namespace GuardianBackend.Infrastructure.Configurations
{
    public class StartupConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<StartupConfiguration>>();

            var domainAssembly = Assembly.Load("GuardianBackend.Domain");
            foreach (var type in domainAssembly.GetTypes())
            {
                logger.LogInformation("Tipo encontrado: {TypeName}", type.FullName);
            }

            var assemblies = new[] {
                Assembly.Load("GuardianBackend.Domain"),
                Assembly.Load("GuardianBackend.Services"),
                Assembly.Load("GuardianBackend.Repository")
            };

            builder.Services.AddAutoDI(logger, assemblies);

            // builder.Services.Configure(); // Commented out until we know the arguments
        }


        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(LogLevel.Trace);
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
                // app.UseSwaggerUI(); // Commented out until we resolve the issue
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
