using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Blogging.WebService.ErrorHandling
{
    /// <summary>Represents the requested resource of an API could not be found. Should be mapped to a 404 HTTP response.</summary>
    public class ApiNotFoundException : ApiException
    {
        // PUBLIC CONSTRUCTORS //////////////////////////////////////////////
        #region Constructors
        public ApiNotFoundException(string resourceType, string resourceId)
            : base(ApiErrorCodes.API_RESOURCE_NOT_FOUND_ERROR)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(resourceId != null);

            this.ResourceType = resourceType;
            this.ResourceId   = resourceId;
        }

        public ApiNotFoundException(string resourceType, string resourceId, string message)
            : base(ApiErrorCodes.API_RESOURCE_NOT_FOUND_ERROR, message)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(resourceId != null);

            this.ResourceType = resourceType;
            this.ResourceId   = resourceId;
        }

        public ApiNotFoundException(string resourceType, string resourceId, string message, Exception innerException)
            : base(ApiErrorCodes.API_RESOURCE_NOT_FOUND_ERROR, message, innerException)
        {
            Contract.Requires(resourceType != null);
            Contract.Requires(resourceId != null);

            this.ResourceType = resourceType;
            this.ResourceId   = resourceId;
        }
        #endregion

        // PUBLIC PROPERTIES ////////////////////////////////////////////////
        #region Properties
        public string ResourceType { get; }

        public string ResourceId { get; }
        #endregion

        // PROTECTED CONSTRUCTORS ///////////////////////////////////////////
        #region Constructors
        protected ApiNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
