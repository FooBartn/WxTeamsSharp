using System;
using System.Net;
using System.Runtime.Serialization;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Exceptions
{
    /// <summary>
    /// Exception thrown from Webex Teams API
    /// </summary>
    [Serializable]
    public class TeamsApiException : Exception
    {
        /// <summary>
        /// API Exception Tracking Id
        /// </summary>
        public string TrackingId { get; }

        /// <summary>
        /// Raw message from API
        /// </summary>
        public string RawMessage { get; }

        /// <summary>
        /// Type of object that had an error
        /// </summary>
        public string ObjectType { get; }

        /// <summary>
        /// API endpoint that was used
        /// </summary>
        public string RequestUrl { get; }

        /// <summary>
        /// Error status code received
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <inheritdoc/>
        public TeamsApiException()
        {
        }

        /// <inheritdoc/>
        public TeamsApiException(string message) : base(message)
        {
        }

        /// <inheritdoc/>
        public TeamsApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc/>
        protected TeamsApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="message">ResponseMessage from API</param>
        internal TeamsApiException(ResponseMessage message) : base(message.Message)
        {
            TrackingId = message.TrackingId;
            RawMessage = message.RawMessage;
            HttpStatusCode = message.HttpStatusCode;
            RequestUrl = message.RequestUrl;
            ObjectType = message.ObjectType;
        }
    }
}
