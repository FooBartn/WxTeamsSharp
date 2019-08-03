using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;

namespace WxTeamsSharp.Interfaces.Webhooks
{
    /// <summary>
    /// Common properties for Webhooks
    /// </summary>
    public interface IWebhookCommon
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
        /// The status of the webhook. Use active to reactivate a disabled webhook.
        /// </summary>
        WebhookStatus Status { get; }
    }
}
