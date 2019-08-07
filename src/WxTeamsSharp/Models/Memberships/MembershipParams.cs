using Newtonsoft.Json;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.Memberships
{
    internal class MembershipParams : IJsonParams
    {
        [JsonProperty(PropertyName = "teamId")]
        public string TeamId { get; set; }
        [JsonProperty(PropertyName = "membershipId")]
        public string MembershipId { get; set; }
        [JsonProperty(PropertyName = "roomId")]
        public string RoomId { get; set; }
        [JsonProperty(PropertyName = "personId")]
        public string PersonId { get; set; }
        [JsonProperty(PropertyName = "personEmail")]
        public string PersonEmail { get; set; }
        [JsonProperty(PropertyName = "isModerator")]
        public bool IsModerator { get; set; }
        public string ToJson() => JsonConvert.SerializeObject(this, JsonSettings.IgnoreNull);
    }
}
