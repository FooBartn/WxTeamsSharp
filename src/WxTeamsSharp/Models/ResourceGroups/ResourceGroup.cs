using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.ResourceGroups;
using WxTeamsSharp.Models.General;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.ResourceGroups
{
    /// <inheritdoc/>
    internal class ResourceGroup : IResourceGroup
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string OrgId { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }
    }
}
