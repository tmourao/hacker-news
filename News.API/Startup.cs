using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreRateLimit;
using News.Core.Web.Infrastructure.Extensions;
using System;
using News.Services.Hacker;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace News.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment  { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Http Clients

            var hackerNewsProvider = Configuration
                .GetSection("HackerNewsProvider")
                .Get<NewsProviderOptions>();

           if (!string.IsNullOrWhiteSpace(hackerNewsProvider?.BaseAddress))
            {
                services.AddHttpClient<IHackerNewsService, HackerNewsService>(c =>
                {
                    c.BaseAddress = new Uri(hackerNewsProvider.BaseAddress);
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (hackerNewsProvider.Timeout.HasValue)
                    {
                        c.Timeout = TimeSpan.FromSeconds(hackerNewsProvider.Timeout.Value);
                    }

                });
            }

            #endregion

            #region Generic Config

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddCustomSwagger("News");

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    // make the results readable on DEV
                    options.SerializerSettings.Formatting = Environment.IsDevelopment() ?
                        Formatting.Indented :
                        Formatting.None;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddOptions();

            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimit"));

            #endregion

            #region Dependency Injection

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddHttpContextAccessor();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api-docs/v1/swagger.json", "News API v1");
            });

            app.UseIpRateLimiting();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "api/v1/{controller}/{action}");
            });
        }
    }
}
