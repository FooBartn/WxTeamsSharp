using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Client;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Events;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Events;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Api
{
    public static partial class WxTeamsApi
    {
        private static readonly string eventsUrl = "/events";

        /// <summary>
        /// List events in your organization. Several query parameters are available to filter the response.
        /// Long result sets will be split into pages.
        /// </summary>
        /// <param name="max">Limit the maximum number of events in the response.</param>
        /// <param name="resource">List events with a specific resource type.</param>
        /// <param name="type">List events with a specific event type.</param>
        /// <param name="actorId">List events performed by this person, by ID.</param>
        /// <param name="from">List events which occurred after a specific date and time.</param>
        /// <param name="to">List events which occurred before a specific date and time.</param>
        /// <returns>The method returns an IListResult object whose Items property is a list of Events</returns>
        public static async Task<IListResult<IEvent>> GetEventsAsync(int max = 100, EventResource? resource = null, EventType? type = null,
            string actorId = "", DateTimeOffset from = default, DateTimeOffset to = default)
        {
            var eventParams = new List<KeyValuePair<string, string>>();

            if (max != 100)
                eventParams.Add(new KeyValuePair<string, string>(nameof(max), max.ToString()));

            if (resource != null)
                eventParams.Add(new KeyValuePair<string, string>(nameof(resource), resource.ToString().FirstCharToLower()));

            if (type != null)
                eventParams.Add(new KeyValuePair<string, string>(nameof(type), type.ToString().FirstCharToLower()));

            if (!string.IsNullOrEmpty(actorId))
                eventParams.Add(new KeyValuePair<string, string>(nameof(actorId), actorId));

            if (from != DateTimeOffset.MinValue)
                eventParams.Add(new KeyValuePair<string, string>(nameof(from), from.ToFormattedUTCTime()));

            if (to != DateTimeOffset.MinValue)
                eventParams.Add(new KeyValuePair<string, string>(nameof(to), to.ToFormattedUTCTime()));

            var path = await GetPathWithQueryAsync(eventsUrl, eventParams);
            return await TeamsClient.GetResultsAsync<Event, IEvent>(path);
        }

        /// <summary>
        /// Shows details for an event, by event ID.
        /// </summary>
        /// <param name="eventId">The unique identifier for the event.</param>
        /// <returns>This method returns an Event</returns>
        public static async Task<IEvent> GetEventAsync(string eventId)
            => await TeamsClient.GetResultAsync<Event>($"{eventsUrl}/{eventId}");
    }
}
