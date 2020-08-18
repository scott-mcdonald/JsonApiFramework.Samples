using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class CommentConfiguration : ResourceTypeBuilder<Comment>
    {
        public CommentConfiguration()
        {
            // Relationships
            this.ToOneRelationship<Article>("article");
            this.ToOneRelationship<Person>("author");

            // Ignore
            this.Attribute(x => x.ArticleId).Ignore();
            this.Attribute(x => x.Article).Ignore();

            this.Attribute(x => x.AuthorId).Ignore();
            this.Attribute(x => x.Author).Ignore();
        }
    }
}
