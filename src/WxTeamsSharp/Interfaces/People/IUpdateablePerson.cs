using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.People
{
    /// <summary>
    /// Updateable person object
    /// </summary>
    public interface IUpdateablePerson : IJsonParams
    {
        /// <summary>
        /// Update person with set properties
        /// </summary>
        /// <returns></returns>
        Task<IPerson> UpdateAsync();
    }
}
