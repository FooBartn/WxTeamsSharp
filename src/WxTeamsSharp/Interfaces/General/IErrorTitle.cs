namespace WxTeamsSharp.Interfaces.General
{
    /// <summary>
    /// Resources which encounter errors during retrieval will include an errors object. 
    /// This errors object will contain a specific error code and reason describing why the 
    /// individual resource could not be returned in the request. Read more: https://developer.webex.com/docs/api/basics
    /// </summary>
    public interface IErrorTitle
    {
        /// <summary>
        /// Error code
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Reason describing why the individual resource could not be returned in the request
        /// </summary>
        string Reason { get; }
    }
}
