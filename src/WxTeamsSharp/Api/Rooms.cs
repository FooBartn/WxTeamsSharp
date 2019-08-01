using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Interfaces.Rooms;
using WxTeamsSharp.Models.Rooms;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string roomsUrl = "/rooms";

        /// <summary>
        /// Shows details for a room, by ID.
        /// The title of the room for 1:1 rooms will be the display name of the other person.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <returns>This method will return a Room</returns>
        public static async Task<IRoom> GetRoomAsync(string roomId)
            => await TeamsClient.GetResultAsync<Room>($"{roomsUrl}/{roomId}");

        /// <summary>
        /// Creates a room. The authenticated user is automatically added as a member of the room. 
        /// To create a 1:1 room, use MessageBuilder.New().SendToPersonId() or .SendToPersonEmail() to send a 
        /// message directly to another person.
        /// </summary>
        /// <param name="title">A user-friendly name for the room.</param>
        /// <param name="teamId">The ID for the team with which this room is associated.</param>
        /// <returns>This method returns the created Room</returns>
        public static async Task<IRoom> CreateRoomAsync(string title, string teamId = null)
        {
            var props = new RoomParams { Title = title, TeamId = teamId };
            return await TeamsClient.PostResultAsync<Room, RoomParams>(roomsUrl, props);
        }

        /// <summary>
        /// Deletes a room, by ID. Deleted rooms cannot be recovered.
        /// Deleting a room that is part of a team will archive the room instead.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <returns>This method returns a ResponseMessage which should be "OK"</returns>
        public static async Task<IResponseMessage> DeleteRoomAsync(string roomId)
            => await TeamsClient.DeleteResultAsync<Room>($"{roomsUrl}/{roomId}");

        /// <summary>
        /// Updates details for a room, by ID.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <param name="title">A user-friendly name for the room.</param>
        /// <returns>This method returns the updated Room</returns>
        public static async Task<IRoom> UpdateRoomAsync(string roomId, string title)
        {
            var props = new RoomParams { Title = title };
            return await TeamsClient.PutResultAsync<Room, RoomParams>($"{roomsUrl}/{roomId}", props);
        }

        /// <summary>
        /// List rooms.
        /// The title of the room for 1:1 rooms will be the display name of the other person.
        /// By default, lists rooms to which the authenticated user belongs.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="max">Limit the maximum number of rooms in the response.</param>
        /// <param name="teamId">List rooms associated with a team, by ID.</param>
        /// <param name="type">List rooms by type.</param>
        /// <param name="sortBy">Sort results.</param>
        /// <returns>This method returns an IListResult object whose Items property is a list of Rooms</returns>
        public static async Task<IListResult<IRoom>> GetRoomsAsync(int max = 100, string teamId = "",
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

            var path = await GetPathWithQueryAsync(roomsUrl, roomParams);
            return await TeamsClient.GetResultsAsync<Room, IRoom>(path);
        }

        /// <summary>
        /// Shows Webex meeting details for a room such as the SIP address, meeting URL, toll-free and toll dial-in numbers.
        /// </summary>
        /// <param name="roomId">The unique identifier for the room.</param>
        /// <returns>This method returns Meeting Details for a room</returns>
        public static async Task<IMeetingDetails> GetMeetingDetailsAsync(string roomId)
            => await TeamsClient.GetResultAsync<MeetingDetails>($"{roomsUrl}/{roomId}/meetingInfo");
    }
}
