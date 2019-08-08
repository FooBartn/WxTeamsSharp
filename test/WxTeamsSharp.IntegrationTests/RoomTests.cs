using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Extensions;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Models.Exceptions;
using WxTeamsSharp.Models.Messages;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class RoomTests : IDisposable
    {
        private readonly IWxTeamsApi _wxTeamsApi;

        public RoomTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            services.AddWxTeamsSharp();
            var provider = services.BuildServiceProvider();

            _wxTeamsApi = provider.GetRequiredService<IWxTeamsApi>();

            var token = configuration.GetSection("BotToken").Value;
            _wxTeamsApi.Initialize(token);
        }

        [Fact]
        public async Task ShouldGetRooms()
        {
            var rooms = await _wxTeamsApi.GetRoomsAsync();

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
            var rooms = await _wxTeamsApi.GetRoomsAsync(1);

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
            var rooms = await _wxTeamsApi.GetRoomsAsync(type: RoomType.Group);

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

            await _wxTeamsApi.SendMessageAsync(newMessage);
            var rooms = await _wxTeamsApi.GetRoomsAsync(sortBy: SortBy.LastActivity);

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
            var rooms = await _wxTeamsApi.GetRoomsAsync(sortBy: SortBy.Id);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().BeGreaterOrEqualTo(2);
            var room = rooms.Items.First();

            room.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldGetRoomsSortedByCreated()
        {
            var rooms = await _wxTeamsApi.GetRoomsAsync(sortBy: SortBy.Created);

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
            var room = await _wxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.CreatorId.Should().EndWith("I3MzA");
            room.Id.Should().EndWith("xZjg2");
        }

        [Fact]
        public async Task ShouldGetRoomMeetingDetailsViaAPI()
        {
            var meetingDetails = await _wxTeamsApi.GetMeetingDetailsAsync(StaticTestingValues.JRoom);

            meetingDetails.Should().NotBeNull();
            meetingDetails.MeetingLink.Should().StartWith("https://web.ciscospark.com/meet/");
        }

        [Fact]
        public async Task ShouldGetRoomMeetingDetailsViaRoomObject()
        {
            var room = await _wxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var meetingDetails = await room.GetMeetingDetailsAsync();

            meetingDetails.Should().NotBeNull();
            meetingDetails.MeetingLink.Should().StartWith("https://web.ciscospark.com/meet/");
        }

        [Fact]
        public async Task ShouldCreateAndDeleteRoomViaDirectAPI()
        {
            var room = await _wxTeamsApi.CreateRoomAsync("Sparkle Pony Fever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Fever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var response = await _wxTeamsApi.DeleteRoomAsync(room.Id);
            response.Message.Should().Be("OK");

            var rooms = await _wxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Fever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public async Task ShouldCreateAndDeleteRoomViaObject()
        {
            var room = await _wxTeamsApi.CreateRoomAsync("Sparkle Pony Fever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Fever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var response = await room.DeleteAsync();
            response.Message.Should().Be("OK");

            var rooms = await _wxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Fever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public async Task ShouldCreateUpdateAndDeleteRoomViaDirectAPI()
        {
            var room = await _wxTeamsApi.CreateRoomAsync("Sparkle Pony Forever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Forever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var updatedRoom = await _wxTeamsApi.UpdateRoomAsync(room.Id, "Sparkle Pony Squad Forever");
            updatedRoom.Title.Should().Be("Sparkle Pony Squad Forever");

            var response = await _wxTeamsApi.DeleteRoomAsync(room.Id);
            response.Message.Should().Be("OK");

            var rooms = await _wxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Forever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public async Task ShouldCreateUpdateAndDeleteRoomViaObject()
        {
            var room = await _wxTeamsApi.CreateRoomAsync("Sparkle Pony Forever");

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Type.Should().Be(RoomType.Group);
            room.Title.Should().Be("Sparkle Pony Forever");
            room.CreatorId.Should().EndWith("Q0MTY");

            var updatedRoom = await room.UpdateAsync("Sparkle Pony Squad Forever");
            updatedRoom.Title.Should().Be("Sparkle Pony Squad Forever");

            var response = await room.DeleteAsync();
            response.Message.Should().Be("OK");

            var rooms = await _wxTeamsApi.GetRoomsAsync();
            var deletedRoom = rooms.Items.SingleOrDefault(x => x.Title == "Sparkle Pony Forever");

            deletedRoom.Should().BeNull();
        }

        [Fact]
        public void GetInvalidRoomShouldIncludeErrorResponse()
        {
            Func<Task> act = async () => await _wxTeamsApi.GetRoomAsync("1234");

            act.Should().Throw<TeamsApiException>()
                .WithMessage("The requested resource could not be found.");
        }

        [Fact]
        public void DeleteInvalidRoomShouldIncludeErrorResponse()
        {
            Func<Task> act = async () => await _wxTeamsApi.DeleteRoomAsync("1234");

            act.Should().Throw<TeamsApiException>()
                .WithMessage("The requested resource could not be found.");
        }

        [Fact]
        public void GetInvalidMeetingRoomShouldIncludeErrorResponse()
        {
            Func<Task> act = async () => await _wxTeamsApi.GetMeetingDetailsAsync("1234");

            act.Should().Throw<TeamsApiException>()
                .WithMessage("Could not find a room with provided ID.");
        }

        public void Dispose()
        {
            var rooms = _wxTeamsApi.GetRoomsAsync().GetAwaiter().GetResult();
            var testRooms = rooms.Items.Where(x => x.Title.Contains("Awesomesauce") || x.Title.Contains("WooHoo"));

            foreach (var testRoom in testRooms)
            {
                testRoom.DeleteAsync().GetAwaiter().GetResult();
            }
        }

        [Theory]
        [InlineData("WooHoo1", true)]
        [InlineData("WooHoo2", false)]
        public async Task ShouldCreateRoomAddRemoveUpdateMemberDeleteRoomViaAPI(string roomName, bool isModerator)
        {
            var room = await _wxTeamsApi.CreateRoomAsync(roomName);
            var membership = await _wxTeamsApi.AddUserToRoomAsync(room.Id, StaticTestingValues.JId, isModerator);

            membership.Should().NotBeNull();
            membership.PersonEmail.Should().Be(StaticTestingValues.JEmail);
            membership.IsModerator.Should().Be(isModerator);

            if (!isModerator)
            {
                var updatedMembership = await _wxTeamsApi.UpdateMembershipAsync(membership.Id, true);
                updatedMembership.PersonEmail.Should().Be(StaticTestingValues.JEmail);
                updatedMembership.IsModerator.Should().BeTrue();
            }

            var removedUser = await _wxTeamsApi.DeleteMembershipAsync(membership.Id);
            removedUser.Message.Should().Be("OK");

            var deletedRoom = await room.DeleteAsync();
            deletedRoom.Message.Should().Be("OK");
        }

        [Theory]
        [InlineData("Awesomesauce1", true)]
        [InlineData("Awesomesauce2", false)]
        public async Task ShouldCreateRoomAddRemoveMemberDeleteRoomViaObject(string roomName, bool isModerator)
        {
            var room = await _wxTeamsApi.CreateRoomAsync(roomName);
            var membership = await room.AddUserAsync(StaticTestingValues.JId, isModerator);

            membership.Should().NotBeNull();
            membership.PersonEmail.Should().Be(StaticTestingValues.JEmail);
            membership.IsModerator.Should().Be(isModerator);

            if (!isModerator)
            {
                var updatedMembership = await room.UpdateUserAsync(membership.PersonEmail, true);
                updatedMembership.PersonEmail.Should().Be(StaticTestingValues.JEmail);
                updatedMembership.IsModerator.Should().BeTrue();
            }

            var removedUser = await room.RemoveUserAsync(membership.PersonEmail);
            removedUser.Message.Should().Be("OK");

            var deletedRoom = await room.DeleteAsync();
            deletedRoom.Message.Should().Be("OK");
        }
    }
}
