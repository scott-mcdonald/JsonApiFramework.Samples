using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Blogging.WebService.ErrorHandling
{
    /// <summary>Standard exception for the tutorial to ensure all API errors have a unique identifier and standard error code.</summary>
    public class ApiException : Exception
    {
        // PUBLIC CONSTRUCTORS //////////////////////////////////////////////
        #region Constructors
        public ApiException(string code, string message)
            : this(code, message, null)
        {
        }

        public ApiException(string code, string message = null, Exception innerException = null)
            : base(message, innerException)
        {
            Contract.Requires(code != null);

            this.Id = Guid.NewGuid();

            this.Code = code;
        }
        #endregion

        // PUBLIC PROPERTIES ////////////////////////////////////////////////
        #region Properties
        public Guid Id { get; }

        public string Code { get; }
        #endregion

        // PROTECTED CONSTRUCTORS ///////////////////////////////////////////
        #region Constructors
        protected ApiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
