using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Licenses;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi : IWxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IListResult<License>> GetLicensesAsync()
            => await TeamsClient.GetResultsAsync<License>(WxTeamsConstants.LicensesUrl);

        /// <inheritdoc/>
        public async Task<License> GetLicenseAsync(string licenseId)
            => await TeamsClient.GetResultAsync<License>($"{WxTeamsConstants.LicensesUrl}/{licenseId}");
    }
}
