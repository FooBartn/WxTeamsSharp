using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Interfaces.ResourceGroups
{
    /// <summary>
    /// Resource Groups are collections of on-premise clusters which provide Hybrid Services to a particular subset of people in an organization. 
    /// If a person has a Hybrid Services license associated with their account, they will be associated with a resource group to use specific 
    /// on-premise clusters for that service.
    ///
    /// Searching and viewing Resource Groups requires an administrator auth token with a scope of spark-admin:resource_groups_read.
    /// </summary>
    public interface IResourceGroup : IListable
    {
        /// <summary>
        /// A unique identifier for the resource group.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// A user-friendly name for the resource group.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The ID of the organization to which this resource group belongs.
        /// </summary>
        string OrgId { get; }
    }
}
