using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.People;

namespace WxTeamsSharp.Models.People
{
    /// <summary>
    /// Builder class for updating an existing user
    /// </summary>
    public class PersonUpdater : IPersonUpdater
    {
        private string _firstName;
        private string _lastName;
        private string _displayName;
        private string _orgId;
        private string _avatarUrl;
        private List<string> _roles;
        private List<string> _licenses;
        private List<string> _email;
        private PeopleParams _parameters;
        private readonly Person _person;

        /// <summary>
        /// Create a new PersonUpdater with specific IPerson.
        /// Prefer to use an IPerson.Update() or
        /// PersonUpdater.New(IPerson person)
        /// </summary>
        /// <param name="person">Person object to udpate</param>
        public PersonUpdater(Person person)
        {
            _person = person;
        }

        /// <inheritdoc/>
        public static IPersonUpdater StartUpdate(Person person) => new PersonUpdater(person);

        /// <inheritdoc/>
        public IPersonUpdater UpdateEmail(string email)
        {
            _email = new List<string>
            {
                email
            };

            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater UpdateAvatar(string avatarUrl)
        {
            _avatarUrl = avatarUrl;
            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater UpdateOrgId(string orgId)
        {
            _orgId = orgId;
            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater AddRole(string roleId)
        {
            if (_roles == null)
                _roles = new List<string>();

            _roles.Add(roleId);

            if (_person.Roles != null)
            {
                foreach (var role in _person.Roles)
                {
                    _roles.Add(role);
                }
            }

            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater AddLicense(string licenseId)
        {
            if (_licenses == null)
                _licenses = new List<string>();

            _licenses.Add(licenseId);

            if (_person.Licenses != null)
            {
                foreach (var license in _person.Licenses)
                {
                    _licenses.Add(license);
                }
            }

            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater ReplaceRoles(List<string> roles)
        {
            _roles = roles;
            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater ReplaceLicenses(List<string> licenses)
        {
            _licenses = licenses;
            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater UpdateDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater UpdateFirstName(string firstName)
        {
            _firstName = firstName;
            return this;
        }

        /// <inheritdoc/>
        public IPersonUpdater UpdateLastName(string lastName)
        {
            _lastName = lastName;
            return this;
        }

        /// <inheritdoc/>
        public IUpdateablePerson Build()
        {
            _parameters = new PeopleParams(_person.Id)
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

            return _parameters;
        }

        /// <inheritdoc/>
        public async Task<Person> UpdateAsync()
            => await _person.TeamsApi.UpdateUserAsync(_parameters);
    }
}
