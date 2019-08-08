using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.ResourceGroups
{
    /// <inheritdoc/>
    public class ResourceGroupMembership : TeamsObject, IResourceGroupMembership
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string ResourceGroupId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string LicenseId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string PersonId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string PersonOrgId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResourceMembershipStatus Status { get; private set; }
    }
}
