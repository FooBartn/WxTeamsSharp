using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Client;
using WxTeamsSharp.Models.Exceptions;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Client
{
    internal class BaseClient : IWxBaseClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<BaseClient> _logger;
        private readonly IPolicyProvider _policyProvider;
        private string _token;
        private string _url;

        public BaseClient(IHttpClientFactory clientFactory,
            IPolicyProvider policyProvider,
            ILogger<BaseClient> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _policyProvider = policyProvider ?? throw new ArgumentNullException(nameof(policyProvider));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public void SetAuth(string token, string url = WxTeamsConstants.ApiBaseUrl)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _url = url;
        }

        public async Task<HttpResponseMessage> GetHttpResponse<TEntity>(HttpMethod method, string endpoint,
            string json = null, MultipartFormDataContent formData = null)
        {
            ValidateTokenSet();

            var finalResult = await _policyProvider.RetryAfterPolicy
                .ExecuteAsync(async () =>
                {
                    using (var client = _clientFactory.CreateClient())
                    {
                        var request = BuildRequestMessage(method, endpoint, json, formData);
                        client.BaseAddress = new Uri(_url);
                        var result = await client.SendAsync(request);

                        return result;
                    }
                });


            if (!finalResult.IsSuccessStatusCode)
                await HandleResultAsync<TEntity>(finalResult);

            return finalResult;
        }

        private async Task HandleResultAsync<TEntity>(HttpResponseMessage result)
        {
            var message = await result.Content.ReadAsStringAsync();
            var responseMessage = ConvertToResponseMessage(message);
            responseMessage.ObjectType = nameof(TEntity);
            responseMessage.RequestUrl = result.RequestMessage.RequestUri.PathAndQuery;
            responseMessage.HttpStatusCode = result.StatusCode;

            var exception = new TeamsApiException(responseMessage);
            _logger.LogError(exception, $"ObjectType: {exception.ObjectType} -- RawMessage:{exception.RawMessage}");
            throw exception;
        }

        private HttpRequestMessage BuildRequestMessage(HttpMethod method, string endpoint,
            string json, MultipartFormDataContent formData)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (json != null)
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            else if (formData != null)
                request.Content = formData;

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            return request;
        }

        private void ValidateTokenSet()
        {
            if (string.IsNullOrEmpty(_token))
                throw new ArgumentException("Token has not been set");
        }

        private static ResponseMessage ConvertToResponseMessage(string message)
        {
            var errorResult = JsonConvert.DeserializeObject<ResponseMessage>(message);
            errorResult.RawMessage = message;
            return errorResult;
        }
    }
}
