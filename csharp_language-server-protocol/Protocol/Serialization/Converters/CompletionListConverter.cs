using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Serialization.Converters
{
    class CompletionListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var v = value as CompletionList;
            if (!v.IsIncomplete)
            {
                serializer.Serialize(writer, v.Items.ToArray());
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName("isIncomplete");
            writer.WriteValue(v.IsIncomplete);

            writer.WritePropertyName("items");
            writer.WriteStartArray();
            foreach (var item in v.Items)
            {
                serializer.Serialize(writer, item);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var array = JArray.Load(reader).ToObject<IEnumerable<CompletionItem>>(serializer);
                return new CompletionList(array);
            }

            var result = JObject.Load(reader);
            var items = result["items"].ToObject<IEnumerable<CompletionItem>>(serializer);
            return new CompletionList(items, result["isIncomplete"].Value<bool>());
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(CompletionList);
    }
}
