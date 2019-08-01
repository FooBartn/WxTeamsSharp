using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// PersonBuilder Interface
    /// </summary>
    public interface ICreateablePerson : IJsonParams
    {
        /// <summary>
        /// Will create a person. Requires Admin privileges.
        /// </summary>
        /// <returns>IPerson object</returns>
        Task<IPerson> CreateAsync();
    }
}
