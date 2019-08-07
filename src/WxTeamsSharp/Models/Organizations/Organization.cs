using Newtonsoft.Json;
using System;
using WxTeamsSharp.Interfaces.Organizations;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Organizations
{
    /// <inheritdoc/>
    public class Organization : TeamsObject, IOrganization
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string DisplayName { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset Created { get; private set; }
    }
}
