using System.Collections.Generic;
using System.Threading.Tasks;

namespace WxTeamsSharp.Interfaces.General
{
    /// <summary>
    /// Webex Teams returns an array of Items when getting multiple objects such as rooms.
    /// </summary>
    /// <typeparam name="TItem">Type for list of results from API</typeparam>
    public interface IListResult<TItem> : ITeamsObject
    {
        // TODO: Should make items a readonly collection

        /// <summary>
        /// List of items of specified type
        /// </summary>
        List<TItem> Items { get; }

        /// <summary>
        /// Whether the list has a next page or not
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Method for retrieving next page
        /// </summary>
        /// <returns></returns>
        Task<IListResult<TItem>> GetNextPage();

        /// <summary>
        /// If searching by Ids, this includes any not found
        /// </summary>
        List<string> NotFoundIds { get; }
    }
}
