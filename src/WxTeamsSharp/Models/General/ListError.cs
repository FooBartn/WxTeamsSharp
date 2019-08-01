using Newtonsoft.Json;
using WxTeamsSharp.Interfaces.General;

namespace WxTeamsSharp.Models.General
{
    internal class ListError : IListError
    {
        public ListError(ErrorTitle title)
        {
            Title = title;
        }

        [JsonProperty(PropertyName = "title")]
        public IErrorTitle Title { get; set; }
    }
}
