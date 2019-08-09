using Newtonsoft.Json;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.ResourceGroups;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.ResourceGroups
{
    /// <inheritdoc/>
    public class ResourceGroup : TeamsObject, IResourceGroup
    {
        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string Name { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string OrgId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }

        /// <inheritdoc/>
        public bool HasError => Error != null;
    }
}
