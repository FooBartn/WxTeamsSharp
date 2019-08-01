using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Events;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.General;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.Events
{
    /// <inheritdoc/>
    internal class Event : IEvent
    {
        /// <inheritdoc/>
        public string Id { get; set; } = string.Empty;

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public EventResource Resource { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Type { get; set; }

        /// <inheritdoc/>
        public string AppId { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string ActorId { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string OrgId { get; set; } = string.Empty;

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "data")]
        [JsonConverter(typeof(ConcreteConverter<EventData>))]
        public IEventData EventData { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }
    }
}
