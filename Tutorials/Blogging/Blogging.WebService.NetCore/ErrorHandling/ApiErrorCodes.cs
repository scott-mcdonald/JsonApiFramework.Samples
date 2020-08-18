namespace Blogging.WebService.ErrorHandling
{
    /// <summary>API error codes.</summary>
    public static class ApiErrorCodes
    {
        #region Error Codes
        public const string API_INTERNAL_ERROR            = "API_INTERNAL_ERROR";
        public const string API_RESOURCE_CONFLICT_ERROR   = "API_RESOURCE_CONFLICT_ERROR";
        public const string API_RESOURCE_DATABASE_ERROR   = "API_RESOURCE_DATABASE_ERROR";
        public const string API_RESOURCE_NOT_FOUND_ERROR  = "API_RESOURCE_NOT_FOUND_ERROR";
        public const string API_RESOURCE_VALIDATION_ERROR = "API_RESOURCE_VALIDATION_ERROR";
        #endregion
    }
}