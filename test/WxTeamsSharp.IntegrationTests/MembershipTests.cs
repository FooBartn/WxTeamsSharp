using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class MembershipTests : IClassFixture<RoomsFixture>
    {
        public MembershipTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            var token = configuration.GetSection("BotToken").Value;
            WxTeamsApi.SetAuth(token);
        }

        [Theory]
        [InlineData("WooHoo1", true)]
        [InlineData("WooHoo2", false)]
        public async Task ShouldCreateRoomAddRemoveUpdateMemberDeleteRoomViaAPI(string roomName, bool isModerator)
        {
            var room = await WxTeamsApi.CreateRoomAsync(roomName);
            var membership = await WxTeamsApi.AddUserToRoomAsync(room.Id, StaticTestingValues.JId, isModerator);

            membership.Should().NotBeNull();
            membership.PersonEmail.Should().Be(StaticTestingValues.JEmail);
            membership.IsModerator.Should().Be(isModerator);

            if (!isModerator)
            {
                var updatedMembership = await WxTeamsApi.UpdateMembershipAsync(membership.Id, true);
                updatedMembership.PersonEmail.Should().Be(StaticTestingValues.JEmail);
                updatedMembership.IsModerator.Should().BeTrue();
            }

            var removedUser = await WxTeamsApi.DeleteMembershipAsync(membership.Id);
            removedUser.Message.Should().Be("OK");

            var deletedRoom = await room.DeleteAsync();
            deletedRoom.Message.Should().Be("OK");
        }

        [Theory]
        [InlineData("Awesomesauce1", true)]
        [InlineData("Awesomesauce2", false)]
        public async Task ShouldCreateRoomAddRemoveMemberDeleteRoomViaObject(string roomName, bool isModerator)
        {
            var room = await WxTeamsApi.CreateRoomAsync(roomName);
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

        [Fact]
        public async Task ShouldGetAllMyMemberships()
        {
            var memberships = await WxTeamsApi.GetAllMembershipsAsync();

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Count.Should().BeGreaterThan(3);
        }

        [Fact]
        public async Task ShouldGetAllMyMembershipsLimit3()
        {
            var memberships = await WxTeamsApi.GetAllMembershipsAsync(3);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Count.Should().Be(3);
        }

        [Fact]
        public async Task ShouldGetMembershipById()
        {
            var membership = await WxTeamsApi.GetMembershipAsync(StaticTestingValues.JRoomMembership);

            membership.Should().NotBeNull();
            membership.RoomId.Should().Be(StaticTestingValues.JRoom);
        }

        [Fact]
        public async Task ShouldGetMembershipsByUserId()
        {
            var memberships = await WxTeamsApi.GetMembershipsAssociatedWithAsync(StaticTestingValues.BotId);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetMembershipsByUserEmail()
        {
            var memberships = await WxTeamsApi.GetMembershipsAssociatedWithAsync(StaticTestingValues.BotEmail);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetMembershipsByRoomId()
        {
            var memberships = await WxTeamsApi.GetRoomMembershipsAsync(StaticTestingValues.JRoom);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Any(x => x.PersonEmail == StaticTestingValues.JEmail).Should().BeTrue();
            memberships.Items.Count.Should().Be(2);
        }

        [Fact]
        public async Task ShouldGetRoomMemberships()
        {
            var room = await WxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var memberships = await room.GetMembershipsAsync();

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Any(x => x.PersonEmail == StaticTestingValues.JEmail).Should().BeTrue();
            memberships.Items.Count.Should().Be(2);
        }
    }
}
