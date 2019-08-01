using System.Collections.Generic;

namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// Fluent class for updating an existing user
    /// </summary>
    public interface IPersonUpdater
    {
        /// <summary>
        /// Update email
        /// </summary>
        /// <param name="email">New email address</param>
        /// <returns></returns>
        IPersonUpdater UpdateEmail(string email);

        /// <summary>
        /// Update avatar URL
        /// </summary>
        /// <param name="avatarUrl">New avatar url address</param>
        /// <returns></returns>
        IPersonUpdater UpdateAvatar(string avatarUrl);

        /// <summary>
        /// Update organization
        /// </summary>
        /// <param name="orgId">Organization Id</param>
        /// <returns></returns>
        IPersonUpdater UpdateOrgId(string orgId);

        /// <summary>
        /// Add a role to the user (keep existing)
        /// </summary>
        /// <param name="roleId">Role Id to add</param>
        /// <returns></returns>
        IPersonUpdater AddRole(string roleId);

        /// <summary>
        /// Replace (overwrite) all roles with new ones
        /// </summary>
        /// <param name="roles">Role Ids to set for the user</param>
        /// <returns></returns>
        IPersonUpdater ReplaceRoles(List<string> roles);

        /// <summary>
        /// Add a license to the user (keep existing)
        /// </summary>
        /// <param name="licenseId">License Id to add</param>
        /// <returns></returns>
        IPersonUpdater AddLicense(string licenseId);

        /// <summary>
        /// Replace (overwrite) all licenses with new ones
        /// </summary>
        /// <param name="licenses">License Ids to set for the user</param>
        /// <returns></returns>
        IPersonUpdater ReplaceLicenses(List<string> licenses);

        /// <summary>
        /// Update display name
        /// </summary>
        /// <param name="displayName">New display name</param>
        /// <returns></returns>
        IPersonUpdater UpdateDisplayName(string displayName);

        /// <summary>
        /// Update first name
        /// </summary>
        /// <param name="firstName">New first name</param>
        /// <returns></returns>
        IPersonUpdater UpdateFirstName(string firstName);

        /// <summary>
        /// Update last name
        /// </summary>
        /// <param name="lastName">New last name</param>
        /// <returns></returns>
        IPersonUpdater UpdateLastName(string lastName);
    }
}
