using System;
using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.Memberships
{
    /// <summary>
    /// Memberships represent a person's relationship to a room. Use this API to list members 
    /// of any room that you're in or create memberships to invite someone to a room. 
    /// Memberships can also be updated to make someone a moderator or deleted to remove them from the room.
    ///
    /// Just like in the Webex Teams app, you must be a member of the room in order to list its memberships or invite people.
    /// </summary>
    public interface IMembership : IListable
    {
        /// <summary>
        /// A unique identifier for the membership.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The room ID.
        /// </summary>
        string RoomId { get; }

        /// <summary>
        /// The person ID.
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// The email address of the person.
        /// </summary>
        string PersonEmail { get; }

        /// <summary>
        /// The display name of the person.
        /// </summary>
        string PersonDisplayName { get; }

        /// <summary>
        /// The organization ID of the person.
        /// </summary>
        string PersonOrgId { get; }

        /// <summary>
        /// Whether or not the participant is a room moderator.
        /// </summary>
        bool IsModerator { get; }

        /// <summary>
        /// The date and time when the membership was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// Updates properties for this membership
        /// </summary>
        /// <param name="isModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>This method returns the updated Membership</returns>
        Task<IMembership> UpdateAsync(bool isModerator);

        /// <summary>
        /// Delete this membership
        /// </summary>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteAsync();
    }
}
