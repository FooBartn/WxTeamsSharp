using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Extensions;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Interfaces.Rooms;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Models.Memberships;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Webhooks;

namespace WxTeamsSharp.Models.Rooms
{
    /// <inheritdoc/>
    public class Room : TeamsObject, IRoom
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string Title { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomType Type { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public bool IsLocked { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string TeamId { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset LastActivity { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string CreatorId { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset Created { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; private set; }

        /// <inheritdoc/>
        public bool HasError => Error != null;

        /// <inheritdoc/>
        [JsonProperty]
        public string SipAddress { get; private set; }

        /// <inheritdoc/>
        public Task<IResponseMessage> DeleteAsync() => TeamsApi.DeleteRoomAsync(Id);

        /// <inheritdoc/>
        public Task<Room> UpdateAsync(string title) => TeamsApi.UpdateRoomAsync(Id, title);

        /// <inheritdoc/>
        public Task<MeetingDetails> GetMeetingDetailsAsync()
            => TeamsApi.GetMeetingDetailsAsync(Id);

        /// <inheritdoc/>
        public async Task<IListResult<Message>> GetMessagesAsync(int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default, DateTimeOffset before = default, string beforeMessage = "")
                => await TeamsApi.GetRoomMessagesAsync(Id, max, userMentioned, mentionedPeople, before, beforeMessage);

        /// <inheritdoc/>
        public async Task<IMessage> SendPlainMessageAsync(string text)
            => await TeamsApi.SendMessageAsync(
                new MessageBuilder()
                    .SendToRoom(Id)
                    .WithText(text)
                    .Build());

        /// <inheritdoc/>
        public async Task<IMessage> SendMessageAsync(string markdown)
           => await TeamsApi.SendMessageAsync(
                new MessageBuilder()
                    .SendToRoom(Id)
                    .WithMarkdown(markdown)
                    .Build());

        /// <inheritdoc/>
        public async Task<IListResult<RoomMembership>> GetMembershipsAsync(int max = 100)
            => await TeamsApi.GetRoomMembershipsAsync(Id, max);

        /// <inheritdoc/>
        public async Task<RoomMembership> AddUserAsync(string userIdOrEmail, bool isModerator = false)
            => await TeamsApi.AddUserToRoomAsync(Id, userIdOrEmail, isModerator);

        /// <inheritdoc/>
        public async Task<RoomMembership> UpdateUserAsync(string userIdOrEmail, bool isModerator)
        {
            var membership = await FindMembershipByIdOrEmailAsync(userIdOrEmail);
            return await membership.UpdateAsync(isModerator);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> RemoveUserAsync(string userIdOrEmail)
        {
            var membership = await FindMembershipByIdOrEmailAsync(userIdOrEmail);
            return await membership.DeleteAsync();
        }

        private async Task<RoomMembership> FindMembershipByIdOrEmailAsync(string userIdOrEmail)
        {
            var memberships = await TeamsApi.GetRoomMembershipsAsync(Id);
            var membership = memberships.Items.FirstOrDefault(x => x.PersonEmail == userIdOrEmail || x.PersonId == userIdOrEmail);

            if (membership == null)
                throw new KeyNotFoundException("User not found by ID or Email.");

            return membership;
        }

        /// <inheritdoc/>
        public async Task<IListResult<Message>> GetMessagesBeforeDateAsync(DateTimeOffset before, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = null)
            => await TeamsApi.GetRoomMessagesBeforeDateAsync(Id, before, max, userMentioned, mentionedPeople);

        /// <inheritdoc/>
        public async Task<IListResult<Message>> GetMessagesBeforeMessageAsync(string messageId, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = null)
            => await TeamsApi.GetRoomMessagesBeforeMessageAsync(Id, messageId, max, userMentioned, mentionedPeople);

        /// <inheritdoc/>
        public async Task<RoomMembership> AddUserAsync(IPerson user, bool isModerator = false)
            => await AddUserAsync(user.Id, isModerator);

        /// <inheritdoc/>
        public async Task<IResponseMessage> RemoveUserAsync(IPerson user)
            => await RemoveUserAsync(user.Id);

        /// <inheritdoc/>
        public async Task<Webhook> AddMessageCreatedWebhookAsync(string name, string targetUrl, string personIdFilter = "",
            string personEmailFilter = "", IEnumerable<string> mentionedPeople = null, bool hasFiles = false, string secret = "")
        {
            var filter = await BuildFilterAsync(personIdFilter, personEmailFilter, mentionedPeople, hasFiles);
            return await CreateWebhookAsync(name, targetUrl, WebhookResource.Messages, EventType.Created, filter, secret);
        }

        /// <inheritdoc/>
        public async Task<Webhook> AddMessageDeletedWebhookAsync(string name, string targetUrl, string personIdFilter = "",
            string personEmailFilter = "", IEnumerable<string> mentionedPeople = null, bool hasFiles = false, string secret = "")
        {
            var filter = await BuildFilterAsync(personIdFilter, personEmailFilter, mentionedPeople, hasFiles);
            return await CreateWebhookAsync(name, targetUrl, WebhookResource.Messages, EventType.Deleted, filter, secret);
        }

        /// <inheritdoc/>
        public async Task<Webhook> AddUserAddedWebhookAsync(string name, string targetUrl, string personIdFilter = "", string personEmailFilter = "", bool isModerator = false, string secret = "")
        {
            var filter = await BuildFilterAsync(personIdFilter, personEmailFilter, isModerator: isModerator);
            return await CreateWebhookAsync(name, targetUrl, WebhookResource.Messages, EventType.Created, filter, secret);
        }

        /// <inheritdoc/>
        public async Task<Webhook> AddUserRemovedWebhookAsync(string name, string targetUrl, string personIdFilter = "", string personEmailFilter = "", bool isModerator = false, string secret = "")
        {
            var filter = await BuildFilterAsync(personIdFilter, personEmailFilter, isModerator: isModerator);
            return await CreateWebhookAsync(name, targetUrl, WebhookResource.Messages, EventType.Deleted, filter, secret);
        }

        /// <inheritdoc/>
        private async Task<string> BuildFilterAsync(string personId = "", string personEmail = "", IEnumerable<string> mentionedPeople = null,
            bool hasFiles = false, bool isModerator = false)
        {
            var filters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("roomId", Id)
            };

            if (!string.IsNullOrEmpty(personId))
                filters.Add(new KeyValuePair<string, string>(nameof(personId), personId));

            if (!string.IsNullOrEmpty(personEmail))
                filters.Add(new KeyValuePair<string, string>(nameof(personEmail), personEmail));

            if (mentionedPeople != null && mentionedPeople.Any())
                filters.Add(new KeyValuePair<string, string>(nameof(mentionedPeople), string.Join(",", mentionedPeople)));

            if (hasFiles)
                filters.Add(new KeyValuePair<string, string>(nameof(hasFiles), hasFiles.ToString().FirstCharToLower()));

            if (isModerator)
                filters.Add(new KeyValuePair<string, string>(nameof(isModerator), isModerator.ToString().FirstCharToLower()));

            using (var content = new FormUrlEncodedContent(filters))
            {
                return await content.ReadAsStringAsync();
            }
        }

        /// <inheritdoc/>
        private async Task<Webhook> CreateWebhookAsync(string name, string targetUrl, WebhookResource resource,
            EventType eventType, string filter, string secret = "")
            => await TeamsApi.CreateWebhookAsync(name, targetUrl, resource, eventType, filter, secret);
    }
}
