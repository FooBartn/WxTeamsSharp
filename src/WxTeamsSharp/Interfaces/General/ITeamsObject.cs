using WxTeamsSharp.Interfaces.Api;

namespace WxTeamsSharp.Interfaces.General
{
    /// <summary>
    /// Base Teams Object. Currently used to supply objects
    /// with reference to the API so we can make calls directly
    /// from the object
    /// </summary>
    public interface ITeamsObject
    {
        /// <summary>
        /// Reference to the IWxTeamsApi. Is set by CustomJsonSerializer
        /// </summary>
        IWxTeamsApi TeamsApi { get; }
    }
}
