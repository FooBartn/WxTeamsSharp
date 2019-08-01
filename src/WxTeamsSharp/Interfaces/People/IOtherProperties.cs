namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// Set other optional properties of new person
    /// </summary>
    public interface IOtherProperties : ISetIdentity, IBuildablePerson
    {
        /// <summary>
        /// Set Avatar URL
        /// </summary>
        /// <param name="avatarUrl">publically accessible url of avatar image</param>
        /// <returns></returns>
        IOtherProperties WithAvatar(string avatarUrl);

        /// <summary>
        /// Set Organization ID
        /// </summary>
        /// <param name="orgId">Organization Id</param>
        /// <returns></returns>
        IOtherProperties WithOrgId(string orgId);

        /// <summary>
        /// Add a role to the user (keep existing)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IOtherProperties AddRole(string roleId);

        /// <summary>
        /// Add a license to the user (keep existing)
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns></returns>
        IOtherProperties AddLicense(string licenseId);
    }
}
