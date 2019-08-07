using Polly;
using System.Net.Http;

namespace WxTeamsSharp.Interfaces.Client
{
    /// <summary>
    /// Provides Retry Policy 
    /// </summary>
    public interface IPolicyProvider
    {
        /// <summary>
        /// Async Policy for Retry-After Header from API. Used in TeamsClient
        /// </summary>
        IAsyncPolicy<HttpResponseMessage> RetryAfterPolicy { get; }
    }
}
