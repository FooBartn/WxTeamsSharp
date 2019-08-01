using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WxTeamsSharp.Utilities
{
    internal static class JsonUtilities
    {
        internal static readonly JsonSerializerSettings Standard = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };

        internal static readonly JsonSerializerSettings IgnoreNull = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };

        internal static TEntity FromJson<TEntity>(string json, JsonSerializerSettings settings = null)
            where TEntity : class
        {
            if (settings == null)
                settings = Standard;

            return JsonConvert.DeserializeObject<TEntity>(json, settings);
        }

        internal class ConcreteConverter<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType) => true;

            public override object ReadJson(JsonReader reader,
             Type objectType, object existingValue, JsonSerializer serializer)
            {
                return serializer.Deserialize<T>(reader);
            }

            public override void WriteJson(JsonWriter writer,
                object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

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
}
