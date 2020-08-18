using System.Collections.Generic;

namespace Blogging.ServiceModel
{
    public class Article
    {
        public long BlogId { get; set; }
        public Blog Blog { get; set; }

        public long AuthorId { get; set; }
        public Person Author { get; set; }

        public List<Comment> Comments { get; set; }

        public long ArticleId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
