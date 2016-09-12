﻿using JsonApiFramework;

namespace Blogging.ServiceModel
{
    public class Comment : IResource
    {
        public long ArticleId { get; set; }
        public long AuthorId { get; set; }

        public long CommentId { get; set; }
        public string Body { get; set; }
    }
}
