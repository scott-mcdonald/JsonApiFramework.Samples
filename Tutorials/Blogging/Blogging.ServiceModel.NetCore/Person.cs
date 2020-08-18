using System.Collections.Generic;

namespace Blogging.ServiceModel
{
    public class Person
    {
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Twitter { get; set; }

        public List<Article> Articles { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
