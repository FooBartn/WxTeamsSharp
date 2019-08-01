using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Interfaces.Teams;
using WxTeamsSharp.Models.Memberships;
using WxTeamsSharp.Models.Teams;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string teamMembershipUrl = "/team/memberships";
        private static readonly string teamsUrl = "/teams";

        /// <summary>
        /// Deletes a team membership, by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the team membership.</param>
        /// <returns>This method returns a ResponseMessage with should be "OK"</returns>
        public static async Task<IResponseMessage> RemoveUserFromTeamAsync(string membershipId)
            => await TeamsClient.DeleteResultAsync<TeamMembership>($"{teamMembershipUrl}/{membershipId}");

        /// <summary>
        /// Add someone to a team by Person ID or email address; optionally making them a moderator.
        /// </summary>
        /// <param name="teamId">The team ID.</param>
        /// <param name="userIdOrEmail">The user ID or email of the person</param>
        /// <param name="isModerator">Whether or not the participant is a team moderator.</param>
        /// <returns>This method returns the added user's Membership object</returns>
        public static async Task<IMembership> AddUserToTeamAsync(string teamId, string userIdOrEmail, bool isModerator = false)
        {
            var props = new MembershipParams { TeamId = teamId, IsModerator = isModerator };

            if (RegexUtilities.IsValidEmail(userIdOrEmail))
                props.PersonEmail = userIdOrEmail;
            else
                props.PersonId = userIdOrEmail;

            return await TeamsClient.PostResultAsync<TeamMembership, MembershipParams>(teamMembershipUrl, props);
        }

        /// <summary>
        /// Shows details for a team membership, by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the team membership.</param>
        /// <returns>This method returns a Membership</returns>
        public static async Task<IMembership> GetTeamMembership(string membershipId)
            => await TeamsClient.GetResultAsync<TeamMembership>($"{teamMembershipUrl}/{membershipId}");

        /// <summary>
        /// Updates a team membership, by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the team membership.</param>
        /// <param name="isModerator">Whether or not the participant is a team moderator.</param>
        /// <returns>This method returns the updated Membership</returns>
        public static async Task<IMembership> UpdateTeamMembership(string membershipId, bool isModerator)
        {
            var props = new MembershipParams { IsModerator = isModerator };
            return await TeamsClient.PutResultAsync<TeamMembership, MembershipParams>($"{teamMembershipUrl}/{membershipId}", props);
        }

        /// <summary>
        /// Lists all team memberships for a given team, specified by the teamId query parameter.
        /// </summary>
        /// <param name="teamId">List memberships for a team, by ID.</param>
        /// <param name="max">Limit the maximum number of team memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Team Memberships</returns>
        public static async Task<IListResult<IMembership>> GetTeamMembershipsAsync(string teamId, int max = 100)
        {
            var teamParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(teamId), teamId),
                new KeyValuePair<string, string>(nameof(max), max.ToString())
            };

            var path = await GetPathWithQueryAsync(teamMembershipUrl, teamParams);
            return await TeamsClient.GetResultsAsync<TeamMembership, IMembership>(path);
        }

        /// <summary>
        /// Lists teams to which the authenticated user belongs.
        /// </summary>
        /// <param name="max">Limit the maximum number of teams in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Teams</returns>
        public static async Task<IListResult<ITeam>> GetTeamsAsync(int max = 100)
        {
            var teamParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(max), max.ToString())
            };

            var path = await GetPathWithQueryAsync(teamsUrl, teamParams);
            return await TeamsClient.GetResultsAsync<Team, ITeam>(path);
        }

        /// <summary>
        /// Shows details for a team, by ID.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <returns>This method returns a Team</returns>
        public static async Task<ITeam> GetTeamAsync(string teamId)
            => await TeamsClient.GetResultAsync<Team>($"{teamsUrl}/{teamId}");

        /// <summary>
        /// Creates a team. The authenticated user is automatically added as a member of the team.
        /// </summary>
        /// <param name="name">A user-friendly name for the team.</param>
        /// <returns>This method returns the created Team</returns>
        public static async Task<ITeam> CreateTeamAsync(string name)
        {
            var teamParams = new TeamParams { Name = name };
            return await TeamsClient.PostResultAsync<Team, TeamParams>(teamsUrl, teamParams);
        }

        /// <summary>
        /// Updates details for a team, by ID.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <param name="name">A user-friendly name for the team.</param>
        /// <returns>This method returns an updated Team</returns>
        public static async Task<ITeam> UpdateTeamAsync(string teamId, string name)
        {
            var teamParams = new TeamParams { Name = name };
            return await TeamsClient.PutResultAsync<Team, TeamParams>($"{teamsUrl}/{teamId}", teamParams);
        }

        /// <summary>
        /// Deletes a team, by ID.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <returns>This method returns a ResponseMessage that should be "OK"</returns>
        public static async Task<IResponseMessage> DeleteTeamAsync(string teamId)
            => await TeamsClient.DeleteResultAsync<Team>($"{teamsUrl}/{teamId}");
    }
}
