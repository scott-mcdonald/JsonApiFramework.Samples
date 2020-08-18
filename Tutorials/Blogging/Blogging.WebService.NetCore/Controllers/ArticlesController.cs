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
    public class ArticlesController : ApiControllerBase
    {
        #region Constructors
        public ArticlesController(IApiServiceContext apiServiceContext, BloggingRepository bloggingRepository)
            : base(apiServiceContext, bloggingRepository)
        {
        }
        #endregion

        #region WebApi Methods
        [HttpGet("articles")]
        public IActionResult GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Articles from repository
            /////////////////////////////////////////////////////
            var articles = this.BloggingRepository.GetArticles().SafeToList();

            var articleToBlogIncludedResourceCollection = articles
                .Select(x => ToOneIncludedResource.Create(x, "blog", this.BloggingRepository.GetArticleToBlog(x.ArticleId)))
                .ToList();

            var articleToAuthorIncludedResourceCollection = articles
                .Select(x => ToOneIncludedResource.Create(x, "author", this.BloggingRepository.GetArticleToAuthor(x.ArticleId)))
                .ToList();

            var articleToCommentsIncludedResourcesCollection = articles
                .Select(x => ToManyIncludedResources.Create(x, "comments", this.BloggingRepository.GetArticleToComments(x.ArticleId)))
                .ToList();

            // Get all distinct comments used in all the articles.
            var comments = articles
                .SelectMany(x => this.BloggingRepository.GetArticleToComments(x.ArticleId))
                .GroupBy(x => x.CommentId)
                .Select(x => x.First())
                .ToList();

            var commentToAuthorIncludedResourceCollection = comments
                .Select(x => ToOneIncludedResource.Create(x, "author", this.BloggingRepository.GetCommentToAuthor(x.CommentId)))
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
                    .ResourceCollection(articles)
                        .Relationships()
                            .AddRelationship("blog", new[] { Keywords.Related })
                            .AddRelationship("author", new[] { Keywords.Related })
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

        [HttpGet("articles/{id}")]
        public IActionResult Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article by identifier from repository
            /////////////////////////////////////////////////////
            var article = this.BloggingRepository.GetArticle(Convert.ToInt64(id));

            var articleToBlogIncludedResource = ToOneIncludedResource.Create(article, "blog", this.BloggingRepository.GetArticleToBlog(article.ArticleId));
            var articleToAuthorIncludedResource = ToOneIncludedResource.Create(article, "author", this.BloggingRepository.GetArticleToAuthor(article.ArticleId));

            var comments = this.BloggingRepository.GetArticleToComments(article.ArticleId);

            var articleToCommentsIncludedResources = ToManyIncludedResources.Create(article, "comments", this.BloggingRepository.GetArticleToComments(article.ArticleId));

            var commentToAuthorIncludedResourceCollection = comments
                .Select(x => ToOneIncludedResource.Create(x, "author", this.BloggingRepository.GetCommentToAuthor(x.CommentId)))
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
                    .Resource(article)
                        .Relationships()
                            .AddRelationship("blog", new[] { Keywords.Related })
                            .AddRelationship("author", new[] { Keywords.Related })
                            .AddRelationship("comments", new[] { Keywords.Related })
                        .RelationshipsEnd()
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                    .ResourceEnd()
                    .Included()
                        // article => blog (to-one)
                        .Include(articleToBlogIncludedResource)
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .IncludeEnd()

                        // article => author (to-one)
                        .Include(articleToAuthorIncludedResource)
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .IncludeEnd()

                        // article => comments (to-many)
                        .Include(articleToCommentsIncludedResources)
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

        [HttpGet("articles/{id}/blog")]
        public IActionResult GetArticleToBlog(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Blog by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToBlog = this.BloggingRepository.GetArticleToBlog(Convert.ToInt64(id));

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
                    .Resource(articleToBlog)
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

        [HttpGet("articles/{id}/author")]
        public IActionResult GetArticleToAuthor(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Author by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToAuthor = this.BloggingRepository.GetArticleToAuthor(Convert.ToInt64(id));

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
                    .Resource(articleToAuthor)
                        .Relationships()
                            .AddRelationship("articles", new[] { Keywords.Related })
                            .AddRelationship("comments", new[] { Keywords.Related })
                        .RelationshipsEnd()
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                    .ResourceEnd()
                .WriteDocument();

            return this.Ok(document);
        }

        [HttpGet("articles/{id}/comments")]
        public IActionResult GetArticleToComments(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Comments by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToComments = this.BloggingRepository.GetArticleToComments(Convert.ToInt64(id));

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
                    .ResourceCollection(articleToComments)
                        .Relationships()
                            .AddRelationship("article", new[] { Keywords.Related })
                            .AddRelationship("author", new[] { Keywords.Related })
                        .RelationshipsEnd()
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                    .ResourceCollectionEnd()
                .WriteDocument();

            return this.Ok(document);
        }

        [HttpPost("articles")]
        public IActionResult Post([FromBody]Document inDocument)
        {
            var article = this.CreateArticle(inDocument);
            var (document, link) = this.CreateDocumentAndLink(article);

            return this.Created(link, document);
        }

        [HttpPatch("articles/{id}")]
        public IActionResult Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("articles/{id}")]
        public IActionResult Delete(string id)
        {
            this.DeleteArticle(id);

            return this.NoContent();
        }
        #endregion

        #region Private Methods
        private Article CreateArticle(Document inDocument)
        {
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext(inDocument);
            var inArticle = documentContext.GetResource<Article>();

            var apiArticleResource = inDocument.GetResource();
            if (apiArticleResource?.Relationships != null)
            {
                // Author foreign key (required)
                if (apiArticleResource.Relationships.TryGetRelationship("author", out var apiArticleAuthorRelationship))
                {
                    if (apiArticleAuthorRelationship.IsToOneRelationship())
                    {
                        var apiArticleAuthorToOneRelationship = (ToOneRelationship)apiArticleAuthorRelationship;
                        var apiArticleAuthorId                = Convert.ToInt64(apiArticleAuthorToOneRelationship.Data.Id);
                        inArticle.AuthorId = apiArticleAuthorId;
                    }
                }

                // Blog foreign key (required)
                if (apiArticleResource.Relationships.TryGetRelationship("blog", out var apiArticleBlogRelationship))
                {
                    if (apiArticleBlogRelationship.IsToOneRelationship())
                    {
                        var apiArticleBlogToOneRelationship = (ToOneRelationship)apiArticleBlogRelationship;
                        var apiArticleBlogId                = Convert.ToInt64(apiArticleBlogToOneRelationship.Data.Id);
                        inArticle.BlogId = apiArticleBlogId;
                    }
                }
            }

            var validator = new ArticleValidator();
            validator.ValidateAndThrow(inArticle);

            var outArticle = this.BloggingRepository.CreateArticle(inArticle);
            return outArticle;
        }

        private (Document Document, Link Link) CreateDocumentAndLink(Article article)
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
                    .Resource(article)
                        .Relationships()
                            .AddRelationship("blog",     new[] { Keywords.Related })
                            .AddRelationship("author",   new[] { Keywords.Related })
                            .AddRelationship("comments", new[] { Keywords.Related })
                        .RelationshipsEnd()
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                    .ResourceEnd()
                .WriteDocument();

            var link = document.GetResource().Links.Self;

            return (document, link);
        }

        private void DeleteArticle(string id)
        {
            var articleId = Convert.ToInt64(id);
            this.BloggingRepository.DeleteArticle(articleId);
        }
        #endregion
    }
}