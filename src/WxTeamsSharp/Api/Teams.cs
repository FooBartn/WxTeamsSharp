using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Memberships;
using WxTeamsSharp.Models.Teams;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IResponseMessage> RemoveUserFromTeamAsync(string membershipId)
            => await TeamsClient
                .DeleteResultAsync<TeamMembership>($"{WxTeamsConstants.TeamMembershipsUrl}/{membershipId}");

        /// <inheritdoc/>
        public async Task<TeamMembership> AddUserToTeamAsync(string teamId, string userIdOrEmail, bool isModerator = false)
        {
            var props = new MembershipParams { TeamId = teamId, IsModerator = isModerator };

            if (RegexUtilities.IsValidEmail(userIdOrEmail))
                props.PersonEmail = userIdOrEmail;
            else
                props.PersonId = userIdOrEmail;

            return await TeamsClient
                .PostResultAsync<TeamMembership, MembershipParams>(WxTeamsConstants.TeamMembershipsUrl, props);
        }

        /// <inheritdoc/>
        public async Task<TeamMembership> GetTeamMembership(string membershipId)
            => await TeamsClient.GetResultAsync<TeamMembership>($"{WxTeamsConstants.TeamMembershipsUrl}/{membershipId}");

        /// <inheritdoc/>
        public async Task<TeamMembership> UpdateTeamMembership(string membershipId, bool isModerator)
        {
            var props = new MembershipParams { IsModerator = isModerator };
            return await TeamsClient.PutResultAsync<TeamMembership, MembershipParams>($"{WxTeamsConstants.TeamMembershipsUrl}/{membershipId}", props);
        }

        /// <inheritdoc/>
        public async Task<IListResult<TeamMembership>> GetTeamMembershipsAsync(string teamId, int max = 100)
        {
            var teamParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(teamId), teamId),
                new KeyValuePair<string, string>(nameof(max), max.ToString())
            };

            var path = await GetPathWithQueryAsync(WxTeamsConstants.TeamMembershipsUrl, teamParams);
            return await TeamsClient.GetResultsAsync<TeamMembership>(path);
        }

        /// <inheritdoc/>
        public async Task<IListResult<Team>> GetTeamsAsync(int max = 100)
        {
            var teamParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(nameof(max), max.ToString())
            };

            var path = await GetPathWithQueryAsync(WxTeamsConstants.TeamsUrl, teamParams);
            return await TeamsClient.GetResultsAsync<Team>(path);
        }

        /// <inheritdoc/>
        public async Task<Team> GetTeamAsync(string teamId)
            => await TeamsClient.GetResultAsync<Team>($"{WxTeamsConstants.TeamsUrl}/{teamId}");

        /// <inheritdoc/>
        public async Task<Team> CreateTeamAsync(string name)
        {
            var teamParams = new TeamParams { Name = name };
            return await TeamsClient.PostResultAsync<Team, TeamParams>(WxTeamsConstants.TeamsUrl, teamParams);
        }

        /// <inheritdoc/>
        public async Task<Team> UpdateTeamAsync(string teamId, string name)
        {
            var teamParams = new TeamParams { Name = name };
            return await TeamsClient.PutResultAsync<Team, TeamParams>($"{WxTeamsConstants.TeamsUrl}/{teamId}", teamParams);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteTeamAsync(string teamId)
            => await TeamsClient.DeleteResultAsync<Team>($"{WxTeamsConstants.TeamsUrl}/{teamId}");
    }
}
