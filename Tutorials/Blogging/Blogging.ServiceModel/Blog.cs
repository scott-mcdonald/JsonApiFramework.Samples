
using JsonApiFramework;

namespace Blogging.ServiceModel
{
    public class Blog : IResource
    {
        public long BlogId { get; set; }
        public string Name { get; set; }
    }
}
