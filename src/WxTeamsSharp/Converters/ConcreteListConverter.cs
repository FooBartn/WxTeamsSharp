using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WxTeamsSharp.Converters
{
    internal class ConcreteListConverter<T, TInterface> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader,
         Type objectType, object existingValue, JsonSerializer serializer)
        {
            var deserialized = serializer.Deserialize<List<T>>(reader);
            return Enumerable.Cast<TInterface>(deserialized).ToList();
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
