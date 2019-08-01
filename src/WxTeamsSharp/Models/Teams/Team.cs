using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Interfaces.Teams;
using WxTeamsSharp.Models.General;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.Teams
{
    /// <inheritdoc/>
    internal class Team : ITeam
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync()
            => await WxTeamsApi.DeleteTeamAsync(Id);

        /// <inheritdoc/>
        public async Task<IMembership> AddUserAsync(string userIdOrEmail, bool isModerator = false)
            => await WxTeamsApi.AddUserToTeamAsync(Id, userIdOrEmail, isModerator);

        /// <inheritdoc/>
        public async Task<IListResult<IMembership>> GetMembershipsAsync(int max = 100)
            => await WxTeamsApi.GetTeamMembershipsAsync(Id, max);

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
            var memberships = await WxTeamsApi.GetTeamMembershipsAsync(Id);
            var membership = memberships.Items.FirstOrDefault(x => x.PersonEmail == userIdOrEmail || x.PersonId == userIdOrEmail);

            if (membership == null)
                throw new KeyNotFoundException($"User not found by ID or Email in Team {Name} ({Id}).");

            return membership;
        }
    }
}
