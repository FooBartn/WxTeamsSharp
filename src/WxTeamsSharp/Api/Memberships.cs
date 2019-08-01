using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Memberships;
using WxTeamsSharp.Models.Memberships;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string membershipUrl = "/memberships";

        /// <summary>
        /// Lists all room memberships.
        /// </summary>
        /// <param name="max">Limit the maximum number of memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of IMembership</returns>
        public static async Task<IListResult<IMembership>> GetAllMembershipsAsync(int max = 100)
        {
            var membershipParams = BuildMembershipQuery(max, null, null);
            var path = await GetPathWithQueryAsync(membershipUrl, membershipParams);
            return await TeamsClient.GetResultsAsync<RoomMembership, IMembership>(path);
        }


        /// <summary>
        /// Lists all room memberships associated with a particular user by userId or email
        /// </summary>
        /// <param name="userIdOrEmail">The Id or Email memberships should be associated with</param>
        /// <param name="max">Limit the maximum number of memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of IMembership</returns>
        public static async Task<IListResult<IMembership>> GetMembershipsAssociatedWithAsync(string userIdOrEmail, int max = 100)
        {
            var membershipParams = BuildMembershipQuery(max, userIdOrEmail, null);
            var path = await GetPathWithQueryAsync(membershipUrl, membershipParams);
            return await TeamsClient.GetResultsAsync<RoomMembership, IMembership>(path);
        }

        /// <summary>
        /// Lists all room memberships associated with a specific room
        /// </summary>
        /// <param name="roomId">Id of the room to which memberships should be associated</param>
        /// <param name="max">Limit the maximum number of memberships in the response.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of IMembership</returns>
        public static async Task<IListResult<IMembership>> GetRoomMembershipsAsync(string roomId, int max = 100)
        {
            var membershipParams = BuildMembershipQuery(max, null, roomId);
            var path = await GetPathWithQueryAsync(membershipUrl, membershipParams);
            return await TeamsClient.GetResultsAsync<RoomMembership, IMembership>(path);
        }

        /// <summary>
        /// Add someone to a room by Person ID or email address; optionally making them a moderator.
        /// </summary>
        /// <param name="roomId">The room ID</param>
        /// <param name="userIdOrEmail">The ID or email address of the person</param>
        /// <param name="IsModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>This method returns the Membership that was created</returns>
        public static async Task<IMembership> AddUserToRoomAsync(string roomId, string userIdOrEmail, bool IsModerator = false)
        {
            var props = new MembershipParams { RoomId = roomId, IsModerator = IsModerator };

            if (RegexUtilities.IsValidEmail(userIdOrEmail))
                props.PersonEmail = userIdOrEmail;
            else
                props.PersonId = userIdOrEmail;

            return await TeamsClient.PostResultAsync<RoomMembership, MembershipParams>(membershipUrl, props);
        }

        /// <summary>
        /// Updates properties for a membership by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <param name="isModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>This method returns the Membership that was updated</returns>
        public static async Task<IMembership> UpdateMembershipAsync(string membershipId, bool isModerator)
        {
            var props = new MembershipParams { IsModerator = isModerator };
            return await TeamsClient.PutResultAsync<RoomMembership, MembershipParams>($"{membershipUrl}/{membershipId}", props);
        }

        /// <summary>
        /// Get details for a membership by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <returns>This method returns a Membership</returns>
        public static async Task<IMembership> GetMembershipAsync(string membershipId)
            => await TeamsClient.GetResultAsync<RoomMembership>($"{membershipUrl}/{membershipId}");

        /// <summary>
        /// Deletes a membership by ID.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <returns>This method returns a response message which should be "OK"</returns>
        public static async Task<IResponseMessage> DeleteMembershipAsync(string membershipId)
            => await TeamsClient.DeleteResultAsync<RoomMembership>($"{membershipUrl}/{membershipId}");

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
