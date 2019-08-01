using Polly;
using System;
using System.Net;
using System.Net.Http;

namespace WxTeamsSharp.Client
{
    internal static class Policies
    {
        internal static IAsyncPolicy<HttpResponseMessage> RetryAfterPolicy =>
        Policy.Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => r.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: (retryCount, response, context) =>
                {
                    var waitDuration = response?.Result?.Headers?.RetryAfter?.Delta.Value.TotalSeconds ?? 0;
                    return TimeSpan.FromSeconds(waitDuration + 1);
                },
                onRetryAsync: async (response, timespan, retryCount, context) =>
                {
                    /* perhaps some logging, eg the retry count, the timespan delaying for */
                }
            );
    }
}
