using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace GuardianBackend.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesInAssembly(this IServiceCollection services, ILogger logger)
        {
            logger.LogInformation("Iniciando a adição de serviços na assembly");

            var assemblies = GetAssembliesToScan(logger);
            RegisterServices(services, assemblies, logger);

            logger.LogInformation("A adição de serviços na assembly foi concluída com sucesso");
            return services;
        }

        private static IEnumerable<Assembly> GetAssembliesToScan(ILogger logger)
        {
            logger.LogInformation("Obtendo assemblies para escanear");

            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                logger.LogError("Assembly de entrada não encontrada");
                throw new InvalidOperationException("Assembly de entrada não encontrada");
            }

            var assemblies = entryAssembly.GetReferencedAssemblies()
                .Select(Assembly.Load)
                .Concat(new[] { entryAssembly });

            foreach (var assembly in assemblies)
            {
                logger.LogInformation($"Analisando assembly: {assembly.FullName}");
            }

            return assemblies;
        }

        private static void RegisterServices(IServiceCollection services, IEnumerable<Assembly> assemblies, ILogger logger)
        {
            var types = assemblies.SelectMany(a => a.ExportedTypes);
            foreach (Type type in types.Where(t => t.IsClass && !t.IsAbstract))
            {
                RegisterServiceForType(services, type, logger);
            }
        }

        private static void RegisterServiceForType(IServiceCollection services, Type type, ILogger logger)
        {
            logger.LogInformation($"Registrando serviço para o tipo: {type.FullName}");

            var interfaces = type.GetInterfaces();
            foreach (Type @interface in interfaces)
            {
                if (!@interface.IsAbstract)
                {
                    services.AddScoped(@interface, type);
                    logger.LogInformation($"Serviço registrado: {type.FullName} como {@interface.FullName}");
                }
            }
        }
    }
}