using System;
using System.Web;
using System.Web.Http;

using JsonApiFramework.Http;
using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService.Controllers
{
    public class BlogsController : ApiController
    {
        [Route("blogs")]
        public Document GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Blogs from repository
            /////////////////////////////////////////////////////
            var blogs = BloggingRepository.GetBlogs();

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUrl = HttpContext.Current.Request.Url;
            var urlBuilderConfiguration = new UrlBuilderConfiguration(currentRequestUrl.Scheme, currentRequestUrl.Host, currentRequestUrl.Port);
            using (var documentContext = new BloggingDocumentContext(urlBuilderConfiguration))
            {
                var document = documentContext
                    .NewDocument(currentRequestUrl)
                        .SetJsonApiVersion(JsonApiVersion.Version10)
                        .Links()
                            .AddUpLink()
                            .AddSelfLink()
                        .LinksEnd()
                        .ResourceCollection(blogs)
                            .Relationships()
                                .AddRelationship("articles", Keywords.Related)
                            .RelationshipsEnd()
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .ResourceCollectionEnd()
                    .WriteDocument();

                return document;
            }
        }

        [Route("blogs/{id}")]
        public Document Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Blog by identifier from repository
            /////////////////////////////////////////////////////
            var blog = BloggingRepository.GetBlog(Convert.ToInt64(id));

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUrl = HttpContext.Current.Request.Url;
            var urlBuilderConfiguration = new UrlBuilderConfiguration(currentRequestUrl.Scheme, currentRequestUrl.Host, currentRequestUrl.Port);
            using (var documentContext = new BloggingDocumentContext(urlBuilderConfiguration))
            {
                var document = documentContext
                    .NewDocument(currentRequestUrl)
                        .SetJsonApiVersion(JsonApiVersion.Version10)
                        .Links()
                            .AddUpLink()
                            .AddSelfLink()
                        .LinksEnd()
                        .Resource(blog)
                            .Relationships()
                                .AddRelationship("articles", Keywords.Related)
                            .RelationshipsEnd()
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .ResourceEnd()
                    .WriteDocument();

                return document;
            }
        }

        [Route("blogs/{id}/articles")]
        public Document GetBlogToArticles(string id)
        {
            /////////////////////////////////////////////////////
            // Get Blog to related Articles by Blog identifier from repository
            /////////////////////////////////////////////////////
            var blogToArticles = BloggingRepository.GetBlogToArticles(Convert.ToInt64(id));

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUrl = HttpContext.Current.Request.Url;
            var urlBuilderConfiguration = new UrlBuilderConfiguration(currentRequestUrl.Scheme, currentRequestUrl.Host, currentRequestUrl.Port);
            using (var documentContext = new BloggingDocumentContext(urlBuilderConfiguration))
            {
                var document = documentContext
                    .NewDocument(currentRequestUrl)
                        .SetJsonApiVersion(JsonApiVersion.Version10)
                        .Links()
                            .AddUpLink()
                            .AddSelfLink()
                        .LinksEnd()
                        .ResourceCollection(blogToArticles)
                            .Relationships()
                                .AddRelationship("blog", Keywords.Related)
                                .AddRelationship("comments", Keywords.Related)
                            .RelationshipsEnd()
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .ResourceCollectionEnd()
                    .WriteDocument();

                return document;
            }
        }

        [Route("blogs")]
        public Document Post([FromBody]Document inDocument)
        {
            var outDocument = new Document
                {
                    JsonApiVersion = JsonApiVersion.Version10
                };
            return outDocument;
        }

        [Route("blogs/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            var outDocument = new Document
                {
                    JsonApiVersion = JsonApiVersion.Version10
                };
            return outDocument;
        }

        [Route("blogs/{id}")]
        public void Delete(string id)
        {
        }
    }
}
