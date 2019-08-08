using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Models.People;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteUserAsync(string personId)
            => await TeamsClient.DeleteResultAsync<Person>($"{WxTeamsConstants.PeopleUrl}/{personId}");

        /// <inheritdoc/>
        public async Task<Person> UpdateUserAsync(string personId, IUpdateablePerson personParams)
            => await TeamsClient.PutResultAsync<Person, IUpdateablePerson>($"{WxTeamsConstants.PeopleUrl}/{personId}", personParams);

        /// <inheritdoc/>
        public async Task<Person> CreateUserAsync(ICreateablePerson personParams)
            => await TeamsClient.PostResultAsync<Person, ICreateablePerson>(WxTeamsConstants.PeopleUrl, personParams);

        /// <inheritdoc/>
        public async Task<Person> GetMyUserDetails()
            => await TeamsClient.GetResultAsync<Person>($"{WxTeamsConstants.PeopleUrl}/me");

        /// <inheritdoc/>
        public async Task<Person> GetUserAsync(string userId)
            => await TeamsClient.GetResultAsync<Person>($"{WxTeamsConstants.PeopleUrl}/{userId}");

        /// <inheritdoc/>
        public async Task<IListResult<Person>> GetUsersByEmailAsync(string email, int max = 100)
            => await GetUsersAsync(email, max: max);

        /// <inheritdoc/>
        public async Task<IListResult<Person>> GetUsersByDisplayNameAsync(string displayName, int max = 100)
            => await GetUsersAsync(displayName: displayName, max: max);

        /// <inheritdoc/>
        public async Task<IListResult<Person>> GetUsersByIdListAsync(IEnumerable<string> ids, int max = 100)
            => await GetUsersAsync(id: ids, max: max);

        /// <inheritdoc/>
        public async Task<IListResult<Person>> GetUsersByOrgIdAsync(string orgId, int max = 100)
            => await GetUsersAsync(orgId: orgId, max: max);

        /// <inheritdoc/>
        public async Task<IListResult<Person>> GetUsersAsync(string email = "", string displayName = "",
            IEnumerable<string> id = default, string orgId = "", int max = 100)
        {
            var roomParams = new List<KeyValuePair<string, string>>();

            if (id != null && id.Any() && IdIsAcceptableLength(id))
                roomParams.Add(new KeyValuePair<string, string>(nameof(id), string.Join(",", id)));

            if (max != 100)
                roomParams.Add(new KeyValuePair<string, string>(nameof(max), max.ToString()));

            if (!string.IsNullOrEmpty(email))
                roomParams.Add(new KeyValuePair<string, string>(nameof(email), email));

            if (!string.IsNullOrEmpty(displayName))
                roomParams.Add(new KeyValuePair<string, string>(nameof(displayName), displayName));

            if (!string.IsNullOrEmpty(orgId))
                roomParams.Add(new KeyValuePair<string, string>(nameof(orgId), orgId));

            var path = await GetPathWithQueryAsync(WxTeamsConstants.PeopleUrl, roomParams);
            return await TeamsClient.GetResultsAsync<Person>(path);
        }

        // API limits to 85 IDs
        private static bool IdIsAcceptableLength(IEnumerable<string> id)
        {
            if (id.Count() > 85)
                throw new ArgumentOutOfRangeException(nameof(id), "id parameter accepts a max of 85 person IDs");
            else
                return true;
        }
    }
}
