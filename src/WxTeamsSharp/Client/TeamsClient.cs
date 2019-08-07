using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Exceptions;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Client
{
    internal class TeamsClient : IWxTeamsClient
    {
        private readonly ILogger<TeamsClient> _logger;
        private readonly IPolicyProvider _policyProvider;
        private readonly IJsonDeserializer _serializer;
        private readonly IHttpClientFactory _clientFactory;
        private string _token;

        public TeamsClient(IHttpClientFactory clientFactory,
            IJsonDeserializer serializer,
            IPolicyProvider policyProvider,
            ILogger<TeamsClient> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _policyProvider = policyProvider ?? throw new ArgumentNullException(nameof(policyProvider));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public void SetAuth(string token, string url = WxTeamsConstants.ApiBaseUrl)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
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

        private async Task<HttpResponseMessage> GetHttpResponse<TEntity>(HttpMethod method, string endpoint,
            string json = null, MultipartFormDataContent formData = null)
        {
            ValidateTokenSet();

            var finalResult = await _policyProvider.RetryAfterPolicy
                .ExecuteAsync(async () =>
                {
                    using (var client = _clientFactory.CreateClient())
                    {
                        var request = BuildRequestMessage(method, endpoint, json, formData);
                        client.BaseAddress = new Uri(WxTeamsConstants.ApiBaseUrl);
                        var result = await client.SendAsync(request);

                        return result;
                    }
                });


            if (!finalResult.IsSuccessStatusCode)
                await HandleResultAsync<TEntity>(finalResult);

            return finalResult;
        }

        private void ValidateTokenSet()
        {
            if (string.IsNullOrEmpty(_token))
                throw new ArgumentException("Token has not been set");
        }

        private async Task<TEntity> DeserializeTeamsObjectAsync<TEntity>(HttpResponseMessage result)
            where TEntity : TeamsObject
        {
            using (var streamReader = new StreamReader(await result.Content.ReadAsStreamAsync()))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return _serializer.Deserialize<TEntity>(jsonTextReader);
                }
            }
        }

        private async Task<IListResult<TEntity>> DeserializeItemResultsAsync<TEntity>(HttpResponseMessage result)
            where TEntity : TeamsObject
        {
            using (var streamReader = new StreamReader(await result.Content.ReadAsStreamAsync()))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var itemsResult = _serializer.DeserializeList<TEntity>(jsonTextReader);
                    SetNextPage(result, itemsResult);
                    return itemsResult;
                }
            }
        }

        private void SetNextPage<TEntity>(HttpResponseMessage result, IListResult<TEntity> listResult)
            where TEntity : TeamsObject
        {
            if (listResult is ItemsResult<TEntity> itemsResult
                && result.Headers.TryGetValues("Link", out IEnumerable<string> values)
                && values.FirstOrDefault() is string link
                && !string.IsNullOrEmpty(link)
                && link.Contains("rel=\"next\""))
            {
                link = link
                    .Replace("<https://api.ciscospark.com/v1", "")
                    .Replace(">; rel=\"next\"", "");

                if (Uri.IsWellFormedUriString(link, UriKind.Relative))
                    itemsResult.SetNextPage(link);
            }
        }

        public async Task<IListResult<TEntity>> GetResultsAsync<TEntity>(string endpoint)
            where TEntity : TeamsObject
        {
            using (var result = await GetHttpResponse<TEntity>(HttpMethod.Get, endpoint))
                return await DeserializeItemResultsAsync<TEntity>(result);
        }

        public async Task<TEntity> GetResultAsync<TEntity>(string endpoint)
            where TEntity : TeamsObject
        {
            using (var result = await GetHttpResponse<TEntity>(HttpMethod.Get, endpoint))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }

        public async Task<IResponseMessage> DeleteResultAsync<TEntity>(string endpoint)
        {
            using (await GetHttpResponse<TEntity>(HttpMethod.Delete, endpoint))
                return new ResponseMessage { Message = "OK" };
        }

        private static ResponseMessage ConvertToResponseMessage(string message)
        {
            var errorResult = JsonConvert.DeserializeObject<ResponseMessage>(message);
            errorResult.RawMessage = message;
            return errorResult;
        }

        public async Task<TEntity> PostMultiPartResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IFormDataParams
            where TEntity : TeamsObject
        {
            var formData = requestParams.ToFormData();
            using (var result = await GetHttpResponse<TEntity>(HttpMethod.Post, endpoint, formData: formData))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }

        public async Task<TEntity> PostResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : TeamsObject
        {
            var json = requestParams.ToJson();
            using (var result = await GetHttpResponse<TEntity>(HttpMethod.Post, endpoint, json))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }

        public async Task<TEntity> PutResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : TeamsObject
        {
            var json = requestParams.ToJson();
            using (var result = await GetHttpResponse<TEntity>(HttpMethod.Put, endpoint, json))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }
    }
}
