using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using GuardianBackend.Common.Attributes;

namespace GuardianBackend.Infrastructure.ReflectionDI
{
    public static class AutoDIRegistrar
    {
        public static void RegisterServices(IServiceCollection services, Assembly assembly)
        {
            // Obter todos os tipos com o atributo AutoDI
            var typesWithAutoDI = assembly.GetTypes()
                                          .Where(t => t.GetCustomAttributes(typeof(AutoDIAttribute), false).Length > 0)
                                          .ToArray();

            foreach (var type in typesWithAutoDI)
            {
                // Para cada tipo, obtenha a implementação correspondente e adicione aos serviços
                var implementation = assembly.GetTypes().FirstOrDefault(t => type.IsAssignableFrom(t) && !t.IsInterface);
                if (implementation != null)
                {
                    services.AddScoped(type, implementation);
                }
            }
        }
    }
}
