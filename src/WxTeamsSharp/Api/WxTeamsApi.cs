using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Client;

namespace WxTeamsSharp.Api
{
    /// <summary>
    /// Webex Teams API
    /// </summary>
    public static partial class WxTeamsApi
    {
        /// <summary>
        /// Sets the access token (and optionally the url of the API). 
        /// Required before using the API.
        /// </summary>
        /// <param name="accessToken">The token used to access the API</param>
        /// <param name="url">The url of the API. Don't change this unless you have a really good reason.</param>
        public static void SetAuth(string accessToken, string url = "https://api.ciscospark.com/v1/")
            => TeamsClient.SetAuth(accessToken, url);

        private static async Task<string> GetPathWithQueryAsync(string path, List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Any())
            {
                var query = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();
                return $"{path}?{query}";
            }

            return path;
        }
    }
}
