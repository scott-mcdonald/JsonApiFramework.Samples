using System;

using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class ApiEntryPointConfiguration : ResourceTypeBuilder<ApiEntryPoint>
    {
        public ApiEntryPointConfiguration()
        {
            // Hypermedia
            this.Hypermedia()
                .SetApiCollectionPathSegment(String.Empty);

            // ResourceIdentity
            // Explicitly set the JSON API type as "api-entry-point".
            this.ResourceIdentity()
                .SetApiType("api-entry-point");
        }
    }
}
