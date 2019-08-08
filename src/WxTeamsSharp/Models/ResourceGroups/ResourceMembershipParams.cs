using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.ResourceGroups
{
    internal class ResourceMembershipParams : IJsonParams
    {
        [JsonProperty(PropertyName = "resourceGroupId")]
        public string ResourceGroupId { get; set; }

        [JsonProperty(PropertyName = "licenseId")]
        public string LicenseId { get; set; }

        [JsonProperty(PropertyName = "personId")]
        public string PersonId { get; set; }

        [JsonProperty(PropertyName = "personOrgId")]
        public string PersonOrgId { get; set; }

        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResourceMembershipStatus Status { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, JsonSettings.Standard);
    }
}
