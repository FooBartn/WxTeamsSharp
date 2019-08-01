namespace WxTeamsSharp.Enums
{
    /// <summary>
    /// The current presence status of the person.
    /// </summary>
    public enum PersonStatus
    {
        /// <summary>
        /// active within the last 10 minutes
        /// </summary>
        Active,

        /// <summary>
        /// the user is in a call
        /// </summary>
        Call,

        /// <summary>
        /// the user has manually set their status to "Do Not Disturb"
        /// </summary>
        DoNotDisturb,

        /// <summary>
        /// last activity occurred more than 10 minutes ago
        /// </summary>
        Inactive,

        /// <summary>
        /// the user is in a meeting
        /// </summary>
        Meeting,

        /// <summary>
        /// the user or a Hybrid Calendar service has indicated that they are "Out of Office"
        /// </summary>
        OutOfOffice,

        /// <summary>
        /// the user has never logged in; a status cannot be determined
        /// </summary>
        Pending,

        /// <summary>
        /// the user is sharing content
        /// </summary>
        Presenting,

        /// <summary>
        /// the user’s status could not be determined
        /// </summary>
        Unknown
    }
}
