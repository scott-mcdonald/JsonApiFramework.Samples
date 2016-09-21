using System;
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
        public BloggingDocumentContext(Uri currentRequestUrl)
        {
            Contract.Requires(currentRequestUrl != null);

            var urlBuilderConfiguration = CreateUrlBuilderConfiguration(currentRequestUrl);
            this.UrlBuilderConfiguration = urlBuilderConfiguration;
        }

        public BloggingDocumentContext(Uri currentRequestUrl, Document document)
            : base(document)
        {
            Contract.Requires(currentRequestUrl != null);

            var urlBuilderConfiguration = CreateUrlBuilderConfiguration(currentRequestUrl);
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

            optionsBuilder.UseConventions(conventions);
            optionsBuilder.UseServiceModel(serviceModel);
            optionsBuilder.UseUrlBuilderConfiguration(urlBuilderConfiguration);
        }
        #endregion

        #region Properties
        private IUrlBuilderConfiguration UrlBuilderConfiguration { get; set; }
        #endregion

        #region Methods
        private static UrlBuilderConfiguration CreateUrlBuilderConfiguration(Uri currentRequestUrl)
        {
            Contract.Requires(currentRequestUrl != null);

            var scheme = currentRequestUrl.Scheme;
            var host = currentRequestUrl.Host;
            var port = currentRequestUrl.Port;
            var urlBuilderConfiguration = new UrlBuilderConfiguration(scheme, host, port);
            return urlBuilderConfiguration;
        }
        #endregion
    }
}
