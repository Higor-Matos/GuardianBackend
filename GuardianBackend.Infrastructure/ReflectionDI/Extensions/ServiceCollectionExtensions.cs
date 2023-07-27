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

                // Obter todos os tipos com o atributo AutoDI
                var typesWithAutoDI = assembly.GetTypes()
                                              .Where(t => t.GetCustomAttributes(typeof(AutoDIAttribute), false).Length > 0)
                                              .ToArray();

                logger.LogInformation("Total de tipos com AutoDI no assembly {AssemblyName}: {Count}", assembly.FullName, typesWithAutoDI.Length);

                foreach (var type in typesWithAutoDI)
                {
                    logger.LogInformation("Procurando implementação para: {InterfaceName}", type.FullName);

                    Type implementation = null;

                    // Agora, vamos procurar a implementação em todos os assemblies fornecidos
                    foreach (var implAssembly in assemblies)
                    {
                        foreach (var possibleImpl in implAssembly.GetTypes().Where(t => !t.IsInterface))
                        {
                            logger.LogInformation("Verificando possível implementação: {PossibleImplementationName}", possibleImpl.FullName);
                            if (type.IsAssignableFrom(possibleImpl))
                            {
                                implementation = possibleImpl;
                                break;
                            }
                        }
                        if (implementation != null) break; // Se encontrarmos uma implementação, saímos do loop
                    }

                    if (implementation != null)
                    {
                        logger.LogInformation("Encontrada implementação {ImplementationName} para {InterfaceName} no assembly {AssemblyName}", implementation.FullName, type.FullName, implementation.Assembly.FullName);
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
    }
}
