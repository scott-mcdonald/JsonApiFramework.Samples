using System;
using System.Runtime.Serialization;

namespace Blogging.WebService.ErrorHandling
{
    /// <summary>Represents the client of an API has made a bad request. Should be mapped to a 400 HTTP response.</summary>
    public class ApiBadRequestException : ApiException
    {
        // PUBLIC CONSTRUCTORS //////////////////////////////////////////////
        #region Constructors
        public ApiBadRequestException(string code)
            : base(code)
        {
        }

        public ApiBadRequestException(string code, string message)
            : base(code, message)
        {
        }

        public ApiBadRequestException(string code, string message, Exception innerException)
            : base(code, message, innerException)
        {
        }
        #endregion

        // PROTECTED CONSTRUCTORS ///////////////////////////////////////////
        #region Constructors
        protected ApiBadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
