using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Roles;
using WxTeamsSharp.Models.Roles;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string roleUrl = "/roles";

        /// <summary>
        /// List all roles.
        /// This roles resource can be accessed only by an admin.
        /// </summary>
        /// <returns>This method returns an IListResult object whose Items property is a list of Roles</returns>
        public static async Task<IListResult<IRole>> GetRolesAsync()
            => await TeamsClient.GetResultsAsync<Role, IRole>(roleUrl);

        /// <summary>
        /// Shows details for a role, by ID.
        /// This roles resource can be accessed only by an admin.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>This method returns a Role</returns>
        public static async Task<IRole> GetRoleAsync(string roleId)
            => await TeamsClient.GetResultAsync<Role>($"{roleUrl}/{roleId}");
    }
}
