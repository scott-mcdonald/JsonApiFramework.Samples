using System.Diagnostics.Contracts;

using Blogging.ServiceModel;

using JsonApiFramework;
using JsonApiFramework.Http;
using JsonApiFramework.Server;
using JsonApiFramework.Server.Hypermedia;
using JsonApiFramework.ServiceModel;

using Microsoft.Extensions.Options;

namespace Blogging.WebService.Framework.Internal
{
    internal class ApiJsonApiFrameworkRegistry : IApiJsonApiFrameworkRegistry
    {
        // PUBLIC CONSTRUCTORS //////////////////////////////////////////////
        #region Constructors
        public ApiJsonApiFrameworkRegistry(IOptions<ApiHypermediaOptions> apiHypermediaOptionsAccessor)
        {
            Contract.Requires(apiHypermediaOptionsAccessor != null);

            var apiServiceModel           = ConfigurationFactory.CreateServiceModel();
            var apiDocumentContextOptions = CreateDocumentContextOptions(apiServiceModel, null, apiHypermediaOptionsAccessor);

            this.ApiServiceModel           = apiServiceModel;
            this.ApiDocumentContextOptions = apiDocumentContextOptions;
        }
        #endregion

        // PUBLIC PROPERTIES ////////////////////////////////////////////////
        #region IApiJsonApiFrameworkRegistry Implementation
        public IDocumentContextOptions ApiDocumentContextOptions { get; }

        public IServiceModel ApiServiceModel { get; }
        #endregion

        // PRIVATE METHODS //////////////////////////////////////////////////
        #region Factory Methods
        private static IDocumentContextOptions CreateDocumentContextOptions(IServiceModel                  apiServiceModel,
                                                                            IHypermediaAssemblerRegistry   apiHypermediaAssemblerRegistry,
                                                                            IOptions<ApiHypermediaOptions> apiHypermediaOptionsAccessor)
        {
            Contract.Requires(apiServiceModel != null);
            Contract.Requires(apiHypermediaOptionsAccessor != null);

            var options        = new DocumentContextOptions<DocumentContext>();
            var optionsBuilder = new DocumentContextOptionsBuilder(options);

            optionsBuilder.UseServiceModel(apiServiceModel);
            if (apiHypermediaAssemblerRegistry != null)
            {
                optionsBuilder.UseHypermediaAssemblerRegistry(apiHypermediaAssemblerRegistry);
            }

            var apiHypermediaOptions = apiHypermediaOptionsAccessor.Value;

            var scheme                  = apiHypermediaOptions.Scheme;
            var host                    = apiHypermediaOptions.Host;
            var port                    = apiHypermediaOptions.Port;
            var urlBuilderConfiguration = new UrlBuilderConfiguration(scheme, host, port);
            optionsBuilder.UseUrlBuilderConfiguration(urlBuilderConfiguration);

            return options;
        }
        #endregion
    }
}