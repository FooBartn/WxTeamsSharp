using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Webhooks;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi : IWxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IListResult<Webhook>> GetWebhooksAsync(int max = 100)
        {
            var webhookParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(max), max.ToString())
            };

            var path = await GetPathWithQueryAsync(WxTeamsConstants.WebhooksUrl, webhookParams);
            return await TeamsClient.GetResultsAsync<Webhook>(path);
        }

        /// <inheritdoc/>
        public async Task<Webhook> GetWebhookAsync(string webhookId)
            => await TeamsClient.GetResultAsync<Webhook>($"{WxTeamsConstants.WebhooksUrl}/{webhookId}");

        /// <inheritdoc/>
        public async Task<Webhook> CreateWebhookAsync(string name, string targetUrl, WebhookResource resource,
            EventType eventType, string filter = "", string secret = "")
        {
            ValidateWebhookParameters(name, targetUrl);

            var webhookParams = new WebhookParams
            {
                Name = name,
                TargetUrl = targetUrl,
                Resource = resource,
                Event = eventType,
                Filter = filter,
                Secret = secret
            };

            return await TeamsClient.PostResultAsync<Webhook, WebhookParams>(WxTeamsConstants.WebhooksUrl, webhookParams);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteWebhookAsync(string webhookId)
            => await TeamsClient.DeleteResultAsync<Webhook>($"{WxTeamsConstants.WebhooksUrl}/{webhookId}");

        /// <inheritdoc/>
        public async Task<Webhook> UpdateWebhookAsync(string webhookId, string name, string targetUrl, string secret = "",
            WebhookStatus? status = null)
        {
            ValidateWebhookParameters(name, targetUrl);

            var webhookParams = new WebhookParams
            {
                Name = name,
                TargetUrl = targetUrl
            };

            if (!string.IsNullOrEmpty(secret))
                webhookParams.Secret = secret;

            if (status != null)
                webhookParams.Status = status;

            return await TeamsClient.PutResultAsync<Webhook, WebhookParams>($"{WxTeamsConstants.WebhooksUrl}/{webhookId}", webhookParams);
        }

        private void ValidateWebhookParameters(string name, string targetUrl)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(targetUrl))
            {
                var exception = new ArgumentException("Name, TargetURL, Resource, and EventType cannot be null");
                _logger.LogError(exception, exception.Message);
                throw exception;
            }
        }
    }
}
