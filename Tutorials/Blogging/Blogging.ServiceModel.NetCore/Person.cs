using JsonApiFramework;

namespace Blogging.ServiceModel
{
    public class Person
    {
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Twitter { get; set; }
    }
}
