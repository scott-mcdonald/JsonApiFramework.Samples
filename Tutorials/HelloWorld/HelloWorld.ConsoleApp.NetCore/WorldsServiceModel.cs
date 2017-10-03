using System;

using JsonApiFramework;

namespace HelloWorld
{
    public enum SupportLife
    {
        No,
        Yes,
        Unknown
    }

    public class World : IResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long? SurfaceArea { get; set; }
        public SupportLife SupportLife { get; set; }
        public bool? HasWater { get; set; }
    }

    public class SolarSystem : IResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class Moon : IResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
