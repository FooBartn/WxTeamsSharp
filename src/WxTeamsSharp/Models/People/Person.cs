using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Models.Messages;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.People
{
    /// <inheritdoc/>
    internal class Person : IPerson
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public List<string> Emails { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(ConcreteListConverter<PhoneNumber, IPhoneNumber>))]
        public List<IPhoneNumber> PhoneNumbers { get; set; }

        /// <inheritdoc/>
        public string DisplayName { get; set; }

        /// <inheritdoc/>
        public string NickName { get; set; }

        /// <inheritdoc/>
        public string FirstName { get; set; }

        /// <inheritdoc/>
        public string LastName { get; set; }

        /// <inheritdoc/>
        public string Avatar { get; set; }

        /// <inheritdoc/>
        public string OrgId { get; set; }

        /// <inheritdoc/>
        public List<string> Roles { get; set; }

        /// <inheritdoc/>
        public List<string> Licenses { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset LastModified { get; set; }

        /// <inheritdoc/>
        public string Timezone { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset LastActivity { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public PersonStatus Status { get; set; }

        /// <inheritdoc/>
        public bool InvitePending { get; set; }

        /// <inheritdoc/>
        public bool LoginEnabled { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public PersonType Type { get; set; }

        private PeopleParams GetUserParams()
        {
            var peopleParams = new PeopleParams
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

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> AddRoleAsync(string roleId)
        {
            var peopleParams = GetUserParams();

            if (peopleParams.Roles == null)
                peopleParams.Roles = new List<string>();

            peopleParams.Roles.Add(roleId);

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync() =>
            await WxTeamsApi.DeleteUserAsync(Id);

        /// <inheritdoc/>
        public async Task<IPerson> ReplaceLicensesAsync(List<string> licenseIds)
        {
            var peopleParams = GetUserParams();
            peopleParams.Licenses = licenseIds;

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> ReplaceRolesAsync(List<string> roleIds)
        {
            var peopleParams = GetUserParams();
            peopleParams.Roles = roleIds;

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IMessage> SendMessageAsync(string markdown)
            => await MessageBuilder.New()
                .SendToUserId(Id)
                .WithMarkdown(markdown)
                .Build()
                .SendAsync();

        /// <inheritdoc/>
        public async Task<IMessage> SendPlainMessageAsync(string text)
            => await MessageBuilder.New()
                .SendToUserId(Id)
                .WithText(text)
                .Build()
                .SendAsync();

        /// <inheritdoc/>
        public async Task<IPerson> UpdateAvatarAsync(string avatar)
        {
            var peopleParams = GetUserParams();
            peopleParams.Avatar = avatar;

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateDisplayNameAsync(string displayName)
        {
            var peopleParams = GetUserParams();
            peopleParams.DisplayName = displayName;

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateEmailAsync(string email)
        {
            var peopleParams = GetUserParams();
            peopleParams.Emails = new List<string> { email };

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateFirstNameAsync(string firstName)
        {
            var peopleParams = GetUserParams();
            peopleParams.FirstName = firstName;

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateLastNameAsync(string lastName)
        {
            var peopleParams = GetUserParams();
            peopleParams.LastName = lastName;

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public async Task<IPerson> UpdateOrganizationAsync(string orgId)
        {
            var peopleParams = GetUserParams();
            peopleParams.OrganizationId = orgId;

            return await WxTeamsApi.UpdateUserAsync(Id, peopleParams);
        }

        /// <inheritdoc/>
        public IPersonUpdater UpdateUser() => new PersonUpdater(this);
    }
}
