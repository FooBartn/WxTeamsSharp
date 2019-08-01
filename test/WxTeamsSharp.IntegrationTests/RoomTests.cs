using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Models.Exceptions;
using WxTeamsSharp.Models.Messages;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class RoomTests : IClassFixture<RoomsFixture>
    {
        public RoomTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            var token = configuration.GetSection("BotToken").Value;
            WxTeamsApi.SetAuth(token);
        }

        [Fact]
        public async Task ShouldGetRooms()
        {
            var rooms = await WxTeamsApi.GetRoomsAsync();

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().BeGreaterOrEqualTo(2);
            var room = rooms.Items.SingleOrDefault(x => x.Title.EndsWith("on"));

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.CreatorId.Should().EndWith("I3MzA");
            room.Id.Should().EndWith("xZjg2");
        }

        [Fact]
        public async Task ShouldGetRoomsLimit1()
        {
            var rooms = await WxTeamsApi.GetRoomsAsync(1);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().Be(1);
            var room = rooms.Items.FirstOrDefault();

            room.Should().NotBeNull();
            room.CreatorId.Should().NotBeNull();
            room.Id.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldGetRoomsLimitToGroup()
        {
            var rooms = await WxTeamsApi.GetRoomsAsync(type: RoomType.Group);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().BeGreaterOrEqualTo(1);
            var room = rooms.Items.SingleOrDefault(x => x.Title == "Project Sparkle");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.CreatorId.Should().EndWith("Q0MTY");
            room.Id.Should().EndWith("mNzcw");
        }

        // TODO: When messages have been implemented, use that function to test last activity
        [Theory]
        [InlineData("Y2lzY29zcGFyazovL3VzL1JPT00vNDIxNWU1ZDEtODRlOS0zMjc4LWFmMmYtNjM1NTNiOTcxZjg2", "ton")]
        [InlineData("Y2lzY29zcGFyazovL3VzL1JPT00vOTg4ODAyOTAtYThiOC0xMWU5LWI5YWMtYWZjZDIwMDFjODI0", "Test")]
        public async Task ShouldGetRoomsSortedByLastActivity(string roomId, string roomName)
        {
            var newMessage = MessageBuilder.New()
                .SendToRoom(roomId)
                .WithText("Activity Test")
                .Build();

            await WxTeamsApi.SendMessageAsync(newMessage);
            var rooms = await WxTeamsApi.GetRoomsAsync(sortBy: SortBy.LastActivity);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().BeGreaterOrEqualTo(2);
            var room = rooms.Items.First();

            room.Should().NotBeNull();
            room.Title.Should().EndWith(roomName);
            room.IsLocked.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldGetRoomsSortedById()
        {
            var rooms = await WxTeamsApi.GetRoomsAsync(sortBy: SortBy.Id);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().BeGreaterOrEqualTo(2);
            var room = rooms.Items.First();

            room.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldGetRoomsSortedByCreated()
        {
            var rooms = await WxTeamsApi.GetRoomsAsync(sortBy: SortBy.Created);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().BeGreaterOrEqualTo(2);
            var room = rooms.Items.FirstOrDefault(x => x.Title == "Project Sparkle");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.CreatorId.Should().EndWith("Q0MTY");
            room.Id.Should().EndWith("mNzcw");
        }

        [Fact]
        public async Task ShouldGetRoom()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.CreatorId.Should().EndWith("I3MzA");
            room.Id.Should().EndWith("xZjg2");
        }

        [Fact]
        public async Task ShouldGetRoomMeetingDetailsViaAPI()
        {
            var meetingDetails = await WxTeamsApi.GetMeetingDetailsAsync(StaticTestingValues.JRoom);

            meetingDetails.Should().NotBeNull();
            meetingDetails.MeetingLink.Should().StartWith("https://web.ciscospark.com/meet/");
        }

        [Fact]
        public async Task ShouldGetRoomMeetingDetailsViaRoomObject()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var meetingDetails = await room.GetMeetingDetailsAsync();

            meetingDetails.Should().NotBeNull();
            meetingDetails.MeetingLink.Should().StartWith("https://web.ciscospark.com/meet/");
        }

        [Fact]
        public async Task ShouldCreateAndDeleteRoomViaDirectAPI()
        {
            var room = await WxTeamsApi.CreateRoomAsync("Sparkle Pony Fever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Fever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var response = await WxTeamsApi.DeleteRoomAsync(room.Id);
            response.Message.Should().Be("OK");

            var rooms = await WxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Fever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public async Task ShouldCreateAndDeleteRoomViaObject()
        {
            var room = await WxTeamsApi.CreateRoomAsync("Sparkle Pony Fever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Fever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var response = await room.DeleteAsync();
            response.Message.Should().Be("OK");

            var rooms = await WxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Fever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public async Task ShouldCreateUpdateAndDeleteRoomViaDirectAPI()
        {
            var room = await WxTeamsApi.CreateRoomAsync("Sparkle Pony Forever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Forever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var updatedRoom = await WxTeamsApi.UpdateRoomAsync(room.Id, "Sparkle Pony Squad Forever");
            updatedRoom.Title.Should().Be("Sparkle Pony Squad Forever");

            var response = await WxTeamsApi.DeleteRoomAsync(room.Id);
            response.Message.Should().Be("OK");

            var rooms = await WxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Forever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public async Task ShouldCreateUpdateAndDeleteRoomViaObject()
        {
            var room = await WxTeamsApi.CreateRoomAsync("Sparkle Pony Forever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Forever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var updatedRoom = await room.UpdateAsync("Sparkle Pony Squad Forever");
            updatedRoom.Title.Should().Be("Sparkle Pony Squad Forever");

            var response = await room.DeleteAsync();
            response.Message.Should().Be("OK");

            var rooms = await WxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Forever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public void GetInvalidRoomShouldIncludeErrorResponse()
        {
            Func<Task> act = async () => await WxTeamsApi.GetRoomAsync("1234");

            act.Should().Throw<TeamsApiException>()
                .WithMessage("The requested resource could not be found.");
        }

        [Fact]
        public void DeleteInvalidRoomShouldIncludeErrorResponse()
        {
            Func<Task> act = async () => await WxTeamsApi.DeleteRoomAsync("1234");

            act.Should().Throw<TeamsApiException>()
                .WithMessage("The requested resource could not be found.");
        }

        [Fact]
        public void GetInvalidMeetingRoomShouldIncludeErrorResponse()
        {
            Func<Task> act = async () => await WxTeamsApi.GetMeetingDetailsAsync("1234");

            act.Should().Throw<TeamsApiException>()
                .WithMessage("Could not find a room with provided ID.");
        }
    }
}
