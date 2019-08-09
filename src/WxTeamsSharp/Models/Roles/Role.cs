using Newtonsoft.Json;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Roles;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Roles
{
    /// <inheritdoc/>
    public class Role : TeamsObject, IRole
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Name { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; private set; }

        /// <inheritdoc/>
        public bool HasError => Error != null;
    }
}
