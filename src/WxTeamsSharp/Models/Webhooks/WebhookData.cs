using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Webhooks;

namespace WxTeamsSharp.Models.Webhooks
{
    /// <inheritdoc/>
    public class WebhookData<T> : IWebhookData<T>
        where T : class
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Name { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public WebhookResource Resource { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Event { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Filter { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "orgId")]
        public string OrganizationId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string CreatedBy { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "appId")]
        public string ApplicationId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string OwnedBy { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public WebhookStatus Status { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string ActorId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public T Data { get; private set; }
    }
}
