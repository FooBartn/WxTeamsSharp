namespace WxTeamsSharp.Interfaces.Webhooks
{
    /// <summary>
    /// Webhook Data
    /// </summary>
    /// <typeparam name="TResource">Type of resource</typeparam>
    public interface IWebhookData<out TResource> : IWebhookCommon
    {
        /// <summary>
        /// The personId of the user that caused the webhook to be sent. 
        /// For example, on a messsage created webhook, the author of the message will be the actor. 
        /// On a membership deleted webhook, the actor is the person who removed a user from a room.
        /// </summary>
        string ActorId { get; }

        /// <summary>
        /// Contains the resource that triggered the webhook.
        /// </summary>
        TResource Data { get; }
    }
}
