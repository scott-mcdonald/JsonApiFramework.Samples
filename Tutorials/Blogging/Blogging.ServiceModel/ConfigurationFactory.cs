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

            conventionSetBuilder.ApiAttributeNamingConventions()
                                .AddStandardMemberNamingConvention();

            conventionSetBuilder.ApiTypeNamingConventions()
                                .AddPluralNamingConvention()
                                .AddStandardMemberNamingConvention();

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
