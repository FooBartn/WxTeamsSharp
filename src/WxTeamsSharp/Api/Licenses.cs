using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Licenses;
using WxTeamsSharp.Models.Licenses;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string licenseUrl = "/licenses";

        /// <summary>
        /// List all licenses for a given organization. 
        /// If no orgId is specified, the default is the organization of the authenticated user.
        /// </summary>
        /// <returns>The method returns an IListResult object whose Items property is a list of Licenses</returns>
        public static async Task<IListResult<ILicense>> GetLicensesAsync()
            => await TeamsClient.GetResultsAsync<License, ILicense>(licenseUrl);

        /// <summary>
        /// Shows details for a license, by ID.
        /// </summary>
        /// <param name="licenseId">The unique identifier for the license.</param>
        /// <returns>The method returns a License</returns>
        public static async Task<ILicense> GetLicenseAsync(string licenseId)
            => await TeamsClient.GetResultAsync<License>($"{licenseUrl}/{licenseId}");
    }
}
