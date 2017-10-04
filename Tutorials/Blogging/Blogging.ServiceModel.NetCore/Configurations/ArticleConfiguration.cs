using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class ArticleConfiguration : ResourceTypeBuilder<Article>
    {
        public ArticleConfiguration()
        {
            // Attributes to Ignore
            this.Attribute(x => x.BlogId).Ignore();
            this.Attribute(x => x.AuthorId).Ignore();

            // Relationships
            this.ToOneRelationship<Blog>("blog");
            this.ToOneRelationship<Person>("author");
            this.ToManyRelationship<Comment>("comments");
        }
    }
}
