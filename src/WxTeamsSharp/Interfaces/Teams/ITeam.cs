using System;
using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;

namespace WxTeamsSharp.Interfaces.Teams
{
    /// <summary>
    /// Teams are groups of people with a set of rooms that are visible to all members of that team. 
    /// This API is used to manage the teams themselves. 
    /// 
    /// Teams are created and deleted with this API. You can also update a team to change its name, for example.
    /// </summary>
    public interface ITeam : IListable
    {
        /// <summary>
        /// A unique identifier for the team.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// A user-friendly name for the team.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The date and time the team was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// Lists all team memberships for this team
        /// </summary>
        /// <param name="max">Limit the maximum number of team memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Team Memberships</returns>
        Task<IListResult<IMembership>> GetMembershipsAsync(int max = 100);

        /// <summary>
        /// Add someone to this team
        /// </summary>
        /// <param name="userIdOrEmail">ID or email of the user to add</param>
        /// <param name="isModerator">Whether or not the participant is a team moderator.</param>
        /// <returns>This method returns the new Membership of the added user</returns>
        Task<IMembership> AddUserAsync(string userIdOrEmail, bool isModerator = false);

        /// <summary>
        /// Updates a user of this team
        /// </summary>
        /// <param name="userIdOrEmail">ID or email of the user to update</param>
        /// <param name="isModerator">Whether or not the participant is a team moderator.</param>
        /// <returns>This method returns the updated Membership</returns>
        Task<IMembership> UpdateUserAsync(string userIdOrEmail, bool isModerator);

        /// <summary>
        /// Removes a user from this team
        /// </summary>
        /// <param name="userIdOrEmail">ID or email of the user to update</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> RemoveUserAsync(string userIdOrEmail);

        /// <summary>
        /// Delete this team
        /// </summary>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteAsync();
    }
}
