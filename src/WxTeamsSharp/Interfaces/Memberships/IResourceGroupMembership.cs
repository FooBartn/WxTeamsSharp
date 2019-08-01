using WxTeamsSharp.Enums;

namespace WxTeamsSharp.Interfaces.Memberships
{
    /// <summary>
    /// Resource Group Memberships represent a person's relationship to a Resource Group for a particular 
    /// Hybrid Services license. Users assigned a new license will be automatically placed in a "default" Resource Group. 
    /// Use this API to list memberships for all people in an organization or update memberships to use a different Resource Group.
    /// 
    /// Searching and viewing Resource Group Memberships requires an administrator auth token with 
    /// the spark-admin:resource_group_memberships_read scope.Updating memberships requires an administrator auth token 
    /// with the spark-admin:resource_group_memberships_write scope.
    /// </summary>
    public interface IResourceGroupMembership
    {
        /// <summary>
        /// A unique identifier for the resource group membership.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// The resource group ID.
        /// </summary>
        string ResourceGroupId { get; }

        /// <summary>
        /// The license ID.
        /// </summary>
        string LicenseId { get; }

        /// <summary>
        /// The person ID.
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// The organization ID of the person.
        /// </summary>
        string PersonOrgId { get; }

        /// <summary>
        /// The activation status of the resource group membership.
        /// 
        /// pending - activation pending
        /// activated - activated
        /// error - error present
        /// </summary>
        ResourceMembershipStatus Status { get; }
    }
}
