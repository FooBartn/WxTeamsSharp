using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.Rooms;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Rooms
{
    /// <inheritdoc/>
    public class MeetingDetails : TeamsObject, IMeetingDetails
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string RoomId { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string MeetingLink { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string SipAddress { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string MeetingNumber { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string CallInTollFreeNumber { get; private set; } = string.Empty;

        /// <inheritdoc/>
        [JsonProperty]
        public string CallInTollNumber { get; private set; } = string.Empty;
    }
}
