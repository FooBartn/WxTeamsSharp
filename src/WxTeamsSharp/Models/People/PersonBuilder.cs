using System;
using System.Collections.Generic;
using System.Linq;
using WxTeamsSharp.Interfaces.People;

namespace WxTeamsSharp.Models.People
{
    /// <summary>
    /// Builder class for new people. Start with PersonBuilder.New()
    /// </summary>
    public class PersonBuilder : IOtherProperties, ISetEmail
    {
        private string _firstName;
        private string _lastName;
        private string _displayName;
        private string _orgId;
        private string _avatarUrl;
        private List<string> _roles;
        private List<string> _licenses;
        private List<string> _email;

        /// <inheritdoc/>
        public static ISetEmail New() => new PersonBuilder();

        /// <inheritdoc/>
        public ISetIdentity WithEmail(string email)
        {
            _email = new List<string>
            {
                email
            };

            return this;
        }

        /// <inheritdoc/>
        public IOtherProperties WithAvatar(string avatarUrl)
        {
            _avatarUrl = avatarUrl;
            return this;
        }

        /// <inheritdoc/>
        public IOtherProperties WithOrgId(string orgId)
        {
            _orgId = orgId;
            return this;
        }

        /// <inheritdoc/>
        public IOtherProperties AddRole(string roleId)
        {
            if (_roles == null)
                _roles = new List<string>();

            _roles.Add(roleId);
            return this;
        }

        /// <inheritdoc/>
        public IOtherProperties AddLicense(string licenseId)
        {
            if (_licenses == null)
                _licenses = new List<string>();

            _licenses.Add(licenseId);
            return this;
        }

        /// <inheritdoc/>
        public IOtherProperties WithDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        /// <inheritdoc/>
        public IOtherProperties WithFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        /// <inheritdoc/>
        public IOtherProperties WithLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        /// <inheritdoc/>
        public ICreateablePerson Build()
        {
            if (_email == null || !_email.Any())
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrEmpty(_displayName) && string.IsNullOrEmpty(_firstName) && string.IsNullOrEmpty(_lastName))
                throw new ArgumentException("At least one of DisplayName, FirstName, or LastName are required");

            var personParams = new PeopleParams
            {
                Avatar = _avatarUrl,
                DisplayName = _displayName,
                FirstName = _firstName,
                LastName = _lastName,
                Emails = _email,
                Licenses = _licenses,
                OrganizationId = _orgId,
                Roles = _roles
            };

            return personParams;
        }
    }
}
