using System.Web.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Blogging.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            WebApiConfig.ConfigureFormatters(config);
            WebApiConfig.ConfigureWebApiRouting(config);

            config.EnsureInitialized();
        }

        private static void ConfigureFormatters(HttpConfiguration config)
        {
            // Remove XML formatter.
            // This means when we visit an endpoint from a browser, instead of returning XML, it will return JSON.
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Configure JSON formatter.
            var jsonFormatter = config.Formatters.JsonFormatter;

            jsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter
                {
                    AllowIntegerValues = true
                });
        }

        private static void ConfigureWebApiRouting(HttpConfiguration config)
        {
            // Use attribute based routing.
            config.MapHttpAttributeRoutes();
        }
    }
}
