using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Roles;
using WxTeamsSharp.Models.General;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.Roles
{
    /// <inheritdoc/>
    internal class Role : IRole
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }
    }
}
