using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Serialization.Converters
{
    public class MarkedStringsOrMarkupContentConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = value as MarkedStringsOrMarkupContent;
            if (v.HasMarkupContent)
            {
                serializer.Serialize(writer, v.MarkupContent);
            }
            else
            {
                serializer.Serialize(writer, v.MarkedStrings);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var result = JObject.Load(reader);
                return new MarkedStringsOrMarkupContent(result.ToObject<MarkupContent>(serializer));
            }
            if (reader.TokenType == JsonToken.StartArray)
            {
                var result = JArray.Load(reader);
                return new MarkedStringsOrMarkupContent(result.ToObject<MarkedStringContainer>(serializer));
            }
            if (reader.TokenType == JsonToken.String)
            {
                return new MarkedStringsOrMarkupContent(reader.Value as string);
            }

            return "";
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(MarkedStringsOrMarkupContent);
    }
}