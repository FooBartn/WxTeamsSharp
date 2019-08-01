namespace WxTeamsSharp.Interfaces.Messages
{
    /// <summary>
    /// Can add a single file to the message
    /// </summary>
    public interface ISendMessageWithFile : IBuildableMessage
    {
        /// <summary>
        /// Add a local file to the message
        /// </summary>
        /// <param name="file">location of local file</param>
        /// <returns></returns>
        IBuildableMessage WithLocalFile(string file);

        /// <summary>
        /// Add a remote publically accessible file to message
        /// </summary>
        /// <param name="fileUrl">Url of file to add</param>
        /// <returns></returns>
        IBuildableMessage WithPublicFileUrl(string fileUrl);
    }
}
