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

    public class HomeDocument
    {
        public string Message { get; set; }
        public string Version { get; set; }
    }

    public class World
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long? SurfaceArea { get; set; }
        public SupportLife SupportLife { get; set; }
        public bool? HasWater { get; set; }
    }

    public class SolarSystem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class Moon
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
