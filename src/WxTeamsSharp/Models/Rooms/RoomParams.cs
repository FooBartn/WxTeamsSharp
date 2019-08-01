using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Models.Rooms
{
    internal class RoomParams : IJsonParams
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, JsonUtilities.IgnoreNull);
    }
}
