using System.Collections.Generic;

namespace Blogging.ServiceModel
{
    public class Blog
    {
        public long BlogId { get; set; }
        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}
