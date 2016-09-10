using System.Diagnostics.Contracts;

using Blogging.ServiceModel;

using JsonApiFramework;
using JsonApiFramework.Http;
using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService
{
    public class BloggingDocumentContext : DocumentContext
    {
        #region Constructors
        public BloggingDocumentContext(IUrlBuilderConfiguration urlBuilderConfiguration)
        {
            Contract.Requires(urlBuilderConfiguration != null);

            this.UrlBuilderConfiguration = urlBuilderConfiguration;
        }

        public BloggingDocumentContext(IUrlBuilderConfiguration urlBuilderConfiguration, Document document)
            : base(document)
        {
            Contract.Requires(urlBuilderConfiguration != null);

            this.UrlBuilderConfiguration = urlBuilderConfiguration;
        }
        #endregion

        #region DocumentContext Overrides
        protected override void OnConfiguring(IDocumentContextOptionsBuilder optionsBuilder)
        {
            Contract.Requires(optionsBuilder != null);

            var conventions = ConfigurationFactory.CreateConventions();
            var serviceModel = ConfigurationFactory.CreateServiceModel();
            var urlBuilderConfiguration = this.UrlBuilderConfiguration;

            optionsBuilder.UseConventionSet(conventions);
            optionsBuilder.UseServiceModel(serviceModel);
            optionsBuilder.UseUrlBuilderConfiguration(urlBuilderConfiguration);
        }
        #endregion

        #region Properties
        private IUrlBuilderConfiguration UrlBuilderConfiguration { get; set; }
        #endregion
    }
}
