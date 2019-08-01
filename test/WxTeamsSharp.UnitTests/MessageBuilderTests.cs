using FluentAssertions;
using WxTeamsSharp.Models.Messages;
using Xunit;

namespace WxTeamsSharp.UnitTests
{
    public class MessageBuilderTests
    {
        [Fact]
        public void ShouldBuildMarkdownMessage()
        {
            var sendMessage = MessageBuilder.New()
                .SendToUserId("myId")
                .WithMarkdown("**Heyo**")
                .Build();

            sendMessage.Should().NotBeNull();
        }

        [Fact]
        public void ShouldBuildMarkdownMessageWithFileAndExportFormData()
        {
            var sendMessage = MessageBuilder.New()
                .SendToUserId("myId")
                .WithMarkdown("**Heyo**")
                .WithLocalFile("Resources/Event1.json")
                .Build();

            sendMessage.Should().NotBeNull();

            var formData = sendMessage.ToFormData();

            formData.Should().NotBeNull();
        }

        [Fact]
        public void ShouldBuildMarkdownMessageWithFileAndExportJson()
        {
            var sendMessage = MessageBuilder.New()
                .SendToUserId("myId")
                .WithMarkdown("**Heyo**")
                .WithPublicFileUrl("https://media.giphy.com/media/ekeAWR25HbQqG3UxlE/giphy.gif")
                .Build();

            sendMessage.Should().NotBeNull();

            var json = sendMessage.ToJson();
            json.Should().NotBeNull();
            json.Should().Contain("myId");
            json.Should().Contain("**Heyo**");
            json.Should().Contain("giphy");
        }
    }
}
