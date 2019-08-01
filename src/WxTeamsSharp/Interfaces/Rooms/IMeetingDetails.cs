namespace WxTeamsSharp.Interfaces.Rooms
{
    /// <summary>
    /// Shows Webex meeting details for a room such as the SIP address, meeting URL, toll-free and toll dial-in numbers.
    /// </summary>
    public interface IMeetingDetails
    {
        /// <summary>
        /// A unique identifier for the room.
        /// </summary>
        string RoomId { get; }

        /// <summary>
        /// The Webex meeting URL for the room.
        /// </summary>
        string MeetingLink { get; }

        /// <summary>
        /// The SIP address for the room.
        /// </summary>
        string SipAddress { get; }

        /// <summary>
        /// The Webex meeting number for the room.
        /// </summary>
        string MeetingNumber { get; }

        /// <summary>
        /// The toll-free PSTN number for the room.
        /// </summary>
        string CallInTollFreeNumber { get; }

        /// <summary>
        /// The toll (local) PSTN number for the room.
        /// </summary>
        string CallInTollNumber { get; }
    }
}
