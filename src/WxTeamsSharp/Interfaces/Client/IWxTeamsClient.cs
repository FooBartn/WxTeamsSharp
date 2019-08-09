using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Interfaces.Client
{
    /// <summary>
    /// Client for invoking and deserializing returns from BaseClient
    /// </summary>
    public interface IWxTeamsClient
    {
        /// <summary>
        /// Sets the access token. 
        /// Required before using the API.
        /// </summary>
        /// <param name="token">The token used to access the API</param>
        /// <param name="url">Base URL for API. Has default. Don't change unless you're sure what you're doing</param>
        void SetAuth(string token, string url = WxTeamsConstants.ApiBaseUrl);

        /// <summary>
        /// HTTP GET List of TEntity Items
        /// </summary>
        /// <typeparam name="TEntity">Supplied type of objects to get</typeparam>
        /// <param name="endpoint">Relative URI to target</param>
        /// <returns>Returns an IListResult of supplied TEntity type</returns>
        Task<IListResult<TEntity>> GetResultsAsync<TEntity>(string endpoint)
            where TEntity : TeamsObject;

        /// <summary>
        /// HTTP GET single object
        /// </summary>
        /// <typeparam name="TEntity">Supplied type of object to get</typeparam>
        /// <param name="endpoint">Relative URI to target</param>
        /// <returns>Returns an object of supplied TEntity type</returns>
        Task<TEntity> GetResultAsync<TEntity>(string endpoint)
            where TEntity : TeamsObject;

        /// <summary>
        /// HTTP DELETE single object
        /// </summary>
        /// <typeparam name="TEntity">Supplied type of object to delete</typeparam>
        /// <param name="endpoint">Relative URI to target</param>
        /// <returns>Returns an IResponseMessage which should be "OK"</returns>
        Task<IResponseMessage> DeleteResultAsync<TEntity>(string endpoint);

        /// <summary>
        /// HTTP POST that includes MultipartFormData (File)
        /// </summary>
        /// <typeparam name="TEntity">Supplied type of object to post</typeparam>
        /// <typeparam name="TEntityParams">Supplied type of post body object. Must implement IFormDataParams</typeparam>
        /// <param name="endpoint">Relative URI to target</param>
        /// <param name="requestParams">parameters to post with (Body)</param>
        /// <returns>Returns an object of supplied TEntity type</returns>
        Task<TEntity> PostMultiPartResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IFormDataParams
            where TEntity : TeamsObject;

        /// <summary>
        /// HTTP POST JSON
        /// </summary>
        /// <typeparam name="TEntity">Supplied type of object to post</typeparam>
        /// <typeparam name="TEntityParams">Supplied type of post body object. Must implement IJsonParams</typeparam>
        /// <param name="endpoint">Relative URI to target</param>
        /// <param name="requestParams">parameters to post with (Body)</param>
        /// <returns>Returns an object of supplied TEntity type</returns>
        Task<TEntity> PostResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : TeamsObject;

        /// <summary>
        /// HTTP PUT JSON
        /// </summary>
        /// <typeparam name="TEntity">Supplied type of object to put</typeparam>
        /// <typeparam name="TEntityParams">Supplied type of post body object. Must implement IJsonParams</typeparam>
        /// <param name="endpoint">Relative URI to target</param>
        /// <param name="requestParams">parameters to post with (Body)</param>
        /// <returns>Returns an object of supplied TEntity type</returns>
        Task<TEntity> PutResultAsync<TEntity, TEntityParams>(string endpoint, TEntityParams requestParams)
            where TEntityParams : IJsonParams
            where TEntity : TeamsObject;


    }
}
