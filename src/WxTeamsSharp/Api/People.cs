using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Models.People;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string peopleUrl = "/people";

        /// <summary>
        /// Remove a person from the system. Only an admin can remove a person.
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <returns>This method returns a response message which should be "OK"</returns>
        public static async Task<IResponseMessage> DeleteUserAsync(string personId)
            => await TeamsClient.DeleteResultAsync<Person>($"{peopleUrl}/{personId}");

        /// <summary>
        /// Update details for a person, by ID
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <param name="personParams">Get from PersonUpdater.Update(IPerson)</param>
        /// <returns>This method returns the updated Person object</returns>
        public static async Task<IPerson> UpdateUserAsync(string personId, IUpdateablePerson personParams)
            => await TeamsClient.PutResultAsync<Person, IUpdateablePerson>($"{peopleUrl}/{personId}", personParams);

        /// <summary>
        /// Create a new user account for a given organization. Only an admin can create a new user account.
        /// Currently, users may have only one email address associated with their account.
        /// </summary>
        /// <param name="personParams">Get from PersonBuilder.New()</param>
        /// <returns>This method returns the created Person object</returns>
        public static async Task<IPerson> CreateUserAsync(ICreateablePerson personParams)
            => await TeamsClient.PostResultAsync<Person, ICreateablePerson>(peopleUrl, personParams);

        /// <summary>
        /// Show the profile for the authenticated user.
        /// </summary>
        /// <returns>This method returns the user's Person object</returns>
        public static async Task<IPerson> GetMyUserDetails()
            => await TeamsClient.GetResultAsync<Person>($"{peopleUrl}/me");

        /// <summary>
        /// Shows details for a person, by ID
        /// </summary>
        /// <param name="userId">A unique identifier for the person.</param>
        /// <returns>This method returns a Person</returns>
        public static async Task<IPerson> GetUserAsync(string userId)
            => await TeamsClient.GetResultAsync<Person>($"{peopleUrl}/{userId}");

        /// <summary>
        /// List people in your organization with this email address
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="email">List people with this email address.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        public static async Task<IListResult<IPerson>> GetUsersByEmail(string email, int max = 100)
            => await GetUsersAsync(email, max: max);

        /// <summary>
        /// List people in your organization with this display name
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="displayName">List people whose name starts with this string.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        public static async Task<IListResult<IPerson>> GetUsersByDisplayName(string displayName, int max = 100)
            => await GetUsersAsync(displayName: displayName, max: max);

        /// <summary>
        /// List people in your organization that match this list of IDs.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="ids">List people by ID. Accepts up to 85 person IDs separated by commas.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        public static async Task<IListResult<IPerson>> GetUsersByIdList(IEnumerable<string> ids, int max = 100)
            => await GetUsersAsync(id: ids, max: max);

        /// <summary>
        /// List people in this organization. Only admin users of another organization (such as partners) may use this.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="orgId">A unique identifier for the organization</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        public static async Task<IListResult<IPerson>> GetUsersByOrgId(string orgId, int max = 100)
            => await GetUsersAsync(orgId: orgId, max: max);

        /// <summary>
        /// List people in your organization. For most users, either the email or displayName parameter is required. 
        /// Admin users can omit these fields and list all users in their organization.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="email">List people with this email address. For non-admin requests, either this or displayName are required.</param>
        /// <param name="displayName">List people whose name starts with this string. For non-admin requests, either this or email are required.</param>
        /// <param name="id">List people by ID. Accepts up to 85 person IDs separated by commas.</param>
        /// <param name="orgId">List people in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        /// <param name="max">Limit the maximum number of people in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of People</returns>
        public static async Task<IListResult<IPerson>> GetUsersAsync(string email = "", string displayName = "",
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

            var path = await GetPathWithQueryAsync(peopleUrl, roomParams);
            return await TeamsClient.GetResultsAsync<Person, IPerson>(path);
        }

        private static bool IdIsAcceptableLength(IEnumerable<string> id)
        {
            if (id.Count() > 85)
                throw new ArgumentOutOfRangeException(nameof(id), "id parameter accepts a max of 85 person IDs");
            else
                return true;
        }
    }
}
