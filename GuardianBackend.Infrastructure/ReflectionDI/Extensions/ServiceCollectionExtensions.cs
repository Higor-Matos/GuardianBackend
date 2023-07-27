using GuardianBackend.Common.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace GuardianBackend.Infrastructure.ReflectionDI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoDI(this IServiceCollection services, ILogger logger, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                logger.LogInformation("Verificando assembly: {AssemblyName}", assembly.FullName);
                var typesWithAutoDI = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(AutoDIAttribute), false).Any()).ToArray();
                logger.LogInformation("Total de tipos com AutoDI no assembly {AssemblyName}: {Count}", assembly.FullName, typesWithAutoDI.Length);

                foreach (var type in typesWithAutoDI)
                {
                    logger.LogInformation("Procurando implementação para: {InterfaceName}", type.FullName);
                    Type implementation = FindImplementationForType(type, assemblies);
                    if (implementation != null)
                    {
                        logger.LogInformation("Encontrada implementação {ImplementationName} para {InterfaceName}", implementation.FullName, type.FullName);
                        services.AddScoped(type, implementation);
                    }
                    else
                    {
                        logger.LogWarning("Nenhuma implementação encontrada para: {InterfaceName}", type.FullName);
                    }
                }
            }
            return services;
        }

        private static Type FindImplementationForType(Type type, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var implementation = assembly.GetTypes().FirstOrDefault(t => !t.IsInterface && type.IsAssignableFrom(t));
                if (implementation != null)
                    return implementation;
            }
            return null;
        }
    }
}