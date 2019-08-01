namespace WxTeamsSharp.Interfaces.Licenses
{
    /// <summary>
    /// An allowance for features and services that are provided to users on a Webex Teams services subscription. 
    /// Cisco and its partners manage the amount of licenses provided to administrators and users. 
    /// This license resource can be accessed only by an admin.
    /// </summary>
    public interface ILicense
    {
        /// <summary>
        /// A unique identifier for the license.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name of the licensed feature.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Total number of license units allocated.
        /// </summary>
        int TotalUnits { get; }

        /// <summary>
        /// Total number of license units consumed.
        /// </summary>
        int ConsumedUnits { get; }
    }
}
