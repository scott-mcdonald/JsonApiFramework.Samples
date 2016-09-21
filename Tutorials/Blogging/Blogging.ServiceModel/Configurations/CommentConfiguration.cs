using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class CommentConfiguration : ResourceTypeBuilder<Comment>
    {
        public CommentConfiguration()
        {
            // Attributes to Ignore
            this.Attribute(x => x.ArticleId).Ignore();
            this.Attribute(x => x.AuthorId).Ignore();

            // Relationships
            this.ToOneRelationship<Article>("article");
            this.ToOneRelationship<Person>("author");
        }
    }
}
