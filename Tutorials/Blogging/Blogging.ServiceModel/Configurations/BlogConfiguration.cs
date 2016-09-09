using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class BlogConfiguration : ResourceTypeBuilder<Blog>
    {
        public BlogConfiguration()
        {
            // Relationships
            this.ToManyRelationship<Article>("articles");
        }
    }
}
