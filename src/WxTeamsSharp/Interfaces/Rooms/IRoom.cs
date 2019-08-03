using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Interfaces.Webhooks;

namespace WxTeamsSharp.Interfaces.Rooms
{
    /// <summary>
    /// Rooms are virtual meeting places where people post messages and collaborate to get work done. 
    /// This API is used to manage the rooms themselves. Rooms are created and deleted with this API. 
    /// You can also update a room to change its title, for example.
    ///
    /// To create a team room, specify the a teamId in the POST payload.Note that once a room is added to a team, it cannot be moved
    /// </summary>
    public interface IRoom : IMessageable, IListable
    {
        /// <summary>
        /// A unique identifier for the room.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// A user-friendly name for the room.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The room type.
        /// </summary>
        RoomType Type { get; }

        /// <summary>
        /// Whether the room is moderated (locked) or not.
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// The ID for the team with which this room is associated.
        /// </summary>
        string TeamId { get; }

        /// <summary>
        /// The date and time of the room's last activity.
        /// </summary>
        DateTimeOffset LastActivity { get; }

        /// <summary>
        /// The ID of the person who created this room.
        /// </summary>
        string CreatorId { get; }

        /// <summary>
        /// The date and time the room was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// The SIP URI for the room.
        /// </summary>
        string SipAddress { get; }

        /// <summary>
        /// Deletes a room, by ID. Deleted rooms cannot be recovered.
        /// Deleting a room that is part of a team will archive the room instead.
        /// </summary>
        /// <returns>This method returns a ResponseMessage that should be "OK"</returns>
        Task<IResponseMessage> DeleteAsync();

        /// <summary>
        /// Updates details for this room
        /// </summary>
        /// <param name="title">A user-friendly name for the room.</param>
        /// <returns>This method returns the updated Room</returns>
        Task<IRoom> UpdateAsync(string title);

        /// <summary>
        /// Shows Webex meeting details for this room such as the SIP address, meeting URL, toll-free and toll dial-in numbers.
        /// </summary>
        /// <returns>This method returns the Meeting Details for this room</returns>
        Task<IMeetingDetails> GetMeetingDetailsAsync();

        /// <summary>
        /// Lists all messages in this room
        /// </summary>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID</param>
        /// <param name="before">List messages sent before a date and time.</param>
        /// <param name="beforeMessage">List messages sent before a message, by ID.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        Task<IListResult<IMessage>> GetMessagesAsync(int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default, DateTimeOffset before = default, string beforeMessage = "");

        /// <summary>
        /// Lists all messages in this room before a specific date and time. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="before">List messages sent before a date and time.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        Task<IListResult<IMessage>> GetMessagesBeforeDateAsync(DateTimeOffset before, int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default);

        /// <summary>
        /// Lists all messages in this room before a specific message. 
        /// Each message will include content attachments if present.
        /// The list sorts the messages in descending order by creation date.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="messageId">List messages sent before a message, by ID.</param>
        /// <param name="max">Limit the maximum number of messages in the response.</param>
        /// <param name="userMentioned">List messages where the authenticated user is mentioned</param>
        /// <param name="mentionedPeople">List messages with these people mentioned, by ID</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Messages</returns>
        Task<IListResult<IMessage>> GetMessagesBeforeMessageAsync(string messageId, int max = 50, bool userMentioned = false,
            ICollection<string> mentionedPeople = default);

        /// <summary>
        /// Lists this room's memberships.
        /// </summary>
        /// <param name="max">Limit the maximum number of memberships in the response.</param>
        /// <returns></returns>
        Task<IListResult<IMembership>> GetMembershipsAsync(int max = 100);

        /// <summary>
        /// Add someone to this room by user ID or email address; optionally making them a moderator.
        /// </summary>
        /// <param name="userIdOrEmail">User ID or email</param>
        /// <param name="isModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>This method will return the new Membership of the added user</returns>
        Task<IMembership> AddUserAsync(string userIdOrEmail, bool isModerator = false);

        /// <summary>
        /// Add someone to this room by person object; optionally making them a moderator.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isModerator"></param>
        /// <returns></returns>
        Task<IMembership> AddUserAsync(IPerson user, bool isModerator = false);

        /// <summary>
        /// Updates properties for a user in this room
        /// </summary>
        /// <param name="userIdOrEmail">User ID or email</param>
        /// <param name="isModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>This method will return the updated Membership</returns>
        Task<IMembership> UpdateUserAsync(string userIdOrEmail, bool isModerator);

        /// <summary>
        /// Remove a user from this room
        /// </summary>
        /// <param name="userIdOrEmail">User ID or email</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> RemoveUserAsync(string userIdOrEmail);

        /// <summary>
        /// Remove a user from this room
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IResponseMessage> RemoveUserAsync(IPerson user);

        /// <summary>
        /// Add a new webhook for message created events that is filtered for this room by default.
        /// </summary>
        /// <param name="name">Name of the webhook</param>
        /// <param name="targetUrl">Url to receive the webhook POST at</param>
        /// <param name="personIdFilter">limit to a particular person, by ID</param>
        /// <param name="personEmailFilter">limit to a particular person, by email</param>
        /// <param name="mentionedPeople">limit to messages which contain these mentioned people, by person ID; accepts me as a shorthand for your own person ID</param>
        /// <param name="hasFiles">limit to messages which contain file content attachments </param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <returns>This method returns the created Webhook</returns>
        Task<IWebhook> AddMessageCreatedWebhookAsync(string name, string targetUrl, 
            string personIdFilter = "", string personEmailFilter = "", IEnumerable<string> mentionedPeople = default,
            bool hasFiles = false, string secret = "");

        /// <summary>
        /// Add a new webhook for message deleted events that is filtered for this room by default.
        /// </summary>
        /// <param name="name">Name of the webhook</param>
        /// <param name="targetUrl">Url to receive the webhook POST at</param>
        /// <param name="personIdFilter">limit to a particular person, by ID</param>
        /// <param name="personEmailFilter">limit to a particular person, by email</param>
        /// <param name="mentionedPeople">limit to messages which contain these mentioned people, by person ID; accepts me as a shorthand for your own person ID</param>
        /// <param name="hasFiles">limit to messages which contain file content attachments </param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <returns>This method returns the created Webhook</returns>
        Task<IWebhook> AddMessageDeletedWebhookAsync(string name, string targetUrl,
            string personIdFilter = "", string personEmailFilter = "", IEnumerable<string> mentionedPeople = default,
            bool hasFiles = false, string secret = "");

        /// <summary>
        /// Add a new webhook for user added events that is filtered for this room by default.
        /// </summary>
        /// <param name="name">Name of the webhook</param>
        /// <param name="targetUrl">Url to receive the webhook POST at</param>
        /// <param name="personIdFilter">limit to a particular person, by ID</param>
        /// <param name="personEmailFilter">limit to a particular person, by email</param>
        /// <param name="isModerator">limit to moderators of a room</param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <returns>This method returns the created Webhook</returns>
        Task<IWebhook> AddUserAddedWebhookAsync(string name, string targetUrl,
            string personIdFilter = "", string personEmailFilter = "", bool isModerator = false, 
            string secret = "");

        /// <summary>
        /// Add a new webhook for user added events that is filtered for this room by default.
        /// </summary>
        /// <param name="name">Name of the webhook</param>
        /// <param name="targetUrl">Url to receive the webhook POST at</param>
        /// <param name="personIdFilter">limit to a particular person, by ID</param>
        /// <param name="personEmailFilter">limit to a particular person, by email</param>
        /// <param name="isModerator">limit to moderators of a room</param>
        /// <param name="secret">The secret used to generate payload signature.</param>
        /// <returns>This method returns the created Webhook</returns>
        Task<IWebhook> AddUserRemovedWebhookAsync(string name, string targetUrl,
            string personIdFilter = "", string personEmailFilter = "", bool isModerator = false,
            string secret = "");
    }
}
