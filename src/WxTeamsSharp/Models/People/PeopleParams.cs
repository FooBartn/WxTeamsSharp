using Newtonsoft.Json;
using System.Collections.Generic;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.People;

namespace WxTeamsSharp.Models.People
{
    internal class PeopleParams : IUpdateablePerson, ICreateablePerson
    {
        public PeopleParams() { }
        public PeopleParams(string id) { Id = id; }

        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "emails")]
        public List<string> Emails { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }

        [JsonProperty(PropertyName = "orgId")]
        public string OrganizationId { get; set; }

        [JsonProperty(PropertyName = "roles")]
        public List<string> Roles { get; set; }

        [JsonProperty(PropertyName = "licenses")]
        public List<string> Licenses { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this, JsonSettings.Standard);
    }
}
