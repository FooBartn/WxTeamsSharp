using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Models.Exceptions;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Webhooks;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class WebhookTests : IDisposable
    {
        public WebhookTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            var token = configuration.GetSection("BotToken").Value;
            WxTeamsApi.SetAuth(token);
        }

        [Fact]
        public async Task ShouldCreate_AndDelete_Webhook_ViaAPI()
        {
            var webhook = await WxTeamsApi.CreateWebhookAsync("Sparkly TestHook1", "http://unicorncentral.com/sparklywebhookreciever1",
                WebhookResource.Messages, EventType.Created, filter: $"roomId={StaticTestingValues.JRoom}");

            webhook.Should().NotBeNull();
            webhook.Name.Should().Be("Sparkly TestHook1");
            webhook.Resource.Should().Be(WebhookResource.Messages);
            webhook.Event.Should().Be(EventType.Created);
            webhook.Filter.Should().Contain(StaticTestingValues.JRoom);

            var deleted = await WxTeamsApi.DeleteWebhookAsync(webhook.Id);
            deleted.Message.Should().Be("OK");

            Func<Task> webhookNotFound = async () => await WxTeamsApi.GetWebhookAsync(webhook.Id);
            webhookNotFound.Should().Throw<TeamsApiException>().WithMessage("The requested resource could not be found.");

        }

        [Fact]
        public async Task ShouldCreate_Update_AndDelete_Webhook_ViaAPI()
        {
            var webhook = await WxTeamsApi.CreateWebhookAsync("Sparkly TestHook2", "http://unicorncentral.com/sparklywebhookreciever2",
                WebhookResource.Messages, EventType.Created, filter: $"roomId={StaticTestingValues.JRoom}");

            webhook.Should().NotBeNull();
            webhook.Name.Should().Be("Sparkly TestHook2");
            webhook.Resource.Should().Be(WebhookResource.Messages);
            webhook.Event.Should().Be(EventType.Created);
            webhook.Filter.Should().Contain(StaticTestingValues.JRoom);

            var updated = await WxTeamsApi.UpdateWebhookAsync(webhook.Id, "NoName", webhook.TargetUrl);
            updated.Name.Should().Be("NoName");

            var deleted = await WxTeamsApi.DeleteWebhookAsync(webhook.Id);
            deleted.Message.Should().Be("OK");

            Func<Task> webhookNotFound = async () => await WxTeamsApi.GetWebhookAsync(webhook.Id);
            webhookNotFound.Should().Throw<TeamsApiException>().WithMessage("The requested resource could not be found.");

        }

        [Fact]
        public async Task ShouldCreate_Update_AndDelete_Webhook_ViaObject()
        {
            var webhook = await WxTeamsApi.CreateWebhookAsync("Sparkly TestHook3", "http://unicorncentral.com/sparklywebhookreciever2",
                WebhookResource.Messages, EventType.Created, filter: $"roomId={StaticTestingValues.JRoom}");

            webhook.Should().NotBeNull();
            webhook.Name.Should().Be("Sparkly TestHook3");
            webhook.Resource.Should().Be(WebhookResource.Messages);
            webhook.Event.Should().Be(EventType.Created);
            webhook.Filter.Should().Contain(StaticTestingValues.JRoom);

            var updated = await webhook.UpdateAsync(name: "NoName");
            updated.Name.Should().Be("NoName");

            var deleted = await webhook.DeleteAsync();
            deleted.Message.Should().Be("OK");

            Func<Task> webhookNotFound = async () => await WxTeamsApi.GetWebhookAsync(webhook.Id);
            webhookNotFound.Should().Throw<TeamsApiException>().WithMessage("The requested resource could not be found.");

        }

        [Fact]
        public async Task ShouldGetWebhooks()
        {
            var webhook = await WxTeamsApi.CreateWebhookAsync("Sparkly Webhook", "http://unicorncentral.com/mywebhookreceiver",
                WebhookResource.Messages, EventType.Created, filter: $"roomId={StaticTestingValues.JRoom}");

            var webhooks = await WxTeamsApi.GetWebhooksAsync();

            webhooks.Should().NotBeNull();
            webhooks.Items.Count.Should().BeGreaterOrEqualTo(1);

            await webhook.DeleteAsync();
        }

        [Fact]
        public async Task ShouldGetWebhook()
        {
            var webhook = await WxTeamsApi.CreateWebhookAsync("Sparkly Webhook", "http://unicorncentral.com/mywebhookreceiver",
                WebhookResource.Messages, EventType.Created, filter: $"roomId={StaticTestingValues.JRoom}");
            var sameWebhook = await WxTeamsApi.GetWebhookAsync(webhook.Id);

            sameWebhook.Name.Should().Be("Sparkly Webhook");
            sameWebhook.TargetUrl.Should().Be("http://unicorncentral.com/mywebhookreceiver");
            sameWebhook.Resource.Should().Be(WebhookResource.Messages);
            sameWebhook.Event.Should().Be(EventType.Created);
            sameWebhook.Filter.Should().Contain(StaticTestingValues.JRoom);
            sameWebhook.OrganizationId.Should().NotBeNullOrEmpty();
            sameWebhook.CreatedBy.Should().Be(StaticTestingValues.BotId);
            sameWebhook.ApplicationId.Should().NotBeNullOrEmpty();
            sameWebhook.OwnedBy.Should().Be("creator");
            sameWebhook.Status.Should().Be(WebhookStatus.Active);

            await sameWebhook.DeleteAsync();
        }

        [Fact]
        public async Task ShouldGetWebhookDataJson_ThenFullMessage()
        {
            var json = File.ReadAllText("Resources/WebhookPost.json");
            var webhookData = JsonConvert.DeserializeObject<WebhookData<Message>>(json);

            var fullMessage = await webhookData.Data.GetFullMessageAsync();

            fullMessage.Text.Should().Be("Activity Test");
        }

        [Fact]
        public async Task ShouldCreateWebhook_FromRoom_AndDelete()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var webhook = await room.AddMessageCreatedWebhookAsync("Sparkly TestHook4", "http://example.com/woot");


            webhook.Should().NotBeNull();
            webhook.Name.Should().Be("Sparkly TestHook4");
            webhook.Resource.Should().Be(WebhookResource.Messages);
            webhook.Event.Should().Be(EventType.Created);
            webhook.Filter.Should().Contain(StaticTestingValues.JRoom);

            var updated = await webhook.UpdateAsync(name: "NoName");
            updated.Name.Should().Be("NoName");

            var deleted = await webhook.DeleteAsync();
            deleted.Message.Should().Be("OK");

            Func<Task> webhookNotFound = async () => await WxTeamsApi.GetWebhookAsync(webhook.Id);
            webhookNotFound.Should().Throw<TeamsApiException>().WithMessage("The requested resource could not be found.");

        }

        public void Dispose()
        {
            var webhooks = WxTeamsApi.GetWebhooksAsync().GetAwaiter().GetResult();

            foreach (var webhook in webhooks.Items.Where(x => x.Name != "API Test Webhook"))
            {
                webhook.DeleteAsync().GetAwaiter().GetResult();
            }
        }
    }
}
