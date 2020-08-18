using System.Diagnostics.Contracts;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

namespace Blogging.WebService.Framework
{
    /// <summary>Extensions methods for the <c>IApiServiceContext</c> contract.</summary>
    public static class ApiServiceContextExtensions
    {
        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region Extension Methods
        public static DocumentContext CreateApiDocumentContext(this IApiServiceContext apiServiceContext)
        {
            Contract.Requires(apiServiceContext != null);

            var documentContextOptions = apiServiceContext.ApiDocumentContextOptions;
            var documentContext        = new DocumentContext(documentContextOptions);
            return documentContext;
        }

        public static DocumentContext CreateApiDocumentContext(this IApiServiceContext apiServiceContext, Document document)
        {
            Contract.Requires(apiServiceContext != null);
            Contract.Requires(document != null);

            var documentContextOptions = apiServiceContext.ApiDocumentContextOptions;
            var documentContext        = new DocumentContext(documentContextOptions, document);
            return documentContext;
        }
        #endregion
    }
}