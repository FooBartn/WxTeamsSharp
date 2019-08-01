namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// User phone number
    /// </summary>
    public interface IPhoneNumber
    {
        /// <summary>
        /// Phone type (work, etc).
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Phone number
        /// </summary>
        string Value { get; }
    }
}
