using System.Diagnostics.Contracts;

using Blogging.WebService.ErrorHandling;
using Blogging.WebService.Framework;
using Blogging.WebService.Framework.Internal;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogging.WebService
{
    /// <summary>Extension methods for the <see cref="IServiceCollection"/> that help in configuration of dependency injection.</summary>
    public static class ServicesExtensions
    {
        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region Extension Methods
        public static IServiceCollection AddApiServices(this IServiceCollection services,
                                                        IConfiguration          configuration)
        {
            Contract.Requires(services != null);

            services.AddApiExceptionFilter(configuration);
            services.AddApiServiceContext(configuration);

            services.Configure<ApiHypermediaOptions>(configuration.GetSection("ApiHypermediaOptions"));

            return services;
        }

        public static IServiceCollection AddBloggingRepository(this IServiceCollection services,
                                                               IConfiguration          configuration)
        {
            services.AddSingleton<BloggingRepository>();

            return services;
        }
        #endregion

        // PRIVATE METHODS //////////////////////////////////////////////////
        #region Methods
        private static void AddApiExceptionFilter(this IServiceCollection services, IConfiguration configuration)
        {
            Contract.Requires(services != null);

            services.AddMvc(options => { options.Filters.Add(new ServiceFilterAttribute(typeof(ApiExceptionFilter))); });

            services.AddScoped<ApiExceptionFilter>();
        }

        private static void AddApiServiceContext(this IServiceCollection services, IConfiguration configuration)
        {
            Contract.Requires(services != null);

            services.AddSingleton<IApiJsonApiFrameworkRegistry, ApiJsonApiFrameworkRegistry>();

            services.AddScoped<IApiServiceContext, ApiServiceContext>();
        }
        #endregion
    }
}