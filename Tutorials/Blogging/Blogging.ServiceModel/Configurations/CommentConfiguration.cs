using JsonApiFramework.ServiceModel.Configuration;

namespace Blogging.ServiceModel.Configurations
{
    public class CommentConfiguration : ResourceTypeBuilder<Comment>
    {
        public CommentConfiguration()
        {
            // Relationships
            this.ToOneRelationship<Person>("author");
        }
    }
}
