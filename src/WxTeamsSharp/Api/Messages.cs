using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string messageUrl = "/messages";

        /// <summary>
        /// Deletes a message, by message ID.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message.</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        public static async Task<IResponseMessage> DeleteMessageAsync(string messageId)
           => await TeamsClient.DeleteResultAsync<Message>($"{messageUrl}/{messageId}");

        /// <summary>
        /// Shows details for a message, by message ID.
        /// </summary>
        /// <param name="messageId">The unique identifier for the message.</param>
        /// <returns>This method returns a Message</returns>
        public static async Task<IMessage> GetMessageAsync(string messageId)
            => await TeamsClient.GetResultAsync<Message>($"{messageUrl}/{messageId}");

        /// <summary>
        /// Post a plain text or rich text message, and optionally, a file attachment attachment, to a room.
        /// </summary>
        /// <param name="sendMessage">Use MessageBuilder.New() to start building a message. .Build() will produce an ISendableMessage</param>
        /// <returns>This method returns the Message object that was sent.</returns>
        public static async Task<IMessage> SendMessageAsync(ISendableMessage sendMessage)
        {
            if (sendMessage.HasLocalFile())
                return await TeamsClient.PostMultiPartResultAsync<Message, ISendableMessage>(messageUrl, sendMessage);

            return await TeamsClient.PostResultAsync<Message, ISendableMessage>(messageUrl, sendMessage);
        }

        /// <summary>
        /// Lists all messages in a 1:1 (direct) room.
        /// The list sorts the messages in descending order by creation date.
        /// </summary>
        /// <param name="userIdOrEmail">User ID or Email to retreive messages for</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        public static async Task<IListResult<IMessage>> GetDirectMessagesAsync(string userIdOrEmail)
        {
            var queryBy = "personId";

            if (RegexUtilities.IsValidEmail(userIdOrEmail))
                queryBy = "personEmail";

            var messageParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(queryBy, userIdOrEmail)
            };

            var path = await GetPathWithQueryAsync($"{messageUrl}/direct", messageParams);
            return await TeamsClient.GetResultsAsync<Message, IMessage>(path);
        }

        /// <summary>
        /// Lists all messages in a room before a specific date and time. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="roomId">List messages in a room, by ID.</param>
        /// <param name="before">List messages sent before a date and time.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        public static async Task<IListResult<IMessage>> GetRoomMessagesBeforeDateAsync(string roomId, DateTimeOffset before, int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default)
            => await GetRoomMessagesAsync(roomId, max, userMentioned, mentionedPeople, before);

        /// <summary>
        /// Lists all messages in a room before a specific message. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="roomId">List messages in a room, by ID.</param>
        /// <param name="messageId">List messages sent before a message, by ID.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        public static async Task<IListResult<IMessage>> GetRoomMessagesBeforeMessageAsync(string roomId, string messageId, int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default)
            => await GetRoomMessagesAsync(roomId, max, userMentioned, mentionedPeople, beforeMessage: messageId);

        /// <summary>
        /// Lists all messages in a room. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="roomId">List messages in a room, by ID.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID.</param>
        /// <param name="before">List messages sent before a date and time.</param>
        /// <param name="beforeMessage">List messages sent before a message, by ID.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        public static async Task<IListResult<IMessage>> GetRoomMessagesAsync(string roomId, int max = 50, bool userMentioned = false,
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

            var path = await GetPathWithQueryAsync(messageUrl, messageParams);
            return await TeamsClient.GetResultsAsync<Message, IMessage>(path);
        }
    }
}
