using WxTeamsSharp.Interfaces.Licenses;

namespace WxTeamsSharp.Models.Licenses
{
    /// <inheritdoc/>
    internal class License : ILicense
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public int TotalUnits { get; set; }

        /// <inheritdoc/>
        public int ConsumedUnits { get; set; }
    }
}
