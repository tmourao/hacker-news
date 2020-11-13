using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace News.Core.Web.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services, 
            string title, 
            string version = "v1", 
            bool includeXmlComments = true)
        {
            services.AddSwaggerGen(c =>
            {
                if (includeXmlComments)
                {
                    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    c.IncludeXmlComments(xmlPath);
                }

                c.IgnoreObsoleteActions();

                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = title,
                    Version = version
                });
            });
        }
    }
}
