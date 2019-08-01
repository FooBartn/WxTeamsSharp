namespace WxTeamsSharp.Interfaces.General
{
    /// <summary>
    /// Response generated for the user
    /// </summary>
    public interface IResponseMessage
    {
        /// <summary>
        /// Message response from APIs which do not return object results
        /// </summary>
        string Message { get; }
    }
}
