using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GuardianBackend.Infrastructure.Swagger
{
    public static class AddCustomSwaggerConfiguration
    {
        public static void Configure(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GuardianBackEnd", Version = "v1" });
                ExcludeSwaggerPaths(c);
            });
        }

        private static void ExcludeSwaggerPaths(SwaggerGenOptions c)
        {
            var pathsToIgnore = new List<string> { "configuration", "outputcache" };
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                return apiDesc?.RelativePath != null && !pathsToIgnore.Exists(p => apiDesc.RelativePath.StartsWith(p));
            });
        }
    }
}
