using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Interfaces.Teams;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Models.Memberships;

namespace WxTeamsSharp.Models.Teams
{
    /// <inheritdoc/>
    public class Team : TeamsObject, ITeam
    {

        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Name { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset Created { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync()
            => await TeamsApi.DeleteTeamAsync(Id);

        /// <inheritdoc/>
        public async Task<IMembership<TeamMembership>> AddUserAsync(string userIdOrEmail, bool isModerator = false)
            => await TeamsApi.AddUserToTeamAsync(Id, userIdOrEmail, isModerator);

        /// <inheritdoc/>
        public async Task<IListResult<TeamMembership>> GetMembershipsAsync(int max = 100)
            => await TeamsApi.GetTeamMembershipsAsync(Id, max);

        /// <inheritdoc/>
        public async Task<IMembership<TeamMembership>> UpdateUserAsync(string userIdOrEmail, bool isModerator)
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

        private async Task<IMembership<TeamMembership>> FindMembershipByIdOrEmailAsync(string userIdOrEmail)
        {
            var memberships = await TeamsApi.GetTeamMembershipsAsync(Id);
            var membership = memberships.Items.FirstOrDefault(x => x.PersonEmail == userIdOrEmail || x.PersonId == userIdOrEmail);

            if (membership == null)
                throw new KeyNotFoundException($"User not found by ID or Email in Team {Name} ({Id}).");

            return membership;
        }
    }
}
