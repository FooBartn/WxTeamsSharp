using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Client
{
    internal class TeamsClient : IWxTeamsClient
    {
        private readonly IWxBaseClient _client;
        private readonly IJsonDeserializer _serializer;

        public TeamsClient(IJsonDeserializer serializer, IWxBaseClient client)
        {
            _client = client;
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public void SetAuth(string token, string url = WxTeamsConstants.ApiBaseUrl)
            => _client.SetAuth(token, url);

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
            using (var result = await _client.GetHttpResponse<TEntity>(HttpMethod.Get, endpoint))
                return await DeserializeItemResultsAsync<TEntity>(result);
        }

        public async Task<TEntity> GetResultAsync<TEntity>(string endpoint)
            where TEntity : TeamsObject
        {
            using (var result = await _client.GetHttpResponse<TEntity>(HttpMethod.Get, endpoint))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }

        public async Task<IResponseMessage> DeleteResultAsync<TEntity>(string endpoint)
        {
            using (await _client.GetHttpResponse<TEntity>(HttpMethod.Delete, endpoint))
                return new ResponseMessage { Message = "OK" };
        }

        public async Task<TEntity> PostMultiPartResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IFormDataParams
            where TEntity : TeamsObject
        {
            var formData = requestParams.ToFormData();
            using (var result = await _client.GetHttpResponse<TEntity>(HttpMethod.Post, endpoint, formData: formData))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }

        public async Task<TEntity> PostResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : TeamsObject
        {
            var json = requestParams.ToJson();
            using (var result = await _client.GetHttpResponse<TEntity>(HttpMethod.Post, endpoint, json))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }

        public async Task<TEntity> PutResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : TeamsObject
        {
            var json = requestParams.ToJson();
            using (var result = await _client.GetHttpResponse<TEntity>(HttpMethod.Put, endpoint, json))
                return await DeserializeTeamsObjectAsync<TEntity>(result);
        }
    }
}
