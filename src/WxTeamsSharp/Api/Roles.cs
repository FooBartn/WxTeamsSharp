using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Roles;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi : IWxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IListResult<Role>> GetRolesAsync()
            => await TeamsClient.GetResultsAsync<Role>(WxTeamsConstants.RolesUrl);

        /// <inheritdoc/>
        public async Task<Role> GetRoleAsync(string roleId)
            => await TeamsClient.GetResultAsync<Role>($"{WxTeamsConstants.RolesUrl}/{roleId}");
    }
}
