namespace WxTeamsSharp.Interfaces.Messages
{
    /// <summary>
    /// Can add content to new Message
    /// </summary>
    public interface ISendMessageContent
    {
        /// <summary>
        /// Add plain text content to new message
        /// </summary>
        /// <param name="text">plain text to add</param>
        /// <returns></returns>
        ISendMessageWithFile WithText(string text);

        /// <summary>
        /// Add markdown content to new message
        /// </summary>
        /// <param name="markdown">markdown content to add</param>
        /// <returns></returns>
        ISendMessageWithFile WithMarkdown(string markdown);
    }
}
