using JsonApiFramework;

namespace Blogging.ServiceModel
{
    public class Article : IResource
    {
        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
