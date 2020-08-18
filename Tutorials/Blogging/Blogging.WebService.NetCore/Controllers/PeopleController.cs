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
    public class PeopleController : ApiControllerBase
    {
        #region Constructors
        public PeopleController(IApiServiceContext apiServiceContext, BloggingRepository bloggingRepository)
            : base(apiServiceContext, bloggingRepository)
        {
        }
        #endregion

        #region WebApi Methods
        [HttpGet("people")]
        public IActionResult GetCollection()
        {
            /////////////////////////////////////////////////////
            // Get all People from repository
            /////////////////////////////////////////////////////
            var people = this.BloggingRepository.GetPeople();

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

            return this.Ok(document);
        }

        [HttpGet("people/{id}")]
        public IActionResult Get(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person by identifier from repository
            /////////////////////////////////////////////////////
            var person = this.BloggingRepository.GetPerson(Convert.ToInt64(id));

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

            return this.Ok(document);
        }

        [HttpGet("people/{id}/articles")]
        public IActionResult GetPersonToArticles(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person to related Articles by Author identifier from repository
            /////////////////////////////////////////////////////
            var personToArticles = this.BloggingRepository.GetPersonToArticles(Convert.ToInt64(id));

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

            return this.Ok(document);
        }

        [HttpGet("people/{id}/comments")]
        public IActionResult GetPersonToComments(string id)
        {
            /////////////////////////////////////////////////////
            // Get Person to related Comments by Author identifier from repository
            /////////////////////////////////////////////////////
            var personToComments = this.BloggingRepository.GetPersonToComments(Convert.ToInt64(id));

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

            return this.Ok(document);
        }

        [HttpPost("people")]
        public IActionResult Post([FromBody]Document inDocument)
        {
            var person = this.CreatePerson(inDocument);
            var (document, link) = this.CreateDocumentAndLink(person);

            return this.Created(link, document);
        }

        [HttpPatch("people/{id}")]
        public IActionResult Patch(string id, [FromBody]Document inDocument)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("people/{id}")]
        public IActionResult Delete(string id)
        {
            this.DeletePerson(id);

            return this.NoContent();
        }
        #endregion

        #region Private Methods
        private Person CreatePerson(Document inDocument)
        {
            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext(inDocument);
            var inPerson  = documentContext.GetResource<Person>();

            var validator = new PersonValidator();
            validator.ValidateAndThrow(inPerson);

            var outPerson = this.BloggingRepository.CreatePerson(inPerson);
            return outPerson;
        }

        private (Document Document, Link Link) CreateDocumentAndLink(Person person)
        {
            using var documentContext   = this.ApiServiceContext.CreateApiDocumentContext();

            var currentRequestUri = this.GetCurrentRequestUri();
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

            var link = document.GetResource().Links.Self;

            return (document, link);
        }

        private void DeletePerson(string id)
        {
            var personId = Convert.ToInt64(id);
            this.BloggingRepository.DeletePerson(personId);
        }
        #endregion
    }
}