using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Webhooks;
using WxTeamsSharp.Models.Webhooks;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string webhooksUrl = "/webhooks";

        /// <summary>
        /// List all of your webhooks.
        /// </summary>
        /// <param name="max">Limit the maximum number of webhooks in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Webhooks</returns>
        public static async Task<IListResult<IWebhook>> GetWebhooksAsync(int max = 100)
        {
            var webhookParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(max), max.ToString())
            };

            var path = await GetPathWithQueryAsync(webhooksUrl, webhookParams);
            return await TeamsClient.GetResultsAsync<Webhook, IWebhook>(path);
        }

        /// <summary>
        /// Shows details for a webhook, by ID.
        /// </summary>
        /// <param name="webhookId">The unique identifier for the webhook.</param>
        /// <returns>This method returns a Webhook</returns>
        public static async Task<IWebhook> GetWebhookAsync(string webhookId)
            => await TeamsClient.GetResultAsync<Webhook>($"{webhooksUrl}/{webhookId}");

        /// <summary>
        /// Creates a webhook.
        /// For me information: https://developer.webex.com/docs/api/guides/webhooks
        /// </summary>
        /// <param name="name">A user-friendly name for the webhook.</param>
        /// <param name="targetUrl">The URL that receives POST requests for each event.</param>
        /// <param name="resource">The resource type for the webhook. Creating a webhook requires 'read' scope on the resource the webhook is for.</param>
        /// <param name="eventType">The event type for the webhook.</param>
        /// <param name="filter">The filter that defines the webhook scope.</param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <returns>This method returns the created Webhook</returns>
        public static async Task<IWebhook> CreateWebhookAsync(string name, string targetUrl, WebhookResource resource,
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

            return await TeamsClient.PostResultAsync<Webhook, WebhookParams>(webhooksUrl, webhookParams);
        }

        /// <summary>
        /// Deletes a webhook, by ID.
        /// </summary>
        /// <param name="webhookId">The unique identifier for the webhook.</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        public static async Task<IResponseMessage> DeleteWebhookAsync(string webhookId)
            => await TeamsClient.DeleteResultAsync<Webhook>($"{webhooksUrl}/{webhookId}");

        /// <summary>
        /// Updates a webhook, by ID.
        /// </summary>
        /// <param name="webhookId">The unique identifier for the webhook.</param>
        /// <param name="name">A user-friendly name for the webhook.</param>
        /// <param name="targetUrl">The URL that receives POST requests for each event.</param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <param name="status">The status of the webhook. Use active to reactivate a disabled webhook.</param>
        /// <returns>This method returns the updated Webhook</returns>
        public static async Task<IWebhook> UpdateWebhookAsync(string webhookId, string name, string targetUrl, string secret = "",
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

            return await TeamsClient.PutResultAsync<Webhook, WebhookParams>($"{webhooksUrl}/{webhookId}", webhookParams);
        }

        private static void ValidateWebhookParameters(string name, string targetUrl)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(targetUrl))
                throw new ArgumentException("Name, TargetURL, Resource, and EventType cannot be null");
        }
    }
}
