using System;
using System.Collections.Generic;
using System.Diagnostics;

using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create sample worlds resources.
            var sampleWorlds = GetSampleWorlds();

            // Build JSON API document.
            var document = new WorldsDocumentContext()
                .NewDocument("http://api.example.com/worlds")
                    .Links()
                        .AddUpLink()
                        .AddSelfLink()
                    .LinksEnd()
                    .ResourceCollection(sampleWorlds)
                        // Expose to-one relationship to "solar-system"
                        .Relationships()
                            .Relationship("solar-system")
                                .Links()
                                    .AddRelatedLink()
                                .LinksEnd()
                            .RelationshipEnd()
                        .RelationshipsEnd()

                        // Expose to-many relationship to "moons"
                        .Relationships()
                            .AddRelationship("moons", Keywords.Related) // (shortcut syle)
                        .RelationshipsEnd()

                        .Links()
                            .AddSelfLink()
                        .LinksEnd()

                    .ResourceCollectionEnd()
                .WriteDocument();

            // Convert document to JSON and output to the console.
            var jsonSerializerSettings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    Converters = new List<JsonConverter>{new StringEnumConverter()}
                };
            var json = document.ToJson(jsonSerializerSettings);

            // .. Write to both the console and deubg output tab in Visual Studio.
            Console.WriteLine(json);
            Debug.Print(json);
        }

        static IEnumerable<World> GetSampleWorlds()
        {
            var sampleWorlds = new[]
                {
                    new World
                        {
                            Id = Guid.NewGuid(),
                            Name = "Earth",
                            SurfaceArea = 510072000, // km2
                            SupportLife = SupportLife.Yes,
                            HasWater = true
                        },
                    new World
                        {
                            Id = Guid.NewGuid(),
                            Name = "Mars",
                            SurfaceArea = 144798500, // km2
                            SupportLife = SupportLife.No,
                            HasWater = false
                        },
                    new World
                        {
                            Id = Guid.NewGuid(),
                            Name = "Proxima Centauri b",
                            SurfaceArea = null, // km2
                            SupportLife = SupportLife.Unknown,
                            HasWater = null
                        }
                };
            return sampleWorlds;
        }
    }
}
