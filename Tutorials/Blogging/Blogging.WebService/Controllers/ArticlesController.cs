using System;
using System.Web;
using System.Web.Http;

using JsonApiFramework.Http;
using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService.Controllers
{
    public class ArticlesController : ApiController
    {
        [Route("articles")]
        public Document GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Articles from repository
            /////////////////////////////////////////////////////
            var articles = BloggingRepository.GetArticles();

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
                        .ResourceCollection(articles)
                            .Relationships()
                                .AddRelationship("blog", Keywords.Related)
                                .AddRelationship("author", Keywords.Related)
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

        [Route("articles/{id}")]
        public Document Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article by identifier from repository
            /////////////////////////////////////////////////////
            var article = BloggingRepository.GetArticle(Convert.ToInt64(id));

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
                        .Resource(article)
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

        [Route("articles/{id}/blog")]
        public Document GetArticleToBlog(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Blog by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToBlog = BloggingRepository.GetArticleToBlog(Convert.ToInt64(id));

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
                        .Resource(articleToBlog)
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

        [Route("articles/{id}/author")]
        public Document GetArticleToAuthor(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Author by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToAuthor = BloggingRepository.GetArticleToAuthor(Convert.ToInt64(id));

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
                        .Resource(articleToAuthor)
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

        [Route("articles/{id}/comments")]
        public Document GetArticleToComments(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Comments by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToComments = BloggingRepository.GetArticleToComments(Convert.ToInt64(id));

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
                        .ResourceCollection(articleToComments)
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
    }
}