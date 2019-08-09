using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WxTeamsSharp.Converters;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Models.General;

namespace WxTeamsSharp.Models.Memberships
{
    /// <inheritdoc/>
    public class RoomMembership : TeamsObject, IMembership<RoomMembership>
    {

        /// <inheritdoc/>
        [JsonProperty]
        public string Id { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string RoomId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string PersonId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string PersonEmail { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string PersonDisplayName { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public string PersonOrgId { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public bool IsModerator { get; private set; }

        /// <inheritdoc/>
        [JsonProperty]
        public DateTimeOffset Created { get; private set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; private set; }

        /// <inheritdoc/>
        public bool HasError => Error != null;

        /// <inheritdoc/>
        public async Task<RoomMembership> UpdateAsync(bool isModerator)
            => await TeamsApi.UpdateMembershipAsync(Id, isModerator);

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync()
            => await TeamsApi.DeleteMembershipAsync(Id);

    }
}
