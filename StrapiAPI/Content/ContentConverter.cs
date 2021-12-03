using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Solarflare.StrapiAPI
{
    internal class ContentConverter : JsonConverter<Content>
    {
        private const string TypePropertyName = "type";
        
        public override void WriteJson(JsonWriter writer, Content? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            Type contentType = value.GetType();

            JObject jObject = JObject.FromObject(value);

            jObject.WriteTo(writer);
        }

        public override Content? ReadJson(JsonReader reader, Type objectType, Content? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            JToken? typeToken = jObject[TypePropertyName];
            if (typeToken == null)
            {
                throw new JsonSerializationException($"Cannot deserialize {nameof(Content)}. Serialized data does not contain '{TypePropertyName}' property.");
            }
            
            string? contentTypeString = typeToken.Value<string>();
            if (contentTypeString == null)
            {
                throw new JsonSerializationException($"Cannot deserialize {nameof(Content)}. '{TypePropertyName}' property is null.");
            }
            
            Type contentType = GetType().Assembly.GetType(contentTypeString);
            return (Content?) jObject.ToObject(contentType);
        }
    }
}