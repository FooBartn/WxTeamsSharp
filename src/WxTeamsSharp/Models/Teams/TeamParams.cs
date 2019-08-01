using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Models.Teams
{
    internal class TeamParams : IJsonParams
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, JsonUtilities.IgnoreNull);
    }
}
