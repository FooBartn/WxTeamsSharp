using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.Messages
{
    /// <summary>
    /// Message that can be sent
    /// </summary>
    public interface ISendableMessage : IJsonParams, IFormDataParams
    {
        /// <summary>
        /// Whether the message includes a local file or not.
        /// Determines whether or not it gets exported to JSON or MultipartFormData
        /// </summary>
        /// <returns>boolean</returns>
        bool HasLocalFile();
    }
}
