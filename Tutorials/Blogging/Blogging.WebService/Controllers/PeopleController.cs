using System;
using System.Web;
using System.Web.Http;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService.Controllers
{
    public class PeopleController : ApiController
    {
        #region WebApi Methods
        [Route("people")]
        public Document GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all People from repository
            /////////////////////////////////////////////////////
            var people = BloggingRepository.GetPeople();

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
                        .ResourceCollection(people)
                            .Relationships()
                                .AddRelationship("articles", Keywords.Related)
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

        [Route("people/{id}")]
        public Document Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person by identifier from repository
            /////////////////////////////////////////////////////
            var person = BloggingRepository.GetPerson(Convert.ToInt64(id));

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
                        .Resource(person)
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

        [Route("people/{id}/articles")]
        public Document GetPersonToArticles(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person to related Articles by Author identifier from repository
            /////////////////////////////////////////////////////
            var personToArticles = BloggingRepository.GetPersonToArticles(Convert.ToInt64(id));

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
                        .ResourceCollection(personToArticles)
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

        [Route("people/{id}/comments")]
        public Document GetPersonToComments(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person to related Comments by Author identifier from repository
            /////////////////////////////////////////////////////
            var personToComments = BloggingRepository.GetPersonToComments(Convert.ToInt64(id));

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
                        .ResourceCollection(personToComments)
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

        [Route("people")]
        public Document Post([FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [Route("people/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [Route("people/{id}")]
        public void Delete(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}