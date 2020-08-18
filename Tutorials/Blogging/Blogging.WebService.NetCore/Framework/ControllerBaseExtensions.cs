using System;

using Microsoft.AspNetCore.Mvc;

namespace Blogging.WebService.Framework
{
    public static class ControllerBaseExtensions
    {
        #region Extension Methods
        public static Uri GetCurrentRequestUri(this ControllerBase controller)
        {
            var currentRequest = controller.Request;

            var currentRequestUriBuilder = new UriBuilder
            {
                Scheme = currentRequest.Scheme,
                Host   = currentRequest.Host.Host,
                Port   = currentRequest.Host.Port.GetValueOrDefault(80),
                Path   = currentRequest.Path.Value
            };

            var currentRequestUri = currentRequestUriBuilder.Uri;
            return currentRequestUri;
        }
        #endregion
    }
}