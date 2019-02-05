using JsonApiFramework;

namespace Blogging.ServiceModel
{
    public class Article
    {
        public long BlogId { get; set; }
        public long AuthorId { get; set; }

        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
