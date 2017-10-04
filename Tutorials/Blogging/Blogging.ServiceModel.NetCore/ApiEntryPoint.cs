using JsonApiFramework;

namespace Blogging.ServiceModel
{
    public class ApiEntryPoint : IResource
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Version { get; set; }
    }
}
