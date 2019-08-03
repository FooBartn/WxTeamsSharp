using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Interfaces.Rooms;
using WxTeamsSharp.Interfaces.Webhooks;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Utilities;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.Rooms
{
    /// <inheritdoc/>
    public class Room : IRoom
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
        [JsonProperty]
        public string SipAddress { get; private set; }

        /// <inheritdoc/>
        public Task<IResponseMessage> DeleteAsync() => WxTeamsApi.DeleteRoomAsync(Id);

        /// <inheritdoc/>
        public Task<IRoom> UpdateAsync(string title) => WxTeamsApi.UpdateRoomAsync(Id, title);

        /// <inheritdoc/>
        public Task<IMeetingDetails> GetMeetingDetailsAsync()
            => WxTeamsApi.GetMeetingDetailsAsync(Id);

        /// <inheritdoc/>
        public async Task<IListResult<IMessage>> GetMessagesAsync(int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default, DateTimeOffset before = default, string beforeMessage = "")
                => await WxTeamsApi.GetRoomMessagesAsync(Id, max, userMentioned, mentionedPeople, before, beforeMessage);

        /// <inheritdoc/>
        public async Task<IMessage> SendPlainMessageAsync(string text)
            => await MessageBuilder.New()
                .SendToRoom(Id)
                .WithText(text)
                .Build()
                .SendAsync();

        /// <inheritdoc/>
        public async Task<IMessage> SendMessageAsync(string markdown)
           => await MessageBuilder.New()
                .SendToRoom(Id)
                .WithMarkdown(markdown)
                .Build()
                .SendAsync();

        /// <inheritdoc/>
        public async Task<IListResult<IMembership>> GetMembershipsAsync(int max = 100)
            => await WxTeamsApi.GetRoomMembershipsAsync(Id, max);

        /// <inheritdoc/>
        public async Task<IMembership> AddUserAsync(string userIdOrEmail, bool isModerator = false)
            => await WxTeamsApi.AddUserToRoomAsync(Id, userIdOrEmail, isModerator);

        /// <inheritdoc/>
        public async Task<IMembership> UpdateUserAsync(string userIdOrEmail, bool isModerator)
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

        private async Task<IMembership> FindMembershipByIdOrEmailAsync(string userIdOrEmail)
        {
            var memberships = await WxTeamsApi.GetRoomMembershipsAsync(Id);
            var membership = memberships.Items.FirstOrDefault(x => x.PersonEmail == userIdOrEmail || x.PersonId == userIdOrEmail);

            if (membership == null)
                throw new KeyNotFoundException("User not found by ID or Email.");

            return membership;
        }

        /// <inheritdoc/>
        public async Task<IListResult<IMessage>> GetMessagesBeforeDateAsync(DateTimeOffset before, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = null)
            => await WxTeamsApi.GetRoomMessagesBeforeDateAsync(Id, before, max, userMentioned, mentionedPeople);

        /// <inheritdoc/>
        public async Task<IListResult<IMessage>> GetMessagesBeforeMessageAsync(string messageId, int max = 50,
            bool userMentioned = false, ICollection<string> mentionedPeople = null)
            => await WxTeamsApi.GetRoomMessagesBeforeMessageAsync(Id, messageId, max, userMentioned, mentionedPeople);

        /// <inheritdoc/>
        public async Task<IMembership> AddUserAsync(IPerson user, bool isModerator = false)
            => await AddUserAsync(user.Id, isModerator);

        /// <inheritdoc/>
        public async Task<IResponseMessage> RemoveUserAsync(IPerson user)
            => await RemoveUserAsync(user.Id);

        /// <inheritdoc/>
        public async Task<IWebhook> AddMessageCreatedWebhookAsync(string name, string targetUrl, string personIdFilter = "", 
            string personEmailFilter = "", IEnumerable<string> mentionedPeople = null, bool hasFiles = false, string secret = "")
        {
            var filter = await BuildFilterAsync(personIdFilter, personEmailFilter, mentionedPeople, hasFiles);
            return await CreateWebhookAsync(name, targetUrl, WebhookResource.Messages, EventType.Created, filter, secret);
        }

        /// <inheritdoc/>
        public async Task<IWebhook> AddMessageDeletedWebhookAsync(string name, string targetUrl, string personIdFilter = "", 
            string personEmailFilter = "", IEnumerable<string> mentionedPeople = null, bool hasFiles = false, string secret = "")
        {
            var filter = await BuildFilterAsync(personIdFilter, personEmailFilter, mentionedPeople, hasFiles);
            return await CreateWebhookAsync(name, targetUrl, WebhookResource.Messages, EventType.Deleted, filter, secret);
        }

        /// <inheritdoc/>
        public async Task<IWebhook> AddUserAddedWebhookAsync(string name, string targetUrl, string personIdFilter = "", string personEmailFilter = "", bool isModerator = false, string secret = "")
        {
            var filter = await BuildFilterAsync(personIdFilter, personEmailFilter, isModerator: isModerator);
            return await CreateWebhookAsync(name, targetUrl, WebhookResource.Messages, EventType.Created, filter, secret);
        }

        /// <inheritdoc/>
        public async Task<IWebhook> AddUserRemovedWebhookAsync(string name, string targetUrl, string personIdFilter = "", string personEmailFilter = "", bool isModerator = false, string secret = "")
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

            return await new FormUrlEncodedContent(filters).ReadAsStringAsync();
        }

        /// <inheritdoc/>
        private async Task<IWebhook> CreateWebhookAsync(string name, string targetUrl, WebhookResource resource,
            EventType eventType, string filter, string secret = "")
            => await WxTeamsApi.CreateWebhookAsync(name, targetUrl, resource, eventType, filter, secret);
    }
}
