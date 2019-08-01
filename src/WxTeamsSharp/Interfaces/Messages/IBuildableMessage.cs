namespace WxTeamsSharp.Interfaces.Messages
{
    /// <summary>
    /// MessageBuilder interface
    /// </summary>
    public interface IBuildableMessage
    {
        /// <summary>
        /// Will build into a sendable message. i.e. can run SendAsync()
        /// </summary>
        /// <returns></returns>
        ISendableMessage Build();
    }
}
