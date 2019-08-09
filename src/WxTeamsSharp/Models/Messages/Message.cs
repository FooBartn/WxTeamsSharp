using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Messages
{
    /// <inheritdoc/>
    public class Message : TeamsObject, IMessage
    {

        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string RoomId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomType RoomType { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "toPersonId")]
        public string RecipientId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "toPersonEmail")]
        public string RecipientEmail { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Text { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Markdown { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public List<string> Files { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "personId")]
        public string AuthorId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "personEmail")]
        public string AuthorEmail { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public List<string> MentionedPeople { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public List<string> MentionedGroups { get; private set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; private set; }

        /// <inheritdoc/>
        public bool HasError => Error != null;

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync() => await TeamsApi.DeleteMessageAsync(Id);
    }
}
