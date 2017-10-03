using System;
using System.Web;
using System.Web.Http;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService.Controllers
{
    public class BlogsController : ApiController
    {
        #region WebApi Methods
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
            using (var documentContext = new BloggingDocumentContext(currentRequestUrl))
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
                                .AddRelationship("articles", new[] { Keywords.Related })
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
            using (var documentContext = new BloggingDocumentContext(currentRequestUrl))
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
                                .AddRelationship("articles", new[] { Keywords.Related })
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
            using (var documentContext = new BloggingDocumentContext(currentRequestUrl))
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
                                .AddRelationship("blog", new[] { Keywords.Related })
                                .AddRelationship("comments", new[] { Keywords.Related })
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
            throw new NotImplementedException();
        }

        [Route("blogs/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [Route("blogs/{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
