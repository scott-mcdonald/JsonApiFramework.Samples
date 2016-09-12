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
        }
    }
}
