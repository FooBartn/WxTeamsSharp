using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Messages;

namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// People are registered users of Webex Teams. Searching and viewing People requires an auth token with a scope of spark:people_read. 
    /// Viewing the list of all People in your Organization requires an administrator auth token with spark-admin:people_read scope. 
    /// Adding, updating, and removing People requires an administrator auth token with the spark-admin:people_write scope.
    /// </summary>
    public interface IPerson : IMessageable
    {
        /// <summary>
        /// A unique identifier for the person.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The email addresses of the person.
        /// </summary>
        List<string> Emails { get; }

        /// <summary>
        /// Phone numbers for the person.
        /// </summary>
        List<IPhoneNumber> PhoneNumbers { get; }

        /// <summary>
        /// The full name of the person.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The nickname of the person if configured. If no nickname is configured for the person, this field will not be present.
        /// </summary>
        string NickName { get; }

        /// <summary>
        /// The first name of the person.
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// The last name of the person.
        /// </summary>
        string LastName { get; }

        /// <summary>
        /// The URL to the person's avatar in PNG format.
        /// </summary>
        string Avatar { get; }

        /// <summary>
        /// The ID of the organization to which this person belongs.
        /// </summary>
        string OrgId { get; }

        /// <summary>
        /// An array of role strings representing the roles to which this person belongs.
        /// </summary>
        List<string> Roles { get; }

        /// <summary>
        /// An array of license strings allocated to this person.
        /// </summary>
        List<string> Licenses { get; }

        /// <summary>
        /// The date and time the person was created.
        /// </summary>
        DateTimeOffset Created { get; }

        /// <summary>
        /// The date and time the person was last changed.
        /// </summary>
        DateTimeOffset LastModified { get; }

        /// <summary>
        /// The time zone of the person if configured. If no timezone is configured on the account, this field will not be present
        /// </summary>
        string Timezone { get; }

        /// <summary>
        /// The date and time of the person's last activity within Webex Teams.
        /// </summary>
        DateTimeOffset LastActivity { get; }

        /// <summary>
        /// The current presence status of the person.
        /// 
        /// active - active within the last 10 minutes
        /// call - the user is in a call
        /// DoNotDisturb - the user has manually set their status to "Do Not Disturb"
        /// inactive - last activity occurred more than 10 minutes ago
        /// meeting - the user is in a meeting
        /// OutOfOffice - the user or a Hybrid Calendar service has indicated that they are "Out of Office"
        /// pending - the user has never logged in; a status cannot be determined
        /// presenting - the user is sharing content
        /// unknown - the user's status could not be determined
        /// </summary>
        PersonStatus Status { get; }

        /// <summary>
        /// Whether or not an invite is pending for the user to complete account activation.
        /// 
        /// true - the person has been invited to Webex Teams but has not created an account
        /// false - an invite is not pending for this person
        /// </summary>
        bool InvitePending { get; }

        /// <summary>
        /// Whether or not the user is allowed to use Webex Teams.
        /// 
        /// true - the person can log into Webex Teams
        /// false - the person cannot log into Webex Teams
        /// </summary>
        bool LoginEnabled { get; }

        /// <summary>
        /// The type of person account, such as person or bot.
        /// </summary>
        PersonType Type { get; }

        /// <summary>
        /// Update the email of this user
        /// </summary>
        /// <param name="email">new email address</param>
        /// <returns>This method returns the updated Person</returns>
        Task<IPerson> UpdateEmailAsync(string email);

        /// <summary>
        /// Update the display name of this user
        /// </summary>
        /// <param name="displayName">new display name</param>
        /// <returns>This method returns the updated Person</returns>
        Task<IPerson> UpdateDisplayNameAsync(string displayName);

        /// <summary>
        /// Update the first name of this user
        /// </summary>
        /// <param name="firstName">new first name</param>
        /// <returns>This method returns the updated Person</returns>
        Task<IPerson> UpdateFirstNameAsync(string firstName);

        /// <summary>
        /// Update the last name of this Person
        /// </summary>
        /// <param name="lastName">new last name</param>
        /// <returns>This method returns the updated Person</returns>
        Task<IPerson> UpdateLastNameAsync(string lastName);

        /// <summary>
        /// Update the avatar of this user
        /// </summary>
        /// <param name="avatar">new avatar url</param>
        /// <returns>This method returns the updated Person</returns>
        Task<IPerson> UpdateAvatarAsync(string avatar);

        /// <summary>
        /// Update the organization of this user
        /// </summary>
        /// <param name="orgId">new organization id</param>
        /// <returns>This method returns the updated Person</returns>
        Task<IPerson> UpdateOrganizationAsync(string orgId);

        /// <summary>
        /// Add a role to this user (keeps existing roles).
        /// </summary>
        /// <param name="roleId">role id to add to user</param>
        /// <returns></returns>
        Task<IPerson> AddRoleAsync(string roleId);

        /// <summary>
        /// Add a license to this user (keeps existing licenses).
        /// </summary>
        /// <param name="licenseId">license id to add to user</param>
        /// <returns></returns>
        Task<IPerson> AddLicenseAsync(string licenseId);

        /// <summary>
        /// Replace (overwrite) all roles for this user
        /// </summary>
        /// <param name="roleIds">list of role ids to give the user</param>
        /// <returns></returns>
        Task<IPerson> ReplaceRolesAsync(List<string> roleIds);

        /// <summary>
        /// Replace (overwrite) all licenses for this user
        /// </summary>
        /// <param name="licenseIds">list of license ids to give the user</param>
        /// <returns></returns>
        Task<IPerson> ReplaceLicensesAsync(List<string> licenseIds);

        /// <summary>
        /// Start the PersonUpdater for this user.
        /// </summary>
        /// <returns>This method returns a PersonUpdater object for this user</returns>
        IPersonUpdater UpdateUser();

        /// <summary>
        /// Delete this user.
        /// </summary>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteAsync();
    }
}
