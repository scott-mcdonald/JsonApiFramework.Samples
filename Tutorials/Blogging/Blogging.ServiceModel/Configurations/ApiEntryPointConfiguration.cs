using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class ApiEntryPointConfiguration : ResourceTypeBuilder<ApiEntryPoint>
    {
        public ApiEntryPointConfiguration()
        {
            // ResourceIdentity
            this.ResourceIdentity(x => x.Id)
                .SetApiType("api-entry-point");
        }
    }
}
