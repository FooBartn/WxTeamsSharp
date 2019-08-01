using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Models.Exceptions;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class WebhookTests
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
            var webhooks = await WxTeamsApi.GetWebhooksAsync();

            webhooks.Should().NotBeNull();
            webhooks.Items.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task ShouldGetWebhook()
        {
            var webhook = await WxTeamsApi.GetWebhookAsync(StaticTestingValues.SparklyWebhook);

            webhook.Name.Should().Be("Sparkly WebHook");
            webhook.TargetUrl.Should().Be("http://unicorncentral.com/mywebhookreciever");
            webhook.Resource.Should().Be(WebhookResource.Messages);
            webhook.Event.Should().Be(EventType.Created);
            webhook.Filter.Should().Contain(StaticTestingValues.JRoom);
            webhook.OrganizationId.Should().NotBeNullOrEmpty();
            webhook.CreatedBy.Should().Be(StaticTestingValues.BotId);
            webhook.ApplicationId.Should().NotBeNullOrEmpty();
            webhook.OwnedBy.Should().Be("creator");
            webhook.Status.Should().Be(WebhookStatus.Active);
        }
    }
}
