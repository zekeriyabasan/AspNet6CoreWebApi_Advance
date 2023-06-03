using AspNetCoreRateLimit;
using Entities.DataTransferObjects;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Services;
using Services.Contracts;
using System.Collections;

namespace BookApi.Extentions
{
    public static class ServicesExtensions // static olmak zorunda
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();
        public static void ConfigureServiceManager(this IServiceCollection services) => services.AddScoped<IServiceManager, ServiceManager>();
        public static void ConfigureLoggerService(this IServiceCollection services) => services.AddSingleton<ILoggerService, LoggerManager>();
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>(); // log için singelton mantıklı
            services.AddScoped<ValidateMediaTypeAttribute>();
        }

        public static void ConfigureDataShaper(this IServiceCollection services) =>
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        public static void ConfigureCors(this IServiceCollection services) // cors  kaynağa erişim izni için
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader().
                WithExposedHeaders("X-Pagination")

                    );
            });
        }

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault(); // oftype (linq) bu tipte formatter varsa al

                if (systemTextJsonOutputFormatter is not null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.tesla.hateoas+json");
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.tesla.apiroot+json");
                }

                var xmlOutputFormatter = config
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if (xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.tesla.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.tesla.apiroot+xml");
                }
            });
        }

        public static void ConfigureVersionning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");

                options.Conventions.Controller<BooksController>().HasApiVersion(new ApiVersion(1, 0));
                options.Conventions.Controller<BooksV2Controller>().HasDeprecatedApiVersion(new ApiVersion(2, 0));

            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) =>
            services.AddResponseCaching();

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders(expressionOpt =>
            {
                expressionOpt.MaxAge = 70;
                //expressionOpt.CacheLocation = CacheLocation.Private;
                expressionOpt.CacheLocation = CacheLocation.Public;
            },
            validationOpt =>
            {
                //validationOpt.MustRevalidate = true;
                validationOpt.MustRevalidate = false;
            });

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint="*",
                    Limit=3,
                    Period="1m"
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore,MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy,AsyncKeyLockProcessingStrategy>();
        }

    }
}
