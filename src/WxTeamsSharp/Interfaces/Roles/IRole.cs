using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.Roles
{
    /// <summary>
    /// A persona for an authenticated user, corresponding to a set of privileges within an organization. 
    /// This roles resource can be accessed only by an admin.
    /// </summary>
    public interface IRole : IListable
    {
        /// <summary>
        /// A unique identifier for the role.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The name of the role.
        /// </summary>
        string Name { get; }
    }
}
