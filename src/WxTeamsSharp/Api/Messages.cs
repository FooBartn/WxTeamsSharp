using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Extensions;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Models.Messages;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi : IWxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteMessageAsync(string messageId)
           => await TeamsClient.DeleteResultAsync<Message>($"{WxTeamsConstants.MessagesUrl}/{messageId}");

        /// <inheritdoc/>
        public async Task<Message> GetMessageAsync(string messageId)
            => await TeamsClient.GetResultAsync<Message>($"{WxTeamsConstants.MessagesUrl}/{messageId}");

        /// <inheritdoc/>
        public async Task<Message> SendMessageAsync(ISendableMessage sendMessage)
        {
            if (sendMessage.HasLocalFile())
                return await TeamsClient.PostMultiPartResultAsync<Message, ISendableMessage>(WxTeamsConstants.MessagesUrl, sendMessage);

            return await TeamsClient.PostResultAsync<Message, ISendableMessage>(WxTeamsConstants.MessagesUrl, sendMessage);
        }

        /// <inheritdoc/>
        public async Task<IListResult<Message>> GetDirectMessagesAsync(string userIdOrEmail)
        {
            var queryBy = "personId";

            if (RegexUtilities.IsValidEmail(userIdOrEmail))
                queryBy = "personEmail";

            var messageParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(queryBy, userIdOrEmail)
            };

            var path = await GetPathWithQueryAsync($"{WxTeamsConstants.MessagesUrl}/direct", messageParams);
            return await TeamsClient.GetResultsAsync<Message>(path);
        }

        /// <inheritdoc/>
        public async Task<IListResult<Message>> GetRoomMessagesBeforeDateAsync(string roomId, DateTimeOffset before, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = default)
            => await GetRoomMessagesAsync(roomId, max, userMentioned, mentionedPeople, before);

        /// <inheritdoc/>
        public async Task<IListResult<Message>> GetRoomMessagesBeforeMessageAsync(string roomId, string messageId, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = default)
            => await GetRoomMessagesAsync(roomId, max, userMentioned, mentionedPeople, beforeMessage: messageId);

        /// <inheritdoc/>
        public async Task<IListResult<Message>> GetRoomMessagesAsync(string roomId, int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default, DateTimeOffset before = default, string beforeMessage = "")
        {
            var messageParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(roomId), roomId)
            };

            if (mentionedPeople == null)
                mentionedPeople = new List<string>();

            if (max != 50)
                messageParams.Add(new KeyValuePair<string, string>(nameof(max), max.ToString()));

            if (userMentioned)
                mentionedPeople.Add("me");

            if (mentionedPeople.Any())
                messageParams.Add(new KeyValuePair<string, string>(nameof(mentionedPeople), string.Join(',', mentionedPeople)));

            if (before != DateTimeOffset.MinValue)
                messageParams.Add(new KeyValuePair<string, string>(nameof(before), before.ToFormattedUTCTime()));

            if (!string.IsNullOrEmpty(beforeMessage))
                messageParams.Add(new KeyValuePair<string, string>(nameof(beforeMessage), beforeMessage));

            var path = await GetPathWithQueryAsync(WxTeamsConstants.MessagesUrl, messageParams);
            return await TeamsClient.GetResultsAsync<Message>(path);
        }
    }
}
