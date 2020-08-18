using System;
using System.Linq;

using Blogging.ServiceModel;
using Blogging.WebService.Framework;
using Blogging.WebService.Validation;

using FluentValidation;

using JsonApiFramework;
using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

using Microsoft.AspNetCore.Mvc;

namespace Blogging.WebService.Controllers
{
    [ApiController]
    [Route("")]
    public class BlogsController : ApiControllerBase
    {
        #region Constructors
        public BlogsController(IApiServiceContext apiServiceContext, BloggingRepository bloggingRepository)
            : base(apiServiceContext, bloggingRepository)
        {
        }
        #endregion

        #region WebApi Methods
        [HttpGet("blogs")]
        public IActionResult GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Blogs from repository
            /////////////////////////////////////////////////////
            var blogs = this.BloggingRepository.GetBlogs();

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUri = this.GetCurrentRequestUri();
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext();

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

            return this.Ok(document);
        }

        [HttpGet("blogs/{id}")]
        public IActionResult Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Blog by identifier from repository
            /////////////////////////////////////////////////////
            var blog = this.BloggingRepository.GetBlog(Convert.ToInt64(id));

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUri = this.GetCurrentRequestUri();
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext();

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

            return this.Ok(document);
        }

        [HttpGet("blogs/{id}/articles")]
        public IActionResult GetBlogToArticles(string id)
        {
            /////////////////////////////////////////////////////
            // Get Blog to related Articles by Blog identifier from repository
            /////////////////////////////////////////////////////
            var blogToArticles = this.BloggingRepository.GetBlogToArticles(Convert.ToInt64(id)).ToList();

            var articleToBlogIncludedResourceCollection = blogToArticles.Select(x => ToOneIncludedResource.Create(x, "blog", this.BloggingRepository.GetArticleToBlog(x.ArticleId)))
                                                                        .ToList();

            var articleToAuthorIncludedResourceCollection = blogToArticles.Select(x => ToOneIncludedResource.Create(x, "author", this.BloggingRepository.GetArticleToAuthor(x.ArticleId)))
                                                                          .ToList();

            var articleToCommentsIncludedResourcesCollection = blogToArticles.Select(x => ToManyIncludedResources.Create(x, "comments", this.BloggingRepository.GetArticleToComments(x.ArticleId)))
                                                                             .ToList();

            // Get all distinct comments used in all the articles.
            var comments = blogToArticles.SelectMany(x => this.BloggingRepository.GetArticleToComments(x.ArticleId))
                                         .GroupBy(x => x.CommentId)
                                         .Select(x => x.First())
                                         .ToList();

            var commentToAuthorIncludedResourceCollection = comments.Select(x => ToOneIncludedResource.Create(x, "author", this.BloggingRepository.GetCommentToAuthor(x.CommentId)))
                                                                    .ToList();

            /////////////////////////////////////////////////////
            // Build JSON API document
            /////////////////////////////////////////////////////
            var currentRequestUri = this.GetCurrentRequestUri();
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext();

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
                    .Included()

                        // article => blog (to-one)
                        .Include(articleToBlogIncludedResourceCollection)
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .IncludeEnd()

                        // article => author (to-one)
                        .Include(articleToAuthorIncludedResourceCollection)
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .IncludeEnd()

                        // article => comments (to-many)
                        .Include(articleToCommentsIncludedResourcesCollection)
                            .Relationships()
                                .AddRelationship("author", new[] { Keywords.Related })
                            .RelationshipsEnd()
                            .Links()
                                .AddLink(Keywords.Self)
                            .LinksEnd()
                        .IncludeEnd()

                        // comment => author (to-one)
                        .Include(commentToAuthorIncludedResourceCollection)
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .IncludeEnd()

                    .IncludedEnd()
                .WriteDocument();

            return this.Ok(document);
        }

        [HttpPost("blogs")]
        public IActionResult Post([FromBody]Document inDocument)
        {
            var blog = this.CreateBlog(inDocument);
            var (document, link) = this.CreateDocumentAndLink(blog);

            return this.Created(link, document);
        }

        [HttpPatch("blogs/{id}")]
        public IActionResult Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("blogs/{id}")]
        public IActionResult Delete(string id)
        {
            this.DeleteBlog(id);

            return this.NoContent();
        }
        #endregion

        #region Private Methods
        private Blog CreateBlog(Document inDocument)
        {
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext(inDocument);
            var inBlog = documentContext.GetResource<Blog>();

            var validator = new BlogValidator();
            validator.ValidateAndThrow(inBlog);

            var outBlog = this.BloggingRepository.CreateBlog(inBlog);
            return outBlog;
        }

        private (Document Document, Link Link) CreateDocumentAndLink(Blog blog)
        {
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext();

            var currentRequestUri = this.GetCurrentRequestUri();
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

            var link = document.GetResource().Links.Self;

            return (document, link);
        }

        private void DeleteBlog(string id)
        {
            var blogId = Convert.ToInt64(id);
            this.BloggingRepository.DeleteBlog(blogId);
        }
        #endregion
    }
}
