﻿using System;
using System.Linq;

using JsonApiFramework;
using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Blogging.WebService.Controllers
{
    public class ArticlesController : Controller
    {
        #region WebApi Methods
        [HttpGet("articles")]
        public Document GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all Articles from repository
            /////////////////////////////////////////////////////
            var articles = BloggingRepository.GetArticles().SafeToList();

            var articleToBlogIncludedResourceCollection = articles
                .Select(x => ToOneIncludedResource.Create(x, "blog", BloggingRepository.GetArticleToBlog(x.ArticleId)))
                .ToList();

            var articleToAuthorIncludedResourceCollection = articles
                .Select(x => ToOneIncludedResource.Create(x, "author", BloggingRepository.GetArticleToAuthor(x.ArticleId)))
                .ToList();

            var articleToCommentsIncludedResourcesCollection = articles
                .Select(x => ToManyIncludedResources.Create(x, "comments", BloggingRepository.GetArticleToComments(x.ArticleId)))
                .ToList();

            // Get all distinct comments used in all the articles.
            var comments = articles
                .SelectMany(x => BloggingRepository.GetArticleToComments(x.ArticleId))
                .GroupBy(x => x.CommentId)
                .Select(x => x.First())
                .ToList();

            var commentToAuthorIncludedResourceCollection = comments
                .Select(x => ToOneIncludedResource.Create(x, "author", BloggingRepository.GetCommentToAuthor(x.CommentId)))
                .ToList();

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

                return document;
            }
        }

        [HttpGet("articles/{id}")]
        public Document Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article by identifier from repository
            /////////////////////////////////////////////////////
            var article = BloggingRepository.GetArticle(Convert.ToInt64(id));

            var articleToBlogIncludedResource = ToOneIncludedResource.Create(article, "blog", BloggingRepository.GetArticleToBlog(article.ArticleId));
            var articleToAuthorIncludedResource = ToOneIncludedResource.Create(article, "author", BloggingRepository.GetArticleToAuthor(article.ArticleId));

            var comments = BloggingRepository.GetArticleToComments(article.ArticleId);

            var articleToCommentsIncludedResources = ToManyIncludedResources.Create(article, "comments", BloggingRepository.GetArticleToComments(article.ArticleId));

            var commentToAuthorIncludedResourceCollection = comments
                .Select(x => ToOneIncludedResource.Create(x, "author", BloggingRepository.GetCommentToAuthor(x.CommentId)))
                .ToList();

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

                return document;
            }
        }

        [HttpGet("articles/{id}/blog")]
        public Document GetArticleToBlog(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Blog by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToBlog = BloggingRepository.GetArticleToBlog(Convert.ToInt64(id));

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
                        .Resource(articleToBlog)
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

        [HttpGet("articles/{id}/author")]
        public Document GetArticleToAuthor(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Author by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToAuthor = BloggingRepository.GetArticleToAuthor(Convert.ToInt64(id));

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

                return document;
            }
        }

        [HttpGet("articles/{id}/comments")]
        public Document GetArticleToComments(string id)
        {
            /////////////////////////////////////////////////////
            // Get Article to related Comments by Article identifier from repository
            /////////////////////////////////////////////////////
            var articleToComments = BloggingRepository.GetArticleToComments(Convert.ToInt64(id));

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

                return document;
            }
        }

        [HttpPost("articles")]
        public Document Post([FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("articles/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("articles/{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}