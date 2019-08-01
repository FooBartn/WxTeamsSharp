using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Interfaces.People;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Models.People
{
    internal class PeopleParams : ICreateablePerson, IUpdateablePerson
    {
        private readonly string _id;

        public PeopleParams() { }
        public PeopleParams(string id) { _id = id; }

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

        public async Task<IPerson> CreateAsync()
            => await WxTeamsApi.CreateUserAsync(this);

        public string ToJson() => JsonConvert.SerializeObject(this, JsonUtilities.Standard);

        public async Task<IPerson> UpdateAsync()
            => await WxTeamsApi.UpdateUserAsync(_id, this);
    }
}
