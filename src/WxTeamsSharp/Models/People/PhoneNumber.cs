using WxTeamsSharp.Interfaces.People;

namespace WxTeamsSharp.Models.People
{
    internal class PhoneNumber : IPhoneNumber
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
}
