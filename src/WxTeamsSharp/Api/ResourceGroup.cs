using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Interfaces.ResourceGroups;
using WxTeamsSharp.Models.ResourceGroups;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string resourceGroupUrl = "/resourceGroups";
        private static readonly string resourceMembershipUrl = "/resourceGroup/memberships";

        /// <summary>
        /// List resource groups.
        /// </summary>
        /// <param name="orgId">List resource groups in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Resource Groups</returns>
        public static async Task<IListResult<IResourceGroup>> GetResourceGroupsAsync(string orgId = "")
        {
            var groupParams = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrEmpty(orgId))
                groupParams.Add(new KeyValuePair<string, string>(nameof(orgId), orgId));

            var path = await GetPathWithQueryAsync(resourceGroupUrl, groupParams);
            return await TeamsClient.GetResultsAsync<ResourceGroup, IResourceGroup>(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceGroupId"></param>
        /// <returns>This method returns a Resource Group</returns>
        public static async Task<IResourceGroup> GetResourceGroupAsync(string resourceGroupId)
            => await TeamsClient.GetResultAsync<ResourceGroup>($"{resourceGroupUrl}/{resourceGroupId}");

        /// <summary>
        /// Lists all resource group memberships for an organization.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="licenseId">List resource group memberships for a license, by ID.</param>
        /// <param name="personId">List resource group memberships for a person, by ID.</param>
        /// <param name="personOrgId">List resource group memberships for an organization, by ID.</param>
        /// <param name="status">Limit resource group memberships to a specific status.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Resource Group Memberships</returns>
        public static async Task<IListResult<IResourceGroupMembership>> GetResourceGroupMembershipsAsync(int max = 100, string licenseId = "", string personId = "",
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

            var path = await GetPathWithQueryAsync(resourceMembershipUrl, resourceParams);
            return await TeamsClient.GetResultsAsync<ResourceGroupMembership, IResourceGroupMembership>(path);
        }

        /// <summary>
        /// Shows details for a resource group membership, by ID.
        /// </summary>
        /// <param name="resourceGroupMembershipId">The unique identifier for the resource group membership.</param>
        /// <returns>This method returns a Resource Group Membership</returns>
        public static async Task<IResourceGroupMembership> GetResourceGroupMembershipAsync(string resourceGroupMembershipId)
            => await TeamsClient.GetResultAsync<ResourceGroupMembership>($"{resourceMembershipUrl}/{resourceGroupMembershipId}");

        /// <summary>
        /// Updates a resource group membership, by ID.
        /// </summary>
        /// <param name="resourceGroupMembershipId">The unique identifier for the resource group membership.</param>
        /// <param name="resourceGroupId">The resource group ID.</param>
        /// <param name="licenseId">The license ID.</param>
        /// <param name="personId">The person ID.</param>
        /// <param name="personOrgId">The organization ID of the person.</param>
        /// <param name="status">The activation status of the resource group membership.</param>
        /// <returns>This method returns the updated Resource Group Membership</returns>
        public static async Task<IResourceGroupMembership> UpdateResourceGroupMembershipAsync(string resourceGroupMembershipId,
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
                    $"{resourceMembershipUrl}/{resourceGroupMembershipId}",
                    membershipParams);
        }
    }
}
