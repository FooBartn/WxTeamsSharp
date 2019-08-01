using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Interfaces.Rooms;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Models.Messages;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.Rooms
{
    /// <inheritdoc/>
    internal class Room : IRoom
    {
        /// <inheritdoc/>
        public string Id { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Title { get; set; } = string.Empty;

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomType Type { get; set; }

        /// <inheritdoc/>
        public bool IsLocked { get; set; }

        /// <inheritdoc/>
        public string TeamId { get; set; } = string.Empty;

        /// <inheritdoc/>
        public DateTimeOffset LastActivity { get; set; }

        /// <inheritdoc/>
        public string CreatorId { get; set; } = string.Empty;

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }

        /// <inheritdoc/>
        public string SipAddress { get; set; }

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
    }
}
