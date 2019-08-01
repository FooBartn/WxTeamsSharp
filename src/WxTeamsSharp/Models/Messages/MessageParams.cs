using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Interfaces.Messages;
using WxTeamsSharp.Utilities;

namespace WxTeamsSharp.Models.Messages
{
    internal class MessageParams : ISendableMessage
    {
        private readonly bool _hasLocalFile;

        public MessageParams(bool hasLocalFile)
        {
            _hasLocalFile = hasLocalFile;
        }

        [JsonProperty("roomId")]
        public string RoomId { get; set; }
        [JsonProperty("toPersonId")]
        public string ToPersonId { get; set; }
        [JsonProperty("toPersonEmail")]
        public string ToPersonEmail { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("markdown")]
        public string Markdown { get; set; }
        [JsonProperty("files")]
        public List<string> Files { get; set; }

        public bool HasLocalFile() => _hasLocalFile;

        public string ToJson() => JsonConvert.SerializeObject(this, JsonUtilities.IgnoreNull);
        public MultipartFormDataContent ToFormData()
        {
            var formData = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(RoomId))
                formData.Add(new StringContent(RoomId), nameof(RoomId).FirstCharToLower());

            if (!string.IsNullOrEmpty(ToPersonId))
                formData.Add(new StringContent(ToPersonId), nameof(ToPersonId).FirstCharToLower());

            if (!string.IsNullOrEmpty(ToPersonEmail))
                formData.Add(new StringContent(ToPersonEmail), nameof(ToPersonEmail).FirstCharToLower());

            if (!string.IsNullOrEmpty(Text))
                formData.Add(new StringContent(Text), nameof(Text).FirstCharToLower());

            if (!string.IsNullOrEmpty(Markdown))
                formData.Add(new StringContent(Markdown), nameof(Markdown).FirstCharToLower());

            if (Files.Any() && _hasLocalFile)
            {
                var file = Files.FirstOrDefault();

                if (!File.Exists(file))
                    throw new ArgumentException($"File could not be found: {file}");

                var fileInfo = new FileInfo(file);
                var fileContents = File.ReadAllBytes(fileInfo.FullName);

                ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
                byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
                formData.Add(byteArrayContent, "files", fileInfo.Name);
            }

            return formData;
        }

        public async Task<IMessage> SendAsync()
            => await WxTeamsApi.SendMessageAsync(this);
    }
}
