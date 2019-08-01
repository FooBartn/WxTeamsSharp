using WxTeamsSharp.Interfaces.Rooms;

namespace WxTeamsSharp.Models.Rooms
{
    /// <inheritdoc/>
    internal class MeetingDetails : IMeetingDetails
    {
        /// <inheritdoc/>
        public string RoomId { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string MeetingLink { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string SipAddress { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string MeetingNumber { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string CallInTollFreeNumber { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string CallInTollNumber { get; set; } = string.Empty;
    }
}
