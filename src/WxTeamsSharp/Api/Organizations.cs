using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Organizations;
using WxTeamsSharp.Models.Organizations;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string organizationUrl = "/organizations";

        /// <summary>
        /// List all organizations visible by your account. The results will not be paginated.
        /// </summary>
        /// <returns>This method returns an IListResult object whose Items property is a list of Organizations</returns>
        public static async Task<IListResult<IOrganization>> GetOrganizationsAsync()
            => await TeamsClient.GetResultsAsync<Organization, IOrganization>(organizationUrl);

        /// <summary>
        /// Shows details for an organization, by ID.
        /// </summary>
        /// <param name="organizationId">The unique identifier for the organization.</param>
        /// <returns>This method returns an Organization</returns>
        public static async Task<IOrganization> GetOrganizationAsync(string organizationId)
            => await TeamsClient.GetResultAsync<Organization>($"{organizationUrl}/{organizationId}");
    }
}
