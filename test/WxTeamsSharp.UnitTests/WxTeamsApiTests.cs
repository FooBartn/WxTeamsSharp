using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Client;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Client;
using WxTeamsSharp.Models.Events;
using WxTeamsSharp.Models.Messages;
using Xunit;

namespace WxTeamsSharp.UnitTests
{
    public class WxTeamsApiTests
    {
        [Theory]
        [InlineData("Resources/Message1.json", RoomType.Direct)]
        [InlineData("Resources/Message2.json", RoomType.Group)]
        public async Task ShouldReturn_Valid_MessageObject(string file, RoomType roomType)
        {
            var messageContent = File.ReadAllText(file);
            var mockBaseClient = new Mock<IWxBaseClient>();
            mockBaseClient.Setup(x => x.GetHttpResponse<Message>(HttpMethod.Get, $"{WxTeamsConstants.MessagesUrl}/MyMessageId", null, null))
                .ReturnsAsync(new HttpResponseMessage { Content = new StringContent(messageContent) });

            var logger = new Mock<ILogger<WxTeamsApi>>();
            var deserializer = new CustomJsonDeserializer();
            var teamsClient = new TeamsClient(deserializer, mockBaseClient.Object);
            var api = new WxTeamsApi(teamsClient, deserializer, logger.Object);

            var message = await api.GetMessageAsync("MyMessageId");

            message.AuthorEmail.Should().Be("jbarton@netsyncnetwork.com");
            message.AuthorId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9hMGJlZGI3Yy0wOWNiLTQ1OGQtOTVkNS0xNjBlNjAwMWI3MzA");
            message.Id.Should().Be("Y2lzY29zcGFyazovL3VzL01FU1NBR0UvMTk4YTIzODAtYTgzNC0xMWU5LWE2ZjktYTNiYTg1NGIxZmU3");
            message.RoomId.Should().Be("Y2lzY29zcGFyazovL3VzL1JPT00vNDIxNWU1ZDEtODRlOS0zMjc4LWFmMmYtNjM1NTNiOTcxZjg2");
            message.RoomType.Should().Be(roomType);
            message.TeamsApi.Should().NotBeNull();
            message.TeamsApi.TeamsClient.Should().NotBeNull();
            message.Text.Should().Be("piece");
        }

        // TODO: Add a test that includes next page headers

        [Fact]
        public async Task ShouldReturn_Valid_IListResult_RoomMessages()
        {
            var messageContent = File.ReadAllText("Resources/Messages.json");
            var mockBaseClient = new Mock<IWxBaseClient>();
            mockBaseClient.Setup(x => x.GetHttpResponse<Message>(HttpMethod.Get, $"{WxTeamsConstants.MessagesUrl}?roomId=MyRoomId", null, null))
                .ReturnsAsync(new HttpResponseMessage { Content = new StringContent(messageContent) });

            var logger = new Mock<ILogger<WxTeamsApi>>();
            var deserializer = new CustomJsonDeserializer();
            var teamsClient = new TeamsClient(deserializer, mockBaseClient.Object);
            var api = new WxTeamsApi(teamsClient, deserializer, logger.Object);

            var listResult = await api.GetRoomMessagesAsync("MyRoomId");

            listResult.HasNextPage.Should().BeFalse();
            listResult.NotFoundIds.Should().BeNull();
            listResult.TeamsApi.Should().NotBeNull();
            listResult.TeamsApi.TeamsClient.Should().NotBeNull();
            listResult.Items.Count.Should().Be(2);

            for (int i = 0; i < listResult.Items.Count; i++)
            {
                listResult.Items[i].AuthorEmail.Should().Be("jbarton@netsyncnetwork.com");
                listResult.Items[i].AuthorId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9hMGJlZGI3Yy0wOWNiLTQ1OGQtOTVkNS0xNjBlNjAwMWI3MzA");
                listResult.Items[i].RoomType.Should().Be(RoomType.Direct);
            }

            listResult.Items[0].Text.Should().Be("piece");
            listResult.Items[1].Text.Should().Be("hello");
        }

        [Theory]
        [InlineData("Resources/Event1.json", EventResource.Messages, EventType.Created, RoomType.Group)]
        [InlineData("Resources/Event2.json", EventResource.Memberships, EventType.Updated, RoomType.Direct)]
        public async Task ShouldReturn_Valid_EventObject(string file, EventResource eventResource, EventType eventType, RoomType roomType)
        {
            // Arrange
            var messageContent = File.ReadAllText(file);
            var mockBaseClient = new Mock<IWxBaseClient>();
            mockBaseClient.Setup(x => x.GetHttpResponse<Event>(HttpMethod.Get, $"{WxTeamsConstants.EventsUrl}/MyEventId", null, null))
                .ReturnsAsync(new HttpResponseMessage { Content = new StringContent(messageContent) });

            var logger = new Mock<ILogger<WxTeamsApi>>();
            var deserializer = new CustomJsonDeserializer();
            var teamsClient = new TeamsClient(deserializer, mockBaseClient.Object);
            var api = new WxTeamsApi(teamsClient, deserializer, logger.Object);

            // Act
            var teamsEvent = await api.GetEventAsync("MyEventId");

            // Assert
            teamsEvent.Id.Should().Be("Y2lzY29zcGFyazovL3VzL0VWRU5UL2JiY2ViMWFkLTQzZjEtM2I1OC05MTQ3LWYxNGJiMGM0ZDE1NAo");
            teamsEvent.ActorId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY");
            teamsEvent.Created.Should().Be(new DateTimeOffset(2016, 05, 16, 21, 34, 59, 324, TimeSpan.Zero));
            teamsEvent.OrgId.Should().Be("OTZhYmMyYWEtM2RjYy0xMWU1LWExNTItZmUzNDgxOWNkYzlh");
            teamsEvent.Type.Should().Be(eventType);
            teamsEvent.EventData.Id.Should().Be("Y2lzY29zcGFyazovL3VzL01FU1NBR0UvOTJkYjNiZTAtNDNiZC0xMWU2LThhZTktZGQ1YjNkZmM1NjVk");
            teamsEvent.EventData.PersonEmail.Should().Be("matt@example.com");
            teamsEvent.EventData.PersonId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY");
            teamsEvent.EventData.RoomId.Should().Be("Y2lzY29zcGFyazovL3VzL1JPT00vYmJjZWIxYWQtNDNmMS0zYjU4LTkxNDctZjE0YmIwYzRkMTU0");
            teamsEvent.EventData.RoomType.Should().Be(roomType);
            teamsEvent.EventData.Text.Should().StartWith("PROJECT UPDATE - A new project");
            teamsEvent.TeamsApi.Should().NotBeNull();
            teamsEvent.TeamsApi.TeamsClient.Should().NotBeNull();
            teamsEvent.Resource.Should().Be(eventResource);
        }
    }
}
