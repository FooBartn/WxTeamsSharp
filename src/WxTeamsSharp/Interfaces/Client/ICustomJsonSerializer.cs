using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Interfaces.Client
{
    /// <summary>
    /// Custom Serializer that adds in API to serialized objects
    /// </summary>
    public interface IJsonDeserializer
    {
        /// <summary>
        /// Used to set the IWxTeamsApi. Called from WxTeamsApi constructor.
        /// </summary>
        /// <param name="api"></param>
        void SetApi(IWxTeamsApi api);

        /// <summary>
        /// Deserialize a single object of type TEntity
        /// </summary>
        /// <typeparam name="TEntity">Type of object that is expected</typeparam>
        /// <param name="reader">Pass in a JsonTextReader that is reading a stream</param>
        /// <returns>An object of type TEntity</returns>
        TEntity Deserialize<TEntity>(JsonTextReader reader)
            where TEntity : TeamsObject;

        /// <summary>
        /// Deserialize multiple objects of type TEntity into an IListResult<typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Type of object that is expected</typeparam>
        /// <param name="reader">Pass in a JsonTextReader that is reading a stream</param>
        /// <returns>An IListResult with a list of items of type TEntity</returns>
        IListResult<TEntity> DeserializeList<TEntity>(JsonTextReader reader)
            where TEntity : TeamsObject;
    }
}
