using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Models.Webhooks
{
    internal class WebhookParams : IJsonParams
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "targetUrl")]
        public string TargetUrl { get; set; }

        [JsonProperty(PropertyName = "resource")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        public WebhookResource? Resource { get; set; } = null;

        [JsonProperty(PropertyName = "event")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        public EventType? Event { get; set; } = null;

        [JsonProperty(PropertyName = "filter")]
        public string Filter { get; set; }

        [JsonProperty(PropertyName = "secret")]
        public string Secret { get; set; }

        [JsonProperty(PropertyName = "status")]
        public WebhookStatus? Status { get; set; } = null;

        public string ToJson() => JsonConvert.SerializeObject(this, JsonUtilities.IgnoreNull);
    }
}
