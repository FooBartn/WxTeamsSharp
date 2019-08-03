using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.Messages
{
    /// <summary>
    /// Messages are how we communicate in a room. In Webex Teams, each message is displayed 
    /// on its own line along with a timestamp and sender information. Use this API to list, create, and delete messages.
    /// 
    /// Message can contain plain text, rich text, and a file attachment.
    /// 
    /// Just like in the Webex Teams app, you must be a member of the room in order to target it with this API.
    /// </summary>
    public interface IMessage : IListable
    {
        /// <summary>
        /// The unique identifier for the message.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The room ID of the message.
        /// </summary>
        string RoomId { get; }

        /// <summary>
        /// The type of room.
        /// direct - 1:1 room
        /// group - group room
        /// </summary>
        RoomType RoomType { get; }

        /// <summary>
        /// The person ID of the recipient when sending a private 1:1 message.
        /// </summary>
        string RecipientId { get; }

        /// <summary>
        /// The email address of the recipient when sending a private 1:1 message.
        /// </summary>
        string RecipientEmail { get; }

        /// <summary>
        /// The message, in plain text. If markdown is specified this parameter may be 
        /// optionally used to provide alternate text for UI clients that do not support rich text.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// The message, in Markdown format.
        /// </summary>
        string Markdown { get; }

        /// <summary>
        /// Public URLs for files attached to the message. Currently limited to 1 at a time.
        /// </summary>
        List<string> Files { get; }

        /// <summary>
        /// The person ID of the message author.
        /// </summary>
        string AuthorId { get; }

        /// <summary>
        /// The email address of the message author.
        /// </summary>
        string AuthorEmail { get; }

        /// <summary>
        /// People IDs for anyone mentioned in the message.
        /// </summary>
        List<string> MentionedPeople { get; }

        /// <summary>
        /// Group names for the groups mentioned in the message.
        /// </summary>
        List<string> MentionedGroups { get; }

        /// <summary>
        /// The date and time the message was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// Deletes this Message
        /// </summary>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteAsync();
    }
}
