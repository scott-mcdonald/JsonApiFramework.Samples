namespace Blogging.WebService.Framework.Internal
{
    /// <summary>
    /// JsonApi API hypermedia options.
    /// Needed by the JsonApiFramework to understand how the hypermedia is configured for this application by properly creating a global URL configuration based on these options.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class ApiHypermediaOptions
    {
        #region Public Properties
        public string Scheme { get; set; }
        public string Host   { get; set; }
        public int    Port   { get; set; }
        #endregion
    }
}