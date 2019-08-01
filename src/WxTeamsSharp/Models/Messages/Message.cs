using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Models.General;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.Messages
{
    /// <inheritdoc/>
    internal class Message : IMessage
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string RoomId { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomType RoomType { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "toPersonId")]
        public string RecipientId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "toPersonEmail")]
        public string RecipientEmail { get; set; }

        /// <inheritdoc/>
        public string Text { get; set; }

        /// <inheritdoc/>
        public string Markdown { get; set; }

        /// <inheritdoc/>
        public IList<string> Files { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "personId")]
        public string AuthorId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "personEmail")]
        public string AuthorEmail { get; set; }

        /// <inheritdoc/>
        public IList<string> MentionedPeople { get; set; }

        /// <inheritdoc/>
        public IList<string> MentionedGroups { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync() => await WxTeamsApi.DeleteMessageAsync(Id);
    }
}
