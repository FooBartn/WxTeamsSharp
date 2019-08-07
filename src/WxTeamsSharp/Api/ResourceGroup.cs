using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.ResourceGroups;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IListResult<ResourceGroup>> GetResourceGroupsAsync(string orgId = "")
        {
            var groupParams = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrEmpty(orgId))
                groupParams.Add(new KeyValuePair<string, string>(nameof(orgId), orgId));

            var path = await GetPathWithQueryAsync(WxTeamsConstants.ResourceGroupsUrl, groupParams);
            return await TeamsClient.GetResultsAsync<ResourceGroup>(path);
        }

        /// <inheritdoc/>
        public async Task<ResourceGroup> GetResourceGroupAsync(string resourceGroupId)
            => await TeamsClient.GetResultAsync<ResourceGroup>($"{WxTeamsConstants.ResourceGroupsUrl}/{resourceGroupId}");

        /// <inheritdoc/>
        public async Task<IListResult<ResourceGroupMembership>> GetResourceGroupMembershipsAsync(int max = 100, string licenseId = "", string personId = "",
            string personOrgId = "", ResourceMembershipStatus? status = null)
        {
            var resourceParams = new List<KeyValuePair<string, string>>();

            if (max != 100)
                resourceParams.Add(new KeyValuePair<string, string>(nameof(max), max.ToString()));

            if (!string.IsNullOrEmpty(licenseId))
                resourceParams.Add(new KeyValuePair<string, string>(nameof(licenseId), licenseId));

            if (!string.IsNullOrEmpty(personId))
                resourceParams.Add(new KeyValuePair<string, string>(nameof(personId), personId));

            if (!string.IsNullOrEmpty(personOrgId))
                resourceParams.Add(new KeyValuePair<string, string>(nameof(personOrgId), personOrgId));

            if (status != null)
                resourceParams.Add(new KeyValuePair<string, string>(nameof(status), status.ToString()));

            var path = await GetPathWithQueryAsync(WxTeamsConstants.ResourceMembershipsUrl, resourceParams);
            return await TeamsClient.GetResultsAsync<ResourceGroupMembership>(path);
        }

        /// <inheritdoc/>
        public async Task<ResourceGroupMembership> GetResourceGroupMembershipAsync(string resourceGroupMembershipId)
            => await TeamsClient.GetResultAsync<ResourceGroupMembership>($"{WxTeamsConstants.ResourceMembershipsUrl}/{resourceGroupMembershipId}");

        /// <inheritdoc/>
        public async Task<ResourceGroupMembership> UpdateResourceGroupMembershipAsync(string resourceGroupMembershipId,
            string resourceGroupId, string licenseId, string personId, string personOrgId, ResourceMembershipStatus status)
        {
            var membershipParams = new ResourceMembershipParams
            {
                LicenseId = licenseId,
                PersonId = personId,
                PersonOrgId = personOrgId,
                ResourceGroupId = resourceGroupId,
                Status = status
            };

            return await TeamsClient.PutResultAsync<ResourceGroupMembership, ResourceMembershipParams>(
                    $"{WxTeamsConstants.ResourceMembershipsUrl}/{resourceGroupMembershipId}",
                    membershipParams);
        }
    }
}
