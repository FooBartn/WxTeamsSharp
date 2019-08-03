using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Webhooks;
using Xunit;

namespace WxTeamsWebhookReceiver.IntegrationTests
{
    public class WebhookTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly WebApplicationFactory<Startup> _factory;

        public WebhookTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestMessagesWebhook_FromStaticResource()
        {
            var client = _factory.CreateClient();
            var file = File.ReadAllText("Resources/WebhookPost.json");
            var content = new StringContent(file, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/messages", content);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var webhook = JsonConvert.DeserializeObject<WebhookData<Message>>(responseContent);

            webhook.Data.Should().BeOfType<Message>();
            webhook.Data.Id.Should().Be("Y2lzY29zcGFyazovL3VzL01FU1NBR0UvOTJkYjNiZTAtNDNiZC0xMWU2LThhZTktZGQ1YjNkZmM1NjVk");
        }
    }
}
