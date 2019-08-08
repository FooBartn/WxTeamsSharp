using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Client
{
    internal class CustomJsonDeserializer : IJsonDeserializer
    {
        private IWxTeamsApi _wxTeamsApi;
        private readonly JsonSerializer _jsonSerializer;

        public CustomJsonDeserializer()
        {
            _jsonSerializer = new JsonSerializer
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                }
            };
        }

        public void SetApi(IWxTeamsApi api)
            => _wxTeamsApi = api;

        public TEntity Deserialize<TEntity>(JsonTextReader reader)
            where TEntity : TeamsObject
        {
            var entity = _jsonSerializer.Deserialize<TEntity>(reader);
            entity.TeamsApi = _wxTeamsApi;

            return entity;
        }

        public IListResult<TEntity> DeserializeList<TEntity>(JsonTextReader reader)
            where TEntity : TeamsObject
        {
            var itemsResult = _jsonSerializer.Deserialize<ItemsResult<TEntity>>(reader);

            for (int i = 0; i < itemsResult.Items.Count; i++)
            {
                itemsResult.Items[i].TeamsApi = _wxTeamsApi;
            }

            itemsResult.TeamsApi = _wxTeamsApi;

            return itemsResult;
        }
    }
}
