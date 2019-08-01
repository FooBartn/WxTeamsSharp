using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Models.General;
using static WxTeamsSharp.Utilities.JsonUtilities;

namespace WxTeamsSharp.Models.Memberships
{
    /// <inheritdoc/>
    internal class RoomMembership : IMembership
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string RoomId { get; set; }

        /// <inheritdoc/>
        public string PersonId { get; set; }

        /// <inheritdoc/>
        public string PersonEmail { get; set; }

        /// <inheritdoc/>
        public string PersonDisplayName { get; set; }

        /// <inheritdoc/>
        public string PersonOrgId { get; set; }

        /// <inheritdoc/>
        public bool IsModerator { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        [JsonProperty(PropertyName = "errors")]
        [JsonConverter(typeof(ConcreteConverter<ListError>))]
        public IListError Error { get; set; }

        /// <inheritdoc/>
        public async Task<IMembership> UpdateAsync(bool isModerator)
            => await WxTeamsApi.UpdateMembershipAsync(Id, isModerator);

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteAsync()
            => await WxTeamsApi.DeleteMembershipAsync(Id);

    }
}
