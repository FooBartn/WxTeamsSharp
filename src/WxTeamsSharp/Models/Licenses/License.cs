using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.Licenses;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Licenses
{
    /// <inheritdoc/>
    public class License : TeamsObject, ILicense
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Name { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public int TotalUnits { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public int ConsumedUnits { get; private set; }
    }
}
