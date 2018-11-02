using System;

using JsonApiFramework;
using JsonApiFramework.Http;
using JsonApiFramework.Server;
using JsonApiFramework.ServiceModel.Configuration;

namespace HelloWorld
{
    public class WorldsDocumentContext : DocumentContext
    {
        private static readonly UrlBuilderConfiguration UrlBuilderConfiguration = new UrlBuilderConfiguration("http", "api.example.com");

        protected override void OnConfiguring(IDocumentContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseUrlBuilderConfiguration(UrlBuilderConfiguration);
        }

        protected override void OnServiceModelCreating(IServiceModelBuilder serviceModelBuilder)
        {
            // Home /////////////////////////////////////////////////////////
            var homeDocumentConfiguration = serviceModelBuilder.Resource<HomeDocument>();

            // .. Hypermedia
            homeDocumentConfiguration.Hypermedia()
                                     .SetApiCollectionPathSegment(String.Empty);

            // .. ResourceIdentity
            homeDocumentConfiguration.ResourceIdentity()
                                     .SetApiType("home");

            serviceModelBuilder.HomeResource<HomeDocument>();

            // World ////////////////////////////////////////////////////////
            var worldConfiguration = serviceModelBuilder.Resource<World>();

            // .. Hypermedia
            worldConfiguration.Hypermedia()
                              .SetApiCollectionPathSegment("worlds");

            // .. ResourceIdentity
            worldConfiguration.ResourceIdentity(x => x.Id)
                              .SetApiType("worlds");

            // .. Attributes
            worldConfiguration.Attribute(x => x.Name)
                              .SetApiPropertyName("name");

            worldConfiguration.Attribute(x => x.SurfaceArea)
                              .SetApiPropertyName("surface-area");

            worldConfiguration.Attribute(x => x.SupportLife)
                              .SetApiPropertyName("support-life");

            worldConfiguration.Attribute(x => x.HasWater)
                              .SetApiPropertyName("has-water");

            // .. Relationships
            worldConfiguration.ToOneRelationship<SolarSystem>(rel: "solar-system");
            worldConfiguration.ToManyRelationship<Moon>(rel: "moons");

            // Solar System /////////////////////////////////////////////////
            var solarSystemConfiguration = serviceModelBuilder.Resource<SolarSystem>();

            // .. Hypermedia
            solarSystemConfiguration.Hypermedia()
                                    .SetApiCollectionPathSegment("solar-systems");

            // .. ResourceIdentity
            solarSystemConfiguration.ResourceIdentity(x => x.Id)
                                    .SetApiType("solar-systems");

            // .. Attributes
            solarSystemConfiguration.Attribute(x => x.Name)
                                    .SetApiPropertyName("name");

            // Moon /////////////////////////////////////////////////////////
            var moonConfiguration = serviceModelBuilder.Resource<Moon>();

            // .. Hypermedia
            moonConfiguration.Hypermedia()
                             .SetApiCollectionPathSegment("moons");

            // .. ResourceIdentity
            moonConfiguration.ResourceIdentity(x => x.Id)
                             .SetApiType("moons");

            // .. Attributes
            moonConfiguration.Attribute(x => x.Name)
                             .SetApiPropertyName("name");
        }
    }
}
