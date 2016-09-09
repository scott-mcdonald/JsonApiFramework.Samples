using JsonApiFramework;

namespace Blogging.ServiceModel
{
    public class Comment : IResource
    {
        public long CommentId { get; set; }
        public string Body { get; set; }
    }
}
