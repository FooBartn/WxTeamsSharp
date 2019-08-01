using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Webhooks;

namespace WxTeamsSharp.Models.Webhooks
{
    /// <inheritdoc/>
    internal class Webhook : IWebhook
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string TargetUrl { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public WebhookResource Resource { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Event { get; set; }

        /// <inheritdoc/>
        public string Filter { get; set; }

        /// <inheritdoc/>
        public string Secret { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public WebhookStatus Status { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "orgId")]
        public string OrganizationId { get; set; }

        /// <inheritdoc/>
        public string CreatedBy { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "appId")]
        public string ApplicationId { get; set; }

        /// <inheritdoc/>
        public string OwnedBy { get; set; }

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

            return await WxTeamsApi.UpdateWebhookAsync(Id, name, targetUrl, secret, status);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync()
            => await WxTeamsApi.DeleteWebhookAsync(Id);
    }
}
