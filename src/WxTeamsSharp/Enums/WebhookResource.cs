namespace WxTeamsSharp.Enums
{
    /// <summary>
    /// The resource type for the webhook. Creating a webhook requires 'read' scope on the resource the webhook is for.
    /// </summary>
    public enum WebhookResource
    {
        /// <summary>
        /// the Memberships resource
        /// </summary>
        Memberships,

        /// <summary>
        /// the Messages resource
        /// </summary>
        Messages,

        /// <summary>
        /// the Rooms resource
        /// </summary>
        Rooms
    }
}
