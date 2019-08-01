namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// Set email of new user
    /// </summary>
    public interface ISetEmail
    {
        /// <summary>
        /// Set email of new user
        /// </summary>
        /// <param name="email">email address</param>
        /// <returns></returns>
        ISetIdentity WithEmail(string email);
    }
}
