using System;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.Webhooks
{
    /// <summary>
    /// Webhooks allow your app to be notified via HTTP when a specific event occurs in Webex Teams. 
    /// For example, your app can register a webhook to be notified when a new message is posted into a specific room.
    ///
    /// Events trigger in near real-time allowing your app and backend IT 
    /// systems to stay in sync with new content and room activity.
    /// </summary>
    public interface IWebhook
    {
        /// <summary>
        /// A unique identifier for the webhook.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// A user-friendly name for the webhook.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The URL that receives POST requests for each event.
        /// </summary>
        string TargetUrl { get; }

        /// <summary>
        /// The resource type for the webhook. Creating a webhook requires 'read' scope on the resource the webhook is for.
        /// </summary>
        WebhookResource Resource { get; }

        /// <summary>
        /// The event type for the webhook.
        /// </summary>
        EventType Event { get; }

        /// <summary>
        /// The filter that defines the webhook scope.
        /// ex: roomId=2356df236tsdgs246e
        /// </summary>
        string Filter { get; }

        /// <summary>
        /// Unique Id of the Organization
        /// </summary>
        string OrganizationId { get; }

        /// <summary>
        /// What user created the webhook
        /// </summary>
        string CreatedBy { get; }
        
        /// <summary>
        /// The application Id attributed to the webhook
        /// </summary>
        string ApplicationId { get; }

        /// <summary>
        /// What user the webhook is owned by
        /// </summary>
        string OwnedBy { get; }

        /// <summary>
        /// The secret used to generate payload signature.
        /// </summary>
        string Secret { get; }

        /// <summary>
        /// The status of the webhook. Use active to reactivate a disabled webhook.
        /// </summary>
        WebhookStatus Status { get; }

        /// <summary>
        /// The date and time the webhook was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// Updates this webhook
        /// </summary>
        /// <param name="name">A user-friendly name for the webhook.</param>
        /// <param name="targetUrl">The URL that receives POST requests for each event.</param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <param name="status">The status of the webhook. Use active to reactivate a disabled webhook.</param>
        /// <returns>This method returns the updated Webhook</returns>
        Task<IWebhook> UpdateAsync(string name = "", string targetUrl = "", string secret = "", WebhookStatus? status = null);


        /// <summary>
        /// Deleted this webhook
        /// </summary>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteAsync();
    }
}
