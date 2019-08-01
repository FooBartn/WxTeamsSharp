using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.General
{
    /// <inheritdoc/>
    internal class ItemsResult<TItem> : IListResult<TItem>
    {
        public ItemsResult() { }
        public ItemsResult(IList<TItem> items, string nextPage)
        {
            Items = items;
            NextPage = nextPage;
        }

        internal string NextPage { get; set; } = string.Empty;

        public IList<TItem> Items { get; set; } = new List<TItem>();
        public bool HasNextPage => !string.IsNullOrEmpty(NextPage);
        public List<string> NotFoundIds { get; set; }

        public async Task<IListResult<TItem>> GetNextPage()
        {
            if (HasNextPage)
                return await TeamsClient.GetResultAsync<ItemsResult<TItem>>(NextPage);

            return new ItemsResult<TItem>();
        }
    }
}
