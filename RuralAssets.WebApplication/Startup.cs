using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Caching;
using Volo.Abp.Http.Modeling;

namespace RuralAssets.WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = services.GetConfiguration();

            services.AddApplication<AbpAspNetCoreModule>();
            services.AddTransient<IValidationService, ValidationService>();
            services.AddTransient<IChangeStatusService, ChangeStatusService>();
            services.AddSingleton<IDistributedCacheSerializer, Utf8JsonDistributedCacheSerializer>();
            services.AddSingleton<IDistributedCacheKeyNormalizer, DistributedCacheKeyNormalizer>();
            services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
            services.AddSingleton<NonceCache>();
            services.AddTransient<IApiDescriptionModelProvider, AspNetCoreApiDescriptionModelProvider>();
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.UseApiBehavior = false;
            });
            services.AddVersionedApiExplorer();

            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Rural Assets Platform API", Version = "v1"});
                    options.OperationFilter<SwaggerFileUploadFilter>();
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                }
            );

            services.Configure<ConfigOptions>(configuration.GetSection("Config"));
            services.Configure<ApiAuthorizeOptions>(configuration.GetSection("ApiAuthorize"));
            services.Configure<ModuleConfigOptions>(options =>
            {
                options.EnableAuthorization = configuration.GetValue<bool>("EnableAuthorization");
                options.EnableIdCardCheck = configuration.GetValue<bool>("EnableIdCardCheck");
            });

            if (configuration.GetValue<bool>("EnableAuthorization"))
            {
                services.AddMvc(mvc => mvc.Filters.Add<AuthorizeFilter>());
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.InitializeApplication();
            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "AssetPlatform API"); });
            app.UseConfiguredEndpoints();
            //app.UseMiddleware<SecurityMiddleware>();
        }
    }
}