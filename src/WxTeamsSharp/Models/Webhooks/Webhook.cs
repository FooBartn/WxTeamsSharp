using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Webhooks;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Webhooks
{
    /// <inheritdoc/>
    public class Webhook : TeamsObject, IWebhook
    {

        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Name { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string TargetUrl { get; private set; }

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
        [JsonProperty]
        public string Secret { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public WebhookStatus Status { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset Created { get; private set; }

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
        public async Task<IWebhook> UpdateAsync(string name = "", string targetUrl = "", string secret = "", WebhookStatus? status = null)
        {
            if (string.IsNullOrEmpty(name))
                name = Name;

            if (string.IsNullOrEmpty(targetUrl))
                targetUrl = TargetUrl;

            if (string.IsNullOrEmpty(secret))
                secret = Secret;

            if (status == null)
                status = Status;

            return await TeamsApi.UpdateWebhookAsync(Id, name, targetUrl, secret, status);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync()
            => await TeamsApi.DeleteWebhookAsync(Id);
    }
}
