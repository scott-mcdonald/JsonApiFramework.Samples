using Blogging.ServiceModel.Configurations;

using JsonApiFramework.ServiceModel;
using JsonApiFramework.ServiceModel.Configuration;
using JsonApiFramework.ServiceModel.Conventions;

namespace Blogging.ServiceModel
{
    public static class ConfigurationFactory
    {
        public static ConventionSet CreateConventions()
        {
            var conventionSetBuilder = new ConventionSetBuilder();

            // Use JSON API standard member naming convention for JSON API resource attributes.
            // For example, FirstName in POCO becomes "first-name" as a JSON API attribute.
            conventionSetBuilder.ApiAttributeNamingConventions()
                                .AddStandardMemberNamingConvention();

            // Use JSON API standard member naming and plurization conventions of the POCO type
            // name as the JSON API type name.
            // For example, Article POCO type becomes "articles" as the JSON API type.
            conventionSetBuilder.ApiTypeNamingConventions()
                                .AddPluralNamingConvention()
                                .AddStandardMemberNamingConvention();

            // Discover all public properties as JSON API resource attributes.
            // For example, FirstName property in POCO becomes an attribute of a JSON API resource.
            conventionSetBuilder.ResourceTypeConventions()
                                .AddPropertyDiscoveryConvention();

            var conventions = conventionSetBuilder.Create();
            return conventions;
        }

        public static IServiceModel CreateServiceModel()
        {
            var serviceModelBuilder = new ServiceModelBuilder();

            serviceModelBuilder.Configurations.Add(new ApiEntryPointConfiguration());
            serviceModelBuilder.Configurations.Add(new ArticleConfiguration());
            serviceModelBuilder.Configurations.Add(new BlogConfiguration());
            serviceModelBuilder.Configurations.Add(new CommentConfiguration());
            serviceModelBuilder.Configurations.Add(new PersonConfiguration());

            var conventions = CreateConventions();
            var serviceModel = serviceModelBuilder.Create(conventions);
            return serviceModel;
        }
    }
}
