using System.Net;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.General
{
    /// <inheritdoc/>
    internal class ResponseMessage : IResponseMessage
    {

        public string Message { get; set; }
        public string TrackingId { get; set; }
        public int RetryAfter { get; set; }
        public string RawMessage { get; set; }
        public string ObjectType { get; set; }
        public string RequestUrl { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
