using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class ArticleConfiguration : ResourceTypeBuilder<Article>
    {
        public ArticleConfiguration()
        {
            // Relationships
            this.ToOneRelationship<Person>("author");
            this.ToManyRelationship<Comment>("comments");
        }
    }
}
