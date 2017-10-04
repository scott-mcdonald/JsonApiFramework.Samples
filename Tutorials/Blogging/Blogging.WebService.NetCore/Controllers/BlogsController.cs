using System;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Blogging.WebService.Controllers
{
    public class BlogsController : Controller
    {
        #region WebApi Methods
        [HttpGet("blogs")]
        public Document GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Blogs from repository
            /////////////////////////////////////////////////////
            var blogs = BloggingRepository.GetBlogs();

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUri = this.Request.GetUri();
            using (var documentContext = new BloggingDocumentContext(currentRequestUri))
            {
                var document = documentContext
                    .NewDocument(currentRequestUri)
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

        [HttpGet("blogs/{id}")]
        public Document Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Blog by identifier from repository
            /////////////////////////////////////////////////////
            var blog = BloggingRepository.GetBlog(Convert.ToInt64(id));

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUri = this.Request.GetUri();
            using (var documentContext = new BloggingDocumentContext(currentRequestUri))
            {
                var document = documentContext
                    .NewDocument(currentRequestUri)
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

        [HttpGet("blogs/{id}/articles")]
        public Document GetBlogToArticles(string id)
        {
            /////////////////////////////////////////////////////
            // Get Blog to related Articles by Blog identifier from repository
            /////////////////////////////////////////////////////
            var blogToArticles = BloggingRepository.GetBlogToArticles(Convert.ToInt64(id));

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUri = this.Request.GetUri();
            using (var documentContext = new BloggingDocumentContext(currentRequestUri))
            {
                var document = documentContext
                    .NewDocument(currentRequestUri)
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

        [HttpPost("blogs")]
        public Document Post([FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("blogs/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("blogs/{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
