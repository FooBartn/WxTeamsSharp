using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Extensions;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.General;
using WxTeamsSharp.Models.Events;

namespace WxTeamsSharp.Api
{
    internal partial class WxTeamsApi
    {
        /// <inheritdoc/>
        public async Task<IListResult<Event>> GetEventsAsync(int max = 100, EventResource? resource = null, EventType? type = null,
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

            var path = await GetPathWithQueryAsync(WxTeamsConstants.EventsUrl, eventParams);
            return await TeamsClient.GetResultsAsync<Event>(path);
        }

        /// <inheritdoc/>
        public async Task<Event> GetEventAsync(string eventId)
            => await TeamsClient.GetResultAsync<Event>($"{WxTeamsConstants.EventsUrl}/{eventId}");
    }
}
