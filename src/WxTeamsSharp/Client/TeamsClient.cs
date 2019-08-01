using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Exceptions;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Client
{
    internal static class TeamsClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _token;

        internal static void SetAuth(string token, string url)
        {
            _token = token;

            if (_httpClient.BaseAddress == null)
                _httpClient.BaseAddress = new Uri(url);
        }

        private static async Task<string> HandleResultAsync<TEntity>(HttpResponseMessage result)
        {
            var message = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
                return message;

            var responseMessage = ConvertToResponseMessage(message);
            responseMessage.ObjectType = nameof(TEntity);
            responseMessage.RequestUrl = result.RequestMessage.RequestUri.PathAndQuery;
            responseMessage.HttpStatusCode = result.StatusCode;

            throw new TeamsApiException(responseMessage);
        }

        private static HttpRequestMessage BuildRequestMessage(HttpMethod method, string endpoint,
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

        private static async Task<HttpResponseMessage> GetHttpResponse(HttpMethod method, string endpoint,
            string json = null, MultipartFormDataContent formData = null)
        {
            ValidateTokenSet();

            var request = BuildRequestMessage(method, endpoint, json, formData);
            return await _httpClient.SendAsync(request);
        }

        private static void ValidateTokenSet()
        {
            if (string.IsNullOrEmpty(_token))
                throw new ArgumentException("Token has not been set");
        }

        internal static async Task<IListResult<TInterface>> GetResultsAsync<TEntity, TInterface>(string endpoint)
            where TEntity : TInterface
        {
            var result = await Policies.RetryAfterPolicy
                .ExecuteAsync(async () => await GetHttpResponse(HttpMethod.Get, endpoint));
            var content = await HandleResultAsync<TEntity>(result);
            var entity = JsonUtilities.FromJson<ItemsResult<TEntity>>(content);

            if (result.Headers.TryGetValues("Link", out IEnumerable<string> values)
                && values.FirstOrDefault() is string link
                && !string.IsNullOrEmpty(link)
                && link.Contains("rel=\"next\""))
            {
                link = link
                    .Replace("<https://api.ciscospark.com/v1", "")
                    .Replace(">; rel=\"next\"", "");

                if (Uri.IsWellFormedUriString(link, UriKind.Relative))
                    entity.NextPage = link;
            }

            return ConvertToIListResult<TInterface, TEntity>(entity);
        }

        internal static async Task<TEntity> GetResultAsync<TEntity>(string endpoint)
            where TEntity : class
        {
            var result = await Policies.RetryAfterPolicy
                .ExecuteAsync(async () => await GetHttpResponse(HttpMethod.Get, endpoint));

            var content = await HandleResultAsync<TEntity>(result);
            var entity = JsonUtilities.FromJson<TEntity>(content);

            return entity;
        }

        internal static async Task<IResponseMessage> DeleteResultAsync<TEntity>(string endpoint)
        {
            var result = await Policies.RetryAfterPolicy
                .ExecuteAsync(async () => await GetHttpResponse(HttpMethod.Delete, endpoint));
            await HandleResultAsync<TEntity>(result);

            return new ResponseMessage
            {
                Message = "OK"
            };
        }

        private static ResponseMessage ConvertToResponseMessage(string message)
        {
            var errorResult = JsonUtilities.FromJson<ResponseMessage>(message);
            errorResult.RawMessage = message;
            return errorResult;
        }

        internal static async Task<TEntity> PostMultiPartResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IFormDataParams
            where TEntity : class
        {
            var formData = requestParams.ToFormData();
            var result = await Policies.RetryAfterPolicy
                .ExecuteAsync(async () => await GetHttpResponse(HttpMethod.Post, endpoint, formData: formData));
            var content = await HandleResultAsync<TEntity>(result);
            var entity = JsonUtilities.FromJson<TEntity>(content);

            return entity;
        }

        internal static async Task<TEntity> PostResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : class
        {
            var json = requestParams.ToJson();
            var result = await Policies.RetryAfterPolicy
                .ExecuteAsync(async () => await GetHttpResponse(HttpMethod.Post, endpoint, json));
            var content = await HandleResultAsync<TEntity>(result);
            var entity = JsonUtilities.FromJson<TEntity>(content);

            return entity;
        }

        internal static async Task<TEntity> PutResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : class
        {
            var json = requestParams.ToJson();
            var result = await Policies.RetryAfterPolicy
                .ExecuteAsync(async () => await GetHttpResponse(HttpMethod.Put, endpoint, json));
            var content = await HandleResultAsync<TEntity>(result);
            var entity = JsonUtilities.FromJson<TEntity>(content);

            return entity;
        }

        private static IListResult<TInterface> ConvertToIListResult<TInterface, TConcrete>(ItemsResult<TConcrete> result)
            where TConcrete : TInterface
        {
            var converted = Enumerable.Cast<TInterface>(result.Items).ToList();
            var listResult = new ItemsResult<TInterface>(converted, result.NextPage);

            return listResult;
        }
    }
}
