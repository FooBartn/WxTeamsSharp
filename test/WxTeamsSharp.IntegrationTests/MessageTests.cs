using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Models.Messages;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class MessageTests
    {
        public MessageTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            var token = configuration.GetSection("BotToken").Value;
            WxTeamsApi.SetAuth(token);
        }

        [Fact]
        public async Task ShouldGetDirectMessagesById()
        {
            var messages = await WxTeamsApi.GetDirectMessagesAsync(StaticTestingValues.JId);

            messages.Should().NotBeNull();
            messages.HasNextPage.Should().BeFalse();
            messages.Items.Count.Should().BeGreaterOrEqualTo(50);
            messages.Items.First().Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetDirectMessagesByEmail()
        {
            var userEmail = "jbarton@netsyncnetwork.com";
            var messages = await WxTeamsApi.GetDirectMessagesAsync(userEmail);

            messages.Should().NotBeNull();
            messages.HasNextPage.Should().BeFalse();
            messages.Items.Count.Should().BeGreaterOrEqualTo(50);
            messages.Items.First().Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetRoomMessagesViaDirectAPI()
        {
            var messages = await WxTeamsApi.GetRoomMessagesAsync(StaticTestingValues.JRoom);

            messages.Should().NotBeNull();
            messages.HasNextPage.Should().BeTrue();
            messages.Items.Count.Should().BeGreaterOrEqualTo(2);
            messages.Items.First().Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetRoomMessagesLimit10ViaDirectAPI()
        {
            var messages = await WxTeamsApi.GetRoomMessagesAsync(StaticTestingValues.JRoom, 10);

            messages.Should().NotBeNull();
            messages.HasNextPage.Should().BeTrue();
            messages.Items.Count.Should().Be(10);
            messages.Items.First().Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetRoomMessagesBeforeMessageViaAPI()
        {
            var message = await WxTeamsApi.GetMessageAsync(StaticTestingValues.ActivityTestMessage);
            var messages = await WxTeamsApi.GetRoomMessagesBeforeMessageAsync(StaticTestingValues.JRoom, message.Id);

            messages.Should().NotBeNull();
            messages.Items.Count.Should().Be(50);
            messages.HasNextPage.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetRoomMessagesBeforeDateViaAPI()
        {
            var dateTime = DateTimeOffset.UtcNow.AddSeconds(-5);
            await WxTeamsApi.SendMessageAsync(new MessageBuilder().SendToRoom(StaticTestingValues.JRoom).WithText("Hi").Build());
            var allMessages = await WxTeamsApi.GetRoomMessagesAsync(StaticTestingValues.JRoom, max: 10000);
            var messages = await WxTeamsApi.GetRoomMessagesBeforeDateAsync(StaticTestingValues.JRoom, dateTime, max: 10000);

            var latestMessage = allMessages.Items.First();
            messages.Items.Any(x => x.Id == latestMessage.Id).Should().BeFalse();
        }

        [Fact]
        public async Task ShouldGetRoomMessagesBotMentionedViaDirectAPI()
        {
            var messages = await WxTeamsApi.GetRoomMessagesAsync(StaticTestingValues.TestRoom, userMentioned: true);

            messages.Should().NotBeNull();
            messages.HasNextPage.Should().BeFalse();
            messages.Items.Count.Should().BeGreaterOrEqualTo(1);
            messages.Items.First().Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetRoomMessagesViaObject()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var messages = await room.GetMessagesAsync();

            messages.Should().NotBeNull();
            messages.HasNextPage.Should().BeTrue();
            messages.Items.Count.Should().BeGreaterOrEqualTo(2);
            messages.Items.First().Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldGetRoomMessagesLimit10ViaObject()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var messages = await room.GetMessagesAsync(max: 10);

            messages.Should().NotBeNull();
            messages.HasNextPage.Should().BeTrue();
            messages.Items.Count.Should().Be(10);
            messages.Items.First().Text.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldSendBasicMessageToRoomViaObject()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var result = await room.SendPlainMessageAsync("Ello there");

            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Ello there");
        }

        [Fact]
        public async Task ShouldSendMarkdownMessageToRoomViaObject()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var result = await room.SendMessageAsync("**Ello there**");

            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Ello there");
            result.Markdown.Should().Be("**Ello there**");
        }

        [Fact]
        public async Task ShouldSendMarkdownMessageToRoomViaISendMessage()
        {
            var result = await MessageBuilder.New()
                .SendToRoom(StaticTestingValues.JRoom)
                .WithText("Hi!!")
                .Build()
                .SendAsync();

            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Hi!!");
        }

        [Fact]
        public async Task ShouldSendBasicMessageToRoom()
        {
            var newMessage = MessageBuilder.New()
                .SendToRoom(StaticTestingValues.JRoom)
                .WithText("Basic Test")
                .Build();

            var result = await WxTeamsApi.SendMessageAsync(newMessage);
            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Basic Test");
        }

        [Fact]
        public async Task ShouldSendAndDeleteBasicDirectMessageViaAPI()
        {
            var newMessage = MessageBuilder.New()
                .SendToUserId(StaticTestingValues.JId)
                .WithText("Basic Test")
                .Build();

            var result = await WxTeamsApi.SendMessageAsync(newMessage);
            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Basic Test");

            var response = await WxTeamsApi.DeleteMessageAsync(result.Id);
            response.Should().NotBeNull();
            response.Message.Should().Be("OK");
        }

        [Fact]
        public async Task ShouldSendAndDeleteBasicDirectMessageViaObject()
        {
            var newMessage = MessageBuilder.New()
                .SendToUserId(StaticTestingValues.JId)
                .WithText("Basic Test")
                .Build();

            var result = await WxTeamsApi.SendMessageAsync(newMessage);
            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Basic Test");

            var response = await result.DeleteAsync();
            response.Should().NotBeNull();
            response.Message.Should().Be("OK");
        }

        [Fact]
        public async Task ShouldSendBasicDirectMessage()
        {
            var newMessage = MessageBuilder.New()
                .SendToUserId(StaticTestingValues.JId)
                .WithText("Basic Test")
                .Build();

            var result = await WxTeamsApi.SendMessageAsync(newMessage);
            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Basic Test");
        }

        [Fact]
        public async Task ShouldSendMarkdownMessage()
        {
            var newMessage = MessageBuilder.New()
                .SendToUserId(StaticTestingValues.JId)
                .WithMarkdown("**Basic Test**")
                .Build();

            var result = await WxTeamsApi.SendMessageAsync(newMessage);
            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Basic Test");
            result.Markdown.Should().Be("**Basic Test**");
        }

        [Fact]
        public async Task ShouldGetMessageDetails()
        {
            var result = await WxTeamsApi.GetMessageAsync(StaticTestingValues.ActivityTestMessage);

            result.Should().NotBeNull();
            result.AuthorEmail.Should().Contain("Sparkles");
            result.Text.Should().Be("Activity Test");
        }

        [Fact]
        public async Task ShouldSendMarkdownMessageWithPublicFile()
        {
            var newMessage = new MessageBuilder()
                .SendToUserId(StaticTestingValues.JId)
                .WithMarkdown("**Basic Test**")
                .WithPublicFileUrl("https://media.giphy.com/media/jQtQNIfTrakH40ZTyN/giphy.gif")
                .Build();

            var result = await WxTeamsApi.SendMessageAsync(newMessage);
            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Basic Test");
            result.Markdown.Should().Be("**Basic Test**");
            result.Files.Should().NotBeNull();
            result.Files.FirstOrDefault().Should().Contain("https://api.ciscospark.com/v1/contents/");
        }

        [Fact]
        public async Task ShouldSendMarkdownMessageWithLocalFile()
        {
            var newMessage = new MessageBuilder()
                .SendToUserId(StaticTestingValues.JId)
                .WithMarkdown("**Basic Test**")
                .WithLocalFile("Resources/TestFile.txt")
                .Build();

            var result = await WxTeamsApi.SendMessageAsync(newMessage);
            result.Should().NotBeNull();
            result.RoomType.Should().Be(RoomType.Direct);
            result.AuthorEmail.Should().Contain("Unicorn");
            result.Text.Should().Be("Basic Test");
            result.Markdown.Should().Be("**Basic Test**");
            result.Files.Should().NotBeNull();
            result.Files.FirstOrDefault().Should().Contain("https://api.ciscospark.com/v1/contents/");
        }
    }
}
