using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Organizations;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IListResult<Organization>> GetOrganizationsAsync()
            => await TeamsClient.GetResultsAsync<Organization>(WxTeamsConstants.OrganizationsUrl);

        /// <inheritdoc/>
        public async Task<Organization> GetOrganizationAsync(string organizationId)
            => await TeamsClient.GetResultAsync<Organization>($"{WxTeamsConstants.OrganizationsUrl}/{organizationId}");
    }
}
