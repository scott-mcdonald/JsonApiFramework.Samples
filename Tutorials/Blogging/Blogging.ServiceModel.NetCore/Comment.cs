namespace Blogging.ServiceModel
{
    public class Comment
    {
        public long ArticleId { get; set; }
        public Article Article { get; set; }

        public long? AuthorId { get; set; }
        public Person Author { get; set; }

        public long CommentId { get; set; }
        public string Body { get; set; }
    }
}
