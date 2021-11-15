using JsonApiFramework.JsonApi;
using JsonApiFramework.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Blogging.WebService
{
    public static class JsonApiFrameworkExtensions
    {
        /// <summary>
        /// Update the resource informed in a PATCH request.
        /// <para>This method only updates the properties of <paramref name="resourceToBeUpdated"/> that are included in <paramref name="document"/></para>
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="ctx">Document context</param>
        /// <param name="document">Document containing the changes sent in the PATCH request</param>
        /// <param name="resourceToBeUpdated">Resource which values will be updated by the fields informed in the <paramref name="document"/> </param>
        public static void UpdateResource<TResource>(this DocumentContext ctx, Document document, TResource resourceToBeUpdated)
        {
            Type clrResourceType = typeof(TResource);
            var serviceModelOfType = ctx.ServiceModel.ResourceTypes.FirstOrDefault(res => res.ClrType.FullName.Equals(clrResourceType.FullName));
            var serviceModelTypeAttributeInfo = serviceModelOfType.AttributesInfo;
            Resource sentResource = document.GetData() as Resource;

            ApiObject inDocumentSentAttributes = sentResource.Attributes ?? new ApiObject();
            foreach (ApiReadProperty sentAttribute in inDocumentSentAttributes)
            {
                var clrPropertyName = serviceModelTypeAttributeInfo
                    .Collection
                    .Where(at => at.ApiPropertyName.Equals(sentAttribute.Name))
                    .Select(at => at.ClrPropertyName)
                    .First();

                JValue jValue = sentAttribute.Value as JValue;
                object rawValue = JTokenToConventionalDotNetObject(jValue);
                clrResourceType
                    .GetProperty(clrPropertyName)
                    .SetValue(resourceToBeUpdated, rawValue);
            }

            Relationships inDocumentSentRelationships = sentResource.Relationships ?? new Relationships();
            var serviceModelTypeRelationshipsInfo = serviceModelOfType.RelationshipsInfo;
            foreach (var sentRelationship in inDocumentSentRelationships)
            {
                var key = sentRelationship.Key;
                var value = sentRelationship.Value;

                var type = serviceModelTypeRelationshipsInfo
                            .Collection
                            .Where(rel => rel.Rel.Equals(key))
                            .Select(at => at.ToClrType)
                            .First();

                // If I were able to get the property name of the relationship (something the same way I got the attribute names), I would be able to update the value dynamically.
                // These property names can be stored in the Service Model (in the ToOneRelationship/ToManyRelationship object), or probably with the EF Core itself.
                if (value is ToOneRelationship toOneRel)
                {
                    throw new ArgumentException("Updating to one relationships is not supported");
                }
                else if (value is ToManyRelationship)
                {
                    throw new ArgumentException("Updating to-many relationships is not supported");
                    // Can probably be done by instantiating a list of the type with the sent Ids 
                }
            }
        }

        /// <summary>Converts a Json.Net JToken to a boxed conventional .NET type (int, List, etc.)</summary>
        /// <param name="token">The JToken to evaluate</param>
        /// <remarks>
        /// Credits to the original implementation of this method: <a href="https://stackoverflow.com/a/36659561/9990676">Convert Json.NET objects to conventional .NET objects without knowing the types [duplicate]</a>
        /// </remarks>
        private static object JTokenToConventionalDotNetObject(JToken token)
        {
            // TODO This method needs improvement: Number precision are being lost with the actual implementation:
            //  - When the type is JTokenType.Integer, the returned value is always int64. 
            //  - If the value is JTokenType.Float, the same rule applies, the value is aways Double.
            // The numbers need to be casted down if needed to avoid type errors in the clients of this method.

            switch (token.Type)
            {
                case JTokenType.Object:
                    return ((JObject)token).Properties()
                        .ToDictionary(prop => prop.Name, prop => JTokenToConventionalDotNetObject(prop.Value));
                case JTokenType.Array:
                    return token.Values().Select(JTokenToConventionalDotNetObject).ToList();
                case JTokenType.Integer:
                    long rawIntValue = token.ToObject<long>();
                    if (rawIntValue >= -32768 && rawIntValue <= 32767) return Convert.ToInt16(rawIntValue);
                    if (rawIntValue >= -2147483648 && rawIntValue <= 2147483647) return Convert.ToInt32(rawIntValue);
                    return rawIntValue;
                case JTokenType.Float:
                    double rawDoubleValue = token.ToObject<double>();
                    return Convert.ToSingle(rawDoubleValue);
                default:
                    return token.ToObject<object>();
            }
        }
    }
}
