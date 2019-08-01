using System;
using WxTeamsSharp.Enums;

namespace WxTeamsSharp.Interfaces.Events
{
    /// <summary>
    /// Details for an Event
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// Unique Id for the EventData
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Unique Id for the Room
        /// </summary>
        string RoomId { get; }

        /// <summary>
        /// Type of Room
        /// </summary>
        RoomType RoomType { get; }

        /// <summary>
        /// Event Text
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Unique Id for the user
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// Email for the user
        /// </summary>
        string PersonEmail { get; }

        /// <summary>
        /// When the EventData was created
        /// </summary>
        DateTimeOffset Created { get; }
    }
}
