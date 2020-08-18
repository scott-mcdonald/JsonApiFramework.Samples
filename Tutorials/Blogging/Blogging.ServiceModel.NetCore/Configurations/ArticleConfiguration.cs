using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class ArticleConfiguration : ResourceTypeBuilder<Article>
    {
        public ArticleConfiguration()
        {
            // Relationships
            this.ToOneRelationship<Blog>("blog");
            this.ToOneRelationship<Person>("author");
            this.ToManyRelationship<Comment>("comments");

            // Ignore
            this.Attribute(x => x.BlogId).Ignore();
            this.Attribute(x => x.Blog).Ignore();

            this.Attribute(x => x.AuthorId).Ignore();
            this.Attribute(x => x.Author).Ignore();

            this.Attribute(x => x.Comments).Ignore();
        }
    }
}
