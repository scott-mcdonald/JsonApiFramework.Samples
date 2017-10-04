using System;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Blogging.WebService.Controllers
{
    public class PeopleController : Controller
    {
        #region WebApi Methods
        [HttpGet("people")]
        public Document GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all People from repository
            /////////////////////////////////////////////////////
            var people = BloggingRepository.GetPeople();

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
                        .ResourceCollection(people)
                            .Relationships()
                                .AddRelationship("articles", new[] { Keywords.Related })
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

        [HttpGet("people/{id}")]
        public Document Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person by identifier from repository
            /////////////////////////////////////////////////////
            var person = BloggingRepository.GetPerson(Convert.ToInt64(id));

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
                        .Resource(person)
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

        [HttpGet("people/{id}/articles")]
        public Document GetPersonToArticles(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person to related Articles by Author identifier from repository
            /////////////////////////////////////////////////////
            var personToArticles = BloggingRepository.GetPersonToArticles(Convert.ToInt64(id));

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
                        .ResourceCollection(personToArticles)
                            .Relationships()
                                .AddRelationship("blog", new[] { Keywords.Related })
                                .AddRelationship("author", new[] { Keywords.Related })
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

        [HttpGet("people/{id}/comments")]
        public Document GetPersonToComments(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person to related Comments by Author identifier from repository
            /////////////////////////////////////////////////////
            var personToComments = BloggingRepository.GetPersonToComments(Convert.ToInt64(id));

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
                        .ResourceCollection(personToComments)
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

        [HttpPost("people")]
        public Document Post([FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("people/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("people/{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}