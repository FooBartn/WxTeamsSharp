using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// Updateable person object
    /// </summary>
    public interface IUpdateablePerson : IJsonParams
    {
        /// <summary>
        /// Person ID required to update
        /// </summary>
        string Id { get; }
    }
}
