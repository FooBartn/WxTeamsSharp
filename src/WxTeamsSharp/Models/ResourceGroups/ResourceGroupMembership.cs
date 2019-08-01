using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Memberships;

namespace WxTeamsSharp.Models.ResourceGroups
{
    /// <inheritdoc/>
    internal class ResourceGroupMembership : IResourceGroupMembership
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string ResourceGroupId { get; set; }

        /// <inheritdoc/>
        public string LicenseId { get; set; }

        /// <inheritdoc/>
        public string PersonId { get; set; }

        /// <inheritdoc/>
        public string PersonOrgId { get; set; }

        /// <inheritdoc/>
        [JsonConverter(typeof(StringEnumConverter))]
        public ResourceMembershipStatus Status { get; set; }
    }
}
