using GuardianBackend.Common.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GuardianBackend.Infrastructure.ReflectionDI
{
    public static class AutoDIRegistrar
    {
        public static void RegisterServices(IServiceCollection services, Assembly assembly)
        {
            var typesWithAutoDI = assembly.GetTypes()
                                          .Where(t => t.GetCustomAttributes(typeof(AutoDIAttribute), false).Any())
                                          .ToArray();

            foreach (var type in typesWithAutoDI)
            {
                var implementation = assembly.GetTypes().ToList().Find(t => type.IsAssignableFrom(t) && !t.IsInterface);
                if (implementation != null)
                {
                    services.AddScoped(type, implementation);
                }
            }
        }
    }
}
