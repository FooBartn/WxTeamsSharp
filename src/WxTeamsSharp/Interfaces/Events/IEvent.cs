using System;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.Events
{
    /// <summary>
    /// Events are generated when actions take place within Webex Teams, 
    /// such as when someone creates or deletes a message. Compliance Officers 
    /// may use the Events API to retrieve events for all users within an organization.
    /// </summary>
    public interface IEvent : IListable
    {
        /// <summary>
        /// The unique identifier for the event.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The type of resource in the event.
        /// </summary>
        EventResource Resource { get; }

        /// <summary>
        /// The action which took place in the event.
        /// </summary>
        EventType Type { get; }

        /// <summary>
        /// The ID of the application for the event.
        /// </summary>
        string AppId { get; }

        /// <summary>
        /// The ID of the person who performed the action.
        /// </summary>
        string ActorId { get; }

        /// <summary>
        /// The ID of the organization for the event.
        /// </summary>
        string OrgId { get; }

        /// <summary>
        /// The date and time of the event.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// The event’s data representation. This object will contain the event's resource, 
        /// such as memberships or messages, at the time the event took place.
        /// </summary>
        IEventData EventData { get; }
    }
}
