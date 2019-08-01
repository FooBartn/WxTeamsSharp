namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// Set the identity for the new user. At least one of these has to be set.
    /// </summary>
    public interface ISetIdentity
    {
        /// <summary>
        /// Set the users display name
        /// </summary>
        /// <param name="displayName">display name</param>
        /// <returns></returns>
        IOtherProperties WithDisplayName(string displayName);

        /// <summary>
        /// Set the users first name
        /// </summary>
        /// <param name="firstName">first name</param>
        /// <returns></returns>
        IOtherProperties WithFirstName(string firstName);

        /// <summary>
        /// Set the users last name
        /// </summary>
        /// <param name="lastName">last name</param>
        /// <returns></returns>
        IOtherProperties WithLastName(string lastName);
    }
}
