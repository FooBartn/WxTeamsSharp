using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.Client;

namespace WxTeamsSharp.Api
{

    internal partial class WxTeamsApi : IWxTeamsApi
    {
        private readonly ILogger<WxTeamsApi> _logger;

        public IWxTeamsClient TeamsClient { get; private set; }

        public WxTeamsApi(IWxTeamsClient teamsClient, IJsonDeserializer serializer,
            ILogger<WxTeamsApi> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            TeamsClient = teamsClient ?? throw new ArgumentNullException(nameof(teamsClient));

            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            serializer.SetApi(this);
        }

        /// <inheritdoc/>
        public void Initialize(string accessToken, string url = WxTeamsConstants.ApiBaseUrl)
            => TeamsClient.SetAuth(accessToken, url);

        private static async Task<string> GetPathWithQueryAsync(string path, List<KeyValuePair<string, string>> parameters)
        {
            if (parameters.Any())
            {
                using (var content = new FormUrlEncodedContent(parameters))
                {
                    var query = await content.ReadAsStringAsync();
                    return $"{path}?{query}";
                }
            }

            return path;
        }
    }
}
