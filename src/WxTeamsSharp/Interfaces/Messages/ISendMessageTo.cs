namespace WxTeamsSharp.Interfaces.Messages
{
    /// <summary>
    /// Can set a recipent for new Message. Can be a user Id, email, or room
    /// </summary>
    public interface ISendMessageTo
    {
        /// <summary>
        /// Send a message to room
        /// </summary>
        /// <param name="roomId">Room Id to send message to</param>
        /// <returns></returns>
        ISendMessageContent SendToRoom(string roomId);

        /// <summary>
        /// Send message to User Id
        /// </summary>
        /// <param name="toUserId">User Id to send message to</param>
        /// <returns></returns>
        ISendMessageContent SendToUserId(string toUserId);

        /// <summary>
        /// Send message to user email
        /// </summary>
        /// <param name="toUserEmail">User email to send message to</param>
        /// <returns></returns>
        ISendMessageContent SendToUserEmail(string toUserEmail);
    }
}
