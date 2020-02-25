using WxTeamsSharp.Interfaces.People;

namespace WxTeamsSharp.Models.People
{
    /// <inheritdoc/>
    public class PhoneNumber : IPhoneNumber
    {
        /// <inheritdoc/>
        public string Type { get; set; }

        /// <inheritdoc/>
        public string Value { get; set; }
    }
}
