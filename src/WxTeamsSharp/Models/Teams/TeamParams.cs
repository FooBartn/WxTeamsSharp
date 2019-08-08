using Newtonsoft.Json;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.Teams
{
    internal class TeamParams : IJsonParams
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, JsonSettings.IgnoreNull);
    }
}
