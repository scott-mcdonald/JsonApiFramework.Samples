using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class ApiEntryPointConfiguration : ResourceTypeBuilder<ApiEntryPoint>
    {
        public ApiEntryPointConfiguration()
        {
            // ResourceIdentity
            // Explicitly set the JSON API type as "api-entry-point".
            this.ResourceIdentity(x => x.Id)
                .SetApiType("api-entry-point");
        }
    }
}
