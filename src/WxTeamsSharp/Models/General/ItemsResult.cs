using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.General
{
    /// <inheritdoc/>
    internal class ItemsResult<TItem> : TeamsObject, IListResult<TItem>
        where TItem : TeamsObject
    {

        public ItemsResult() { }
        public ItemsResult(List<TItem> items, string nextPage)
        {
            Items = new ReadOnlyCollection<TItem>(items);
            NextPage = nextPage;
        }

        internal string NextPage { get; private set; } = string.Empty;

        [JsonProperty]
        public ReadOnlyCollection<TItem> Items { get; private set; }
            = new ReadOnlyCollection<TItem>(new List<TItem>());

        public bool HasNextPage => !string.IsNullOrEmpty(NextPage);

        [JsonProperty]
        public List<string> NotFoundIds { get; set; }

        public async Task<IListResult<TItem>> GetNextPage()
        {
            if (HasNextPage)
                return await TeamsApi.TeamsClient.GetResultsAsync<TItem>(NextPage);

            return new ItemsResult<TItem>();
        }

        internal void SetNextPage(string nextPage) => NextPage = nextPage;
    }
}
