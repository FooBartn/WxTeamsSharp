using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.General
{
    internal class ErrorTitle : IErrorTitle
    {
        public string Code { get; set; }

        public string Reason { get; set; }
    }
}
