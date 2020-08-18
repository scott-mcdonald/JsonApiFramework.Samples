using System;

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
    public class CommentsController : ApiControllerBase
    {
        #region Constructors
        public CommentsController(IApiServiceContext apiServiceContext, BloggingRepository bloggingRepository)
            : base(apiServiceContext, bloggingRepository)
        {
        }
        #endregion

        #region WebApi Methods
        [HttpGet("comments")]
        public IActionResult GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Comments from repository
            /////////////////////////////////////////////////////
            var comments = this.BloggingRepository.GetComments();

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
                    .ResourceCollection(comments)
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

        [HttpGet("comments/{id}")]
        public IActionResult Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Comment by identifier from repository
            /////////////////////////////////////////////////////
            var comment = this.BloggingRepository.GetComment(Convert.ToInt64(id));

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
                    .Resource(comment)
                        .Relationships()
                            .AddRelationship("article", new[] { Keywords.Related })
                            .AddRelationship("author", new[] { Keywords.Related })
                        .RelationshipsEnd()
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                    .ResourceEnd()
                .WriteDocument();

            return this.Ok(document);
        }

        [HttpGet("comments/{id}/article")]
        public IActionResult GetCommentToArticle(string id)
        {
            /////////////////////////////////////////////////////
            // Get Comment to related Article by Comment identifier from repository
            /////////////////////////////////////////////////////
            var commentToArticle = this.BloggingRepository.GetCommentToArticle(Convert.ToInt64(id));

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
                    .Resource(commentToArticle)
                        .Relationships()
                            .AddRelationship("blog", new[] { Keywords.Related })
                            .AddRelationship("author", new[] { Keywords.Related })
                            .AddRelationship("comments", new[] { Keywords.Related })
                        .RelationshipsEnd()
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                    .ResourceEnd()
                .WriteDocument();

            return this.Ok(document);
        }

        [HttpGet("comments/{id}/author")]
        public IActionResult GetCommentToAuthor(string id)
        {
            /////////////////////////////////////////////////////
            // Get Comment to related Author by Comment identifier from repository
            /////////////////////////////////////////////////////
            var commentToAuthor = this.BloggingRepository.GetCommentToAuthor(Convert.ToInt64(id));

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
                    .Resource(commentToAuthor)
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

        [HttpPost("comments")]
        public IActionResult Post([FromBody]Document inDocument)
        {
            var comment = this.CreateComment(inDocument);
            var (document, link) = this.CreateDocumentAndLink(comment);

            return this.Created(link, document);
        }

        [HttpPatch("comments/{id}")]
        public IActionResult Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("comments/{id}")]
        public IActionResult Delete(string id)
        {
            this.DeleteComment(id);

            return this.NoContent();
        }
        #endregion

        #region Private Methods
        private Comment CreateComment(Document inDocument)
        {
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext(inDocument);
            var inComment = documentContext.GetResource<Comment>();

            var apiCommentResource = inDocument.GetResource();
            if (apiCommentResource?.Relationships != null)
            {
                // Author foreign key (optional)
                if (apiCommentResource.Relationships.TryGetRelationship("author", out var apiCommentAuthorRelationship))
                {
                    if (apiCommentAuthorRelationship.IsToOneRelationship())
                    {
                        var apiCommentAuthorToOneRelationship = (ToOneRelationship)apiCommentAuthorRelationship;
                        var apiCommentAuthorId                = Convert.ToInt64(apiCommentAuthorToOneRelationship.Data.Id);
                        inComment.AuthorId = apiCommentAuthorId;
                    }
                }

                // Article foreign key (required)
                if (apiCommentResource.Relationships.TryGetRelationship("article", out var apiCommentArticleRelationship))
                {
                    if (apiCommentArticleRelationship.IsToOneRelationship())
                    {
                        var apiCommentArticleToOneRelationship = (ToOneRelationship)apiCommentArticleRelationship;
                        var apiCommentArticleId                = Convert.ToInt64(apiCommentArticleToOneRelationship.Data.Id);
                        inComment.ArticleId = apiCommentArticleId;
                    }
                }
            }

            var validator = new CommentValidator();
            validator.ValidateAndThrow(inComment);

            var outComment = this.BloggingRepository.CreateComment(inComment);
            return outComment;
        }

        private (Document Document, Link Link) CreateDocumentAndLink(Comment comment)
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
                    .Resource(comment)
                        .Relationships()
                            .AddRelationship("article", new[] { Keywords.Related })
                            .AddRelationship("author",  new[] { Keywords.Related })
                        .RelationshipsEnd()
                        .Links()
                            .AddSelfLink()
                        .LinksEnd()
                    .ResourceEnd()
                .WriteDocument();

            var link = document.GetResource().Links.Self;

            return (document, link);
        }

        private void DeleteComment(string id)
        {
            var commentId = Convert.ToInt64(id);
            this.BloggingRepository.DeleteComment(commentId);
        }
        #endregion
    }
}