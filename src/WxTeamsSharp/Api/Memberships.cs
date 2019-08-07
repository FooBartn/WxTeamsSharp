using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Memberships;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IListResult<RoomMembership>> GetAllMembershipsAsync(int max = 100)
        {
            var membershipParams = BuildMembershipQuery(max, null, null);
            var path = await GetPathWithQueryAsync(WxTeamsConstants.MembershipsUrl, membershipParams);
            return await TeamsClient.GetResultsAsync<RoomMembership>(path);
        }

        /// <inheritdoc/>
        public async Task<IListResult<RoomMembership>> GetMembershipsAssociatedWithAsync(string userIdOrEmail, int max = 100)
        {
            var membershipParams = BuildMembershipQuery(max, userIdOrEmail, null);
            var path = await GetPathWithQueryAsync(WxTeamsConstants.MembershipsUrl, membershipParams);
            return await TeamsClient.GetResultsAsync<RoomMembership>(path);
        }

        /// <inheritdoc/>
        public async Task<IListResult<RoomMembership>> GetRoomMembershipsAsync(string roomId, int max = 100)
        {
            var membershipParams = BuildMembershipQuery(max, null, roomId);
            var path = await GetPathWithQueryAsync(WxTeamsConstants.MembershipsUrl, membershipParams);
            return await TeamsClient.GetResultsAsync<RoomMembership>(path);
        }

        /// <inheritdoc/>
        public async Task<RoomMembership> AddUserToRoomAsync(string roomId, string userIdOrEmail, bool IsModerator = false)
        {
            var props = new MembershipParams { RoomId = roomId, IsModerator = IsModerator };

            if (RegexUtilities.IsValidEmail(userIdOrEmail))
                props.PersonEmail = userIdOrEmail;
            else
                props.PersonId = userIdOrEmail;

            return await TeamsClient.PostResultAsync<RoomMembership, MembershipParams>(WxTeamsConstants.MembershipsUrl, props);
        }

        /// <inheritdoc/>
        public async Task<RoomMembership> UpdateMembershipAsync(string membershipId, bool isModerator)
        {
            var props = new MembershipParams { IsModerator = isModerator };
            return await TeamsClient.PutResultAsync<RoomMembership, MembershipParams>($"{WxTeamsConstants.MembershipsUrl}/{membershipId}", props);
        }

        /// <inheritdoc/>
        public async Task<RoomMembership> GetMembershipAsync(string membershipId)
            => await TeamsClient.GetResultAsync<RoomMembership>($"{WxTeamsConstants.MembershipsUrl}/{membershipId}");

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteMembershipAsync(string membershipId)
            => await TeamsClient.DeleteResultAsync<RoomMembership>($"{WxTeamsConstants.MembershipsUrl}/{membershipId}");

        /// <inheritdoc/>
        private static List<KeyValuePair<string, string>> BuildMembershipQuery(int max, string userIdOrEmail, string roomId)
        {
            var membershipParams = new List<KeyValuePair<string, string>>();

            if (max != 100)
                membershipParams.Add(new KeyValuePair<string, string>(nameof(max), max.ToString()));

            if (!string.IsNullOrEmpty(userIdOrEmail))
            {
                var queryBy = "personId";

                if (RegexUtilities.IsValidEmail(userIdOrEmail))
                    queryBy = "personEmail";

                membershipParams.Add(new KeyValuePair<string, string>(queryBy, userIdOrEmail));
            }

            if (!string.IsNullOrEmpty(roomId))
            {
                membershipParams.Add(new KeyValuePair<string, string>(nameof(roomId), roomId));
            }

            return membershipParams;
        }
    }
}
