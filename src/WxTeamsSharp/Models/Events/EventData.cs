using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Events;

namespace WxTeamsSharp.Models.Events
{
    /// <inheritdoc/>
    internal class EventData : IEventData
    {
        /// <inheritdoc/>
        public string Id { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string RoomId { get; set; } = string.Empty;

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomType RoomType { get; set; }

        /// <inheritdoc/>
        public string Text { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string PersonId { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string PersonEmail { get; set; } = string.Empty;

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }
    }
}
