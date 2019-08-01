using System;

namespace WxTeamsSharp.Interfaces.Organizations
{
    /// <summary>
    /// A set of people in Webex Teams. Organizations may manage other organizations or be managed themselves. 
    /// This organizations resource can be accessed only by an admin.
    /// </summary>
    public interface IOrganization
    {
        /// <summary>
        /// A unique identifier for the organization.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Full name of the organization.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The date and time the organization was created.
        /// </summary>
        DateTimeOffset Created { get; }
    }
}
