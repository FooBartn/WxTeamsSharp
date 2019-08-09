using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Events;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Events
{
    /// <inheritdoc/>
    public class Event : TeamsObject, IEvent
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventResource Resource { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Type { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string AppId { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string ActorId { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string OrgId { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset Created { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "data")]
        [JsonConverter(typeof(ConcreteConverter<EventData>))]
        public IEventData EventData { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; private set; }

        /// <inheritdoc/>
        public bool HasError => Error != null;
    }
}
