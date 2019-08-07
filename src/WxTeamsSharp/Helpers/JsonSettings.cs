using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Helpers
{
    internal static class JsonSettings
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
            where TEntity : TeamsObject
        {
            if (settings == null)
                settings = Standard;

            return JsonConvert.DeserializeObject<TEntity>(json, settings);
        }
    }
}
