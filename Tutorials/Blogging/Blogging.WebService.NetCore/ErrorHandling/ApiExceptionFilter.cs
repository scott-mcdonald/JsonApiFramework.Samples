using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net;

using Blogging.WebService.Framework;

using FluentValidation;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

namespace Blogging.WebService.ErrorHandling
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        // PUBLIC CONSTRUCTORS //////////////////////////////////////////////
        #region Constructors
        public ApiExceptionFilter(IApiServiceContext apiServiceContext, IHttpContextAccessor httpContextAccessor)
        {
            Contract.Requires(apiServiceContext != null);
            Contract.Requires(httpContextAccessor != null);

            this.ApiServiceContext   = apiServiceContext;
            this.HttpContextAccessor = httpContextAccessor;
        }
        #endregion

        // PUBLIC METHODS ///////////////////////////////////////////////////
        #region ExceptionFilterAttribute Overrides
        public override void OnException(ExceptionContext context)
        {
            Contract.Requires(context != null);

            var exception            = context.Exception;
            var errorsDocumentResult = this.CreateErrorsDocumentResult(exception);
            var statusCode           = errorsDocumentResult.Item1;
            var errorsDocument       = errorsDocumentResult.Item2;

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result                          = new JsonResult(errorsDocument);

            base.OnException(context);
        }
        #endregion

        // PRIVATE PROPERTIES ///////////////////////////////////////////////
        #region Properties
        private IApiServiceContext ApiServiceContext { get; }

        private IHttpContextAccessor HttpContextAccessor { get; }
        #endregion

        // PRIVATE METHODS //////////////////////////////////////////////////
        #region Methods
        private Tuple<int, Document> CreateErrorsDocumentResult(Exception exception)
        {
            Contract.Requires(exception != null);

            var exceptionType = exception.GetType();
            var statusCode    = HttpStatusCode.InternalServerError;
            var errorCode     = ApiErrorCodes.API_INTERNAL_ERROR;
            var errorTitle    = exceptionType.Name;
            var errorMessage  = exception.Message;

            if (exceptionType == ApiBadRequestExceptionType)
            {
                statusCode = HttpStatusCode.BadRequest;
                var apiBadRequestException = (ApiBadRequestException)exception;
                errorCode = apiBadRequestException.Code;
            }
            else if (exceptionType == ApiNotFoundExceptionType)
            {
                statusCode = HttpStatusCode.NotFound;
                var apiNotFoundException = (ApiNotFoundException)exception;

                errorCode = apiNotFoundException.Code;

                var clrResourceType = apiNotFoundException.ResourceType;
                var apiResourceId   = apiNotFoundException.ResourceId ?? "null";
                var apiServiceModel = this.ApiServiceContext.ApiServiceModel;
                if (apiServiceModel.TryGetResourceType(clrResourceType, out var resourceType))
                {
                    var apiResourceType = resourceType.ResourceIdentityInfo.ApiType;
                    var defaultMessage  = $"API resource [type={apiResourceType} id={apiResourceId}] not found in the system.";

                    errorMessage = defaultMessage;
                }
            }
            else if (exceptionType == DbUpdateExceptionType)
            {
                statusCode   = HttpStatusCode.BadRequest;
                errorCode    = ApiErrorCodes.API_RESOURCE_DATABASE_ERROR;
                errorMessage = "Database update exception has occurred, correct mistake and try again.";
            }
            else if (exceptionType == DbUpdateConcurrencyExceptionType)
            {
                statusCode   = HttpStatusCode.Conflict;
                errorCode    = ApiErrorCodes.API_RESOURCE_CONFLICT_ERROR;
                errorMessage = "Database concurrency conflict has occurred, get latest version and try again.";
            }
            else if (exceptionType == NotImplementedExceptionType)
            {
                statusCode   = HttpStatusCode.NotImplemented;
                errorMessage = "API is not implemented.";
            }
            else if (exceptionType == ValidationExceptionType)
            {
                statusCode   = HttpStatusCode.BadRequest;
                errorCode    = ApiErrorCodes.API_RESOURCE_VALIDATION_ERROR;
                errorMessage = exception.Message;
            }
            else if (exceptionType == ApiExceptionType)
            {
                var apiException = (ApiException)exception;
                errorCode = Convert.ToString(apiException.Code);
            }

            var apiDocument = this.CreateErrorsDocument(exception,
                                                        statusCode,
                                                        errorTitle,
                                                        errorMessage,
                                                        errorCode);
            var result = new Tuple<int, Document>((int)statusCode, apiDocument);
            return result;
        }

        private Document CreateErrorsDocument(Exception      exception,
                                              HttpStatusCode statusCode,
                                              string         errorTitle,
                                              string         errorMessage,
                                              string         errorCode)
        {
            Contract.Requires(exception != null);
            Contract.Requires(string.IsNullOrWhiteSpace(errorTitle) == false);
            Contract.Requires(string.IsNullOrWhiteSpace(errorMessage) == false);

            var error = new Error
            {
                Id     = Guid.NewGuid().ToString(),
                Status = ((int)statusCode).ToString(CultureInfo.InvariantCulture),
                Title  = errorTitle,
                Detail = errorMessage,
                Code   = errorCode,
                Meta   = CreateErrorMeta(exception)
            };

            using var documentContext = this.ApiServiceContext.CreateApiDocumentContext();

            var currentRequestUrl = this.HttpContextAccessor.HttpContext.Request.GetEncodedUrl();

            var apiDocument = documentContext
                .NewDocument(currentRequestUrl)
                    .SetJsonApiVersion(JsonApiVersion.Version10)
                    .Links()
                        .AddSelfLink()
                    .LinksEnd()
                    .Errors()
                        .AddError(error)
                    .ErrorsEnd()
                .WriteDocument();

            return apiDocument;
        }

        private static JObject CreateErrorMeta(Exception exception)
        {
            Contract.Requires(exception != null);

            var stackTrace = exception.StackTrace;

            if (String.IsNullOrWhiteSpace(stackTrace))
                return null;

            var meta = new JObject {["stacktrace"] = stackTrace};
            return meta;
        }
        #endregion

        // PRIVATE FIELDS ///////////////////////////////////////////////////
        #region Fields
        private static readonly Type ApiExceptionType                 = typeof(ApiException);
        private static readonly Type ApiBadRequestExceptionType       = typeof(ApiBadRequestException);
        private static readonly Type ApiNotFoundExceptionType         = typeof(ApiNotFoundException);
        private static readonly Type DbUpdateExceptionType            = typeof(DbUpdateException);
        private static readonly Type DbUpdateConcurrencyExceptionType = typeof(DbUpdateConcurrencyException);
        private static readonly Type NotImplementedExceptionType      = typeof(NotImplementedException);
        private static readonly Type ValidationExceptionType          = typeof(ValidationException);
        #endregion
    }
}