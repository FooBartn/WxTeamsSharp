using System.Threading.Tasks;

namespace WxTeamsSharp.Interfaces.Messages
{
    /// <summary>
    /// Supports sending messages via Webex Teams API
    /// </summary>
    public interface IMessageable
    {
        /// <summary>
        /// Send a plain text message
        /// </summary>
        /// <param name="text">text to send</param>
        /// <returns></returns>
        Task<IMessage> SendPlainMessageAsync(string text);

        /// <summary>
        /// Send a markdown message
        /// </summary>
        /// <param name="markdown">markdown to send</param>
        /// <returns></returns>
        Task<IMessage> SendMessageAsync(string markdown);
    }
}
