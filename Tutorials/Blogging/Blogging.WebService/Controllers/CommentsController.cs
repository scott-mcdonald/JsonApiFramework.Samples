using System;
using System.Web;
using System.Web.Http;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService.Controllers
{
    public class CommentsController : ApiController
    {
        #region WebApi Methods
        [Route("comments")]
        public Document GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Comments from repository
            /////////////////////////////////////////////////////
            var comments = BloggingRepository.GetComments();

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
                        .ResourceCollection(comments)
                            .Relationships()
                                .AddRelationship("article", Keywords.Related)
                                .AddRelationship("author", Keywords.Related)
                            .RelationshipsEnd()
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .ResourceCollectionEnd()
                    .WriteDocument();

                return document;
            }
        }

        [Route("comments/{id}")]
        public Document Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Comment by identifier from repository
            /////////////////////////////////////////////////////
            var comment = BloggingRepository.GetComment(Convert.ToInt64(id));

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
                        .Resource(comment)
                            .Relationships()
                                .AddRelationship("article", Keywords.Related)
                                .AddRelationship("author", Keywords.Related)
                            .RelationshipsEnd()
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .ResourceEnd()
                    .WriteDocument();

                return document;
            }
        }

        [Route("comments/{id}/article")]
        public Document GetCommentToArticle(string id)
        {
            /////////////////////////////////////////////////////
            // Get Comment to related Article by Comment identifier from repository
            /////////////////////////////////////////////////////
            var commentToArticle = BloggingRepository.GetCommentToArticle(Convert.ToInt64(id));

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
                        .Resource(commentToArticle)
                            .Relationships()
                                .AddRelationship("blog", Keywords.Related)
                                .AddRelationship("author", Keywords.Related)
                                .AddRelationship("comments", Keywords.Related)
                            .RelationshipsEnd()
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .ResourceEnd()
                    .WriteDocument();

                return document;
            }
        }

        [Route("comments/{id}/author")]
        public Document GetCommentToAuthor(string id)
        {
            /////////////////////////////////////////////////////
            // Get Comment to related Author by Comment identifier from repository
            /////////////////////////////////////////////////////
            var commentToAuthor = BloggingRepository.GetCommentToAuthor(Convert.ToInt64(id));

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
                        .Resource(commentToAuthor)
                            .Relationships()
                                .AddRelationship("articles", Keywords.Related)
                                .AddRelationship("comments", Keywords.Related)
                            .RelationshipsEnd()
                            .Links()
                                .AddSelfLink()
                            .LinksEnd()
                        .ResourceEnd()
                    .WriteDocument();

                return document;
            }
        }

        [Route("comments")]
        public Document Post([FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [Route("comments/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [Route("comments/{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}