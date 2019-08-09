using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Rooms;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi : IWxTeamsApi
    {
        /// <inheritdoc/>   
        public async Task<Room> GetRoomAsync(string roomId)
            => await TeamsClient.GetResultAsync<Room>($"{WxTeamsConstants.RoomsUrl}/{roomId}");

        /// <inheritdoc/>
        public async Task<Room> CreateRoomAsync(string title, string teamId = null)
        {
            var props = new RoomParams { Title = title, TeamId = teamId };
            return await TeamsClient.PostResultAsync<Room, RoomParams>(WxTeamsConstants.RoomsUrl, props);
        }

        /// <inheritdoc/>
        public async Task<IResponseMessage> DeleteRoomAsync(string roomId)
            => await TeamsClient.DeleteResultAsync<Room>($"{WxTeamsConstants.RoomsUrl}/{roomId}");

        /// <inheritdoc/>
        public async Task<Room> UpdateRoomAsync(string roomId, string title)
        {
            var props = new RoomParams { Title = title };
            return await TeamsClient.PutResultAsync<Room, RoomParams>($"{WxTeamsConstants.RoomsUrl}/{roomId}", props);
        }

        /// <inheritdoc/>
        public async Task<IListResult<Room>> GetRoomsAsync(int max = 100, string teamId = "",
            RoomType? type = null, SortBy? sortBy = null)
        {
            var roomParams = new List<KeyValuePair<string, string>>();

            if (max != 100)
                roomParams.Add(new KeyValuePair<string, string>(nameof(max), max.ToString()));

            if (!string.IsNullOrEmpty(teamId))
                roomParams.Add(new KeyValuePair<string, string>(nameof(teamId), teamId));

            if (type != null)
                roomParams.Add(new KeyValuePair<string, string>(nameof(type), type.ToString().ToLower()));

            if (sortBy != null)
                roomParams.Add(new KeyValuePair<string, string>(nameof(sortBy), sortBy.ToString().ToLower()));

            var path = await GetPathWithQueryAsync(WxTeamsConstants.RoomsUrl, roomParams);
            return await TeamsClient.GetResultsAsync<Room>(path);
        }

        /// <inheritdoc/>
        public async Task<MeetingDetails> GetMeetingDetailsAsync(string roomId)
            => await TeamsClient.GetResultAsync<MeetingDetails>($"{WxTeamsConstants.RoomsUrl}/{roomId}/meetingInfo");
    }
}
