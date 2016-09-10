using System;
using System.Web;
using System.Web.Http;

using Blogging.ServiceModel;

using JsonApiFramework.Http;
using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService.Controllers
{
    public class ApiEntryPointController : ApiController
    {
        [Route("")]
        public Document GetAsync()
        {
            var apiEntryPoint = new ApiEntryPoint
                {
                    Id = String.Empty,
                    Message = "Entry point into the Blogging Hypermedia API powered by JsonApiFramework [Server]. Implements the JSON API 1.0 specification.",
                    Version = "1.0"
                };

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUrl = HttpContext.Current.Request.Url;

            var urlBuilderConfiguration = new UrlBuilderConfiguration(currentRequestUrl.Scheme, currentRequestUrl.Host, currentRequestUrl.Port);
            var articlesResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                          .Path("articles")
                                                          .Build();
            var blogsResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                       .Path("blogs")
                                                       .Build();
            var commentsResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                          .Path("comments")
                                                          .Build();
            var peopleResourceCollectionUrl = UrlBuilder.Create(urlBuilderConfiguration)
                                                        .Path("people")
                                                        .Build();

            using (var documentContext = new BloggingDocumentContext(urlBuilderConfiguration))
            {
                var document = documentContext
                    .NewDocument(currentRequestUrl)
                        .SetJsonApiVersion(JsonApiVersion.Version10)
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                        .Resource(apiEntryPoint)
                            .Links()
                                .AddLink("articles", new Link(articlesResourceCollectionUrl))
                                .AddLink("blogs", new Link(blogsResourceCollectionUrl))
                                .AddLink("comments", new Link(commentsResourceCollectionUrl))
                                .AddLink("people", new Link(peopleResourceCollectionUrl))
                            .LinksEnd()
                        .ResourceEnd()
                    .WriteDocument();

                return document;
            }
        }
    }
}
