using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using WxTeamsSharp.Interfaces.Client;

namespace WxTeamsSharp.Client
{
    /// <inheritdoc/>
    internal class PollyPolicy : IPolicyProvider
    {
        private readonly ILogger<PollyPolicy> _logger;

        public PollyPolicy(ILogger<PollyPolicy> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public IAsyncPolicy<HttpResponseMessage> RetryAfterPolicy =>
        Policy.Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => r.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: (retryCount, response, context) =>
                {
                    var waitDuration = response?.Result?.Headers?.RetryAfter?.Delta.Value.TotalSeconds ?? 0;
                    return TimeSpan.FromSeconds(waitDuration + 1);
                },
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                onRetryAsync: async (response, timespan, retryCount, context) =>

                {
                    _logger.LogInformation($"Retry: {retryCount}. Waiting for {timespan.TotalSeconds} seconds.");
                }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            );
    }
}
