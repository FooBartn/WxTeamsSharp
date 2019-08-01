using System;
using WxTeamsSharp.Interfaces.Organizations;

namespace WxTeamsSharp.Models.Organizations
{
    /// <inheritdoc/>
    internal class Organization : IOrganization
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string DisplayName { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }
    }
}
