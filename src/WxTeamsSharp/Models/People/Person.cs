using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Models.Messages;

namespace WxTeamsSharp.Models.People
{
    /// <inheritdoc/>
    public class Person : TeamsObject, IPerson
    {

        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public List<string> Emails { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(ConcreteListConverter<PhoneNumber, IPhoneNumber>))]
        public List<IPhoneNumber> PhoneNumbers { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string DisplayName { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string NickName { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string FirstName { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string LastName { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Avatar { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string OrgId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public List<string> Roles { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public List<string> Licenses { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset Created { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset LastModified { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Timezone { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset LastActivity { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public PersonStatus Status { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public bool InvitePending { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public bool LoginEnabled { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public PersonType Type { get; private set; }

        private PeopleParams GetUserParams()
        {
            var peopleParams = new PeopleParams(Id)
            {
                DisplayName = DisplayName,
                FirstName = FirstName,
                LastName = LastName,
                Avatar = Avatar,
                Emails = Emails,
                OrganizationId = OrgId,
                Licenses = new List<string>(),
                Roles = new List<string>()
            };

            foreach (var license in Licenses)
            {
                peopleParams.Licenses.Add(license);
            }

            foreach (var role in Roles)
            {
                peopleParams.Roles.Add(role);
            }

            return peopleParams;
        }

        /// <inheritdoc/>
        public async Task<IPerson> AddLicenseAsync(string licenseId)
        {
            var peopleParams = GetUserParams();

            if (peopleParams.Licenses == null)
                peopleParams.Licenses = new List<string>();

            peopleParams.Licenses.Add(licenseId);

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> AddRoleAsync(string roleId)
        {
            var peopleParams = GetUserParams();

            if (peopleParams.Roles == null)
                peopleParams.Roles = new List<string>();

            peopleParams.Roles.Add(roleId);

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync() =>
            await TeamsApi.DeleteUserAsync(Id);

        /// <inheritdoc/>
        public async Task<IPerson> ReplaceLicensesAsync(List<string> licenseIds)
        {
            var peopleParams = GetUserParams();
            peopleParams.Licenses = licenseIds;

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> ReplaceRolesAsync(List<string> roleIds)
        {
            var peopleParams = GetUserParams();
            peopleParams.Roles = roleIds;

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IMessage> SendMessageAsync(string markdown)
            => await TeamsApi.SendMessageAsync(
                new MessageBuilder()
                    .SendToUserId(Id)
                    .WithMarkdown(markdown)
                    .Build());

        /// <inheritdoc/>
        public async Task<IMessage> SendPlainMessageAsync(string text)
            => await TeamsApi.SendMessageAsync(
                new MessageBuilder()
                    .SendToUserId(Id)
                    .WithText(text)
                    .Build());

        /// <inheritdoc/>
        public async Task<IPerson> UpdateAvatarAsync(string avatar)
        {
            var peopleParams = GetUserParams();
            peopleParams.Avatar = avatar;

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateDisplayNameAsync(string displayName)
        {
            var peopleParams = GetUserParams();
            peopleParams.DisplayName = displayName;

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateEmailAsync(string email)
        {
            var peopleParams = GetUserParams();
            peopleParams.Emails = new List<string> { email };

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateFirstNameAsync(string firstName)
        {
            var peopleParams = GetUserParams();
            peopleParams.FirstName = firstName;

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateLastNameAsync(string lastName)
        {
            var peopleParams = GetUserParams();
            peopleParams.LastName = lastName;

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateOrganizationAsync(string orgId)
        {
            var peopleParams = GetUserParams();
            peopleParams.OrganizationId = orgId;

            return await TeamsApi.UpdateUserAsync(peopleParams);
        }

        /// <inheritdoc/>
        public IPersonUpdater UpdateUser() => new PersonUpdater(this);
    }
}
