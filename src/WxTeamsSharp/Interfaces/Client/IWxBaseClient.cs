using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;

namespace WxTeamsSharp.Interfaces.Client
{
    /// <summary>
    /// Layer that communicates directly with API
    /// </summary>
    public interface IWxBaseClient
    {
        /// <summary>
        /// Sets the access token. 
        /// Required before using the API.
        /// </summary>
        /// <param name="token">The token used to access the API</param>
        /// <param name="url">Base URL for API. Has default. Don't change unless you're sure what you're doing</param>
        void SetAuth(string token, string url = WxTeamsConstants.ApiBaseUrl);

        /// <summary>
        /// Method responsible for direct communication with API and returning 
        /// a result or a TeamsApiException
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="method"></param>
        /// <param name="endpoint"></param>
        /// <param name="json"></param>
        /// <param name="formData"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> GetHttpResponse<TEntity>(HttpMethod method, string endpoint,
            string json = null, MultipartFormDataContent formData = null);
    }
}
