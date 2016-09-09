using System.Web.Http;

using JsonApiFramework.JsonApi;

namespace Blogging.WebService.Controllers
{
    public class BlogsController : ApiController
    {
        [Route("blogs")]
        public Document GetCollectionAsync()
        {
            var document = new Document
                {
                    JsonApiVersion = JsonApiVersion.Version10
                };
            return document;
        }

        [Route("blogs/{id}")]
        public Document GetAsync(string id)
        {
            var document = new Document
                {
                    JsonApiVersion = JsonApiVersion.Version10
                };
            return document;
        }

        [Route("blogs")]
        public Document Post([FromBody]Document inDocument)
        {
            var outDocument = new Document
                {
                    JsonApiVersion = JsonApiVersion.Version10
                };
            return outDocument;
        }

        [Route("blogs/{id}")]
        public Document Patch(string id, [FromBody]Document inDocument)
        {
            var outDocument = new Document
                {
                    JsonApiVersion = JsonApiVersion.Version10
                };
            return outDocument;
        }

        [Route("blogs/{id}")]
        public void Delete(string id)
        {
        }
    }
}
