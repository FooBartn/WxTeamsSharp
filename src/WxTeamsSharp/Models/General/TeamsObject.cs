using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.General
{
    /// <inheritdoc/>
    public abstract class TeamsObject : ITeamsObject
    {
        /// <inheritdoc/>
        public IWxTeamsApi TeamsApi { get; set; }
    }
}
