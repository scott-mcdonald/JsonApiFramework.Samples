using System;

using Blogging.ServiceModel;

using JsonApiFramework.Http;
using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

using Microsoft.AspNetCore.Mvc;

namespace Blogging.WebService.Controllers
{
    public class ApiEntryPointController : Controller
    {
        #region WebApi Methods
        [HttpGet("")]
        public Document GetAsync()
        {
            var apiEntryPoint = new ApiEntryPoint
                {
                    Message = @"Entry point into the Blogging Hypermedia API powered by JsonApiFramework [Server]." + " " +
                              "Implements the JSON API 1.0 specification.",
                    Version = "1.0"
                };

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUri = this.Request.GetUri();

            var scheme = currentRequestUri.Scheme;
            var host = currentRequestUri.Host;
            var port = currentRequestUri.Port;
            var urlBuilderConfiguration = new UrlBuilderConfiguration(scheme, host, port);

            var blogsResourceCollectionLink    = CreateBlogsResourceCollectionLink(urlBuilderConfiguration);
            var articlesResourceCollectionLink = CreateArticlesResourceCollectionUrl(urlBuilderConfiguration);
            var commentsResourceCollectionLink = CreateCommentsResourceCollectionUrl(urlBuilderConfiguration);
            var peopleResourceCollectionLink   = CreatePeopleResourceCollectionUrl(urlBuilderConfiguration);

            using (var documentContext = new BloggingDocumentContext(currentRequestUri))
                {
                    var document = documentContext
                        .NewDocument(currentRequestUri)
                            .SetJsonApiVersion(JsonApiVersion.Version10)
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                            .Resource(apiEntryPoint)
                                .Links()
                                    .AddLink("blogs",    blogsResourceCollectionLink)
                                    .AddLink("articles", articlesResourceCollectionLink)
                                    .AddLink("comments", commentsResourceCollectionLink)
                                    .AddLink("people",   peopleResourceCollectionLink)
                                .LinksEnd()
                            .ResourceEnd()
                        .WriteDocument();

                    return document;
                }
        }
        #endregion

        #region Private Methods
        private static Link CreateBlogsResourceCollectionLink(UrlBuilderConfiguration urlBuilderConfiguration)
        {
            var blogsResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                       .Path("blogs")
                                                       .Build();
            var blogResourceCollectionLink = new Link(blogsResourceCollectionUrl);
            return blogResourceCollectionLink;
        }

        private static Link CreateArticlesResourceCollectionUrl(UrlBuilderConfiguration urlBuilderConfiguration)
        {
            var articlesResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                          .Path("articles")
                                                          .Build();
            var articlesResourceCollectionLink = new Link(articlesResourceCollectionUrl);
            return articlesResourceCollectionLink;
        }

        private static Link CreateCommentsResourceCollectionUrl(UrlBuilderConfiguration urlBuilderConfiguration)
        {
            var commentsResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                          .Path("comments")
                                                          .Build();
            var commentsResourceCollectionLink = new Link(commentsResourceCollectionUrl);
            return commentsResourceCollectionLink;
        }

        private static Link CreatePeopleResourceCollectionUrl(UrlBuilderConfiguration urlBuilderConfiguration)
        {
            var peopleResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                        .Path("people")
                                                        .Build();
            var peopleResourceCollectionLink = new Link(peopleResourceCollectionUrl);
            return peopleResourceCollectionLink;
        }
        #endregion
    }
}
