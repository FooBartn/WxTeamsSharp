using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Extensions;
using WxTeamsSharp.Interfaces.Api;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class MembershipTests
    {
        private readonly IWxTeamsApi _wxTeamsApi;

        public MembershipTests()
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
        public async Task ShouldGetAllMyMemberships()
        {
            var memberships = await _wxTeamsApi.GetAllMembershipsAsync();

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Count.Should().BeGreaterThan(3);
        }

        [Fact]
        public async Task ShouldGetAllMyMembershipsLimit3()
        {
            var memberships = await _wxTeamsApi.GetAllMembershipsAsync(3);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Count.Should().Be(3);
        }

        [Fact]
        public async Task ShouldGetMembershipById()
        {
            var membership = await _wxTeamsApi.GetMembershipAsync(StaticTestingValues.JRoomMembership);

            membership.Should().NotBeNull();
            membership.RoomId.Should().Be(StaticTestingValues.JRoom);
        }

        [Fact]
        public async Task ShouldGetMembershipsByUserId()
        {
            var memberships = await _wxTeamsApi.GetMembershipsAssociatedWithAsync(StaticTestingValues.BotId);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetMembershipsByUserEmail()
        {
            var memberships = await _wxTeamsApi.GetMembershipsAssociatedWithAsync(StaticTestingValues.BotEmail);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetMembershipsByRoomId()
        {
            var memberships = await _wxTeamsApi.GetRoomMembershipsAsync(StaticTestingValues.JRoom);

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Any(x => x.PersonEmail == StaticTestingValues.JEmail).Should().BeTrue();
            memberships.Items.Count.Should().Be(2);
        }

        [Fact]
        public async Task ShouldGetRoomMemberships()
        {
            var room = await _wxTeamsApi.GetRoomAsync(StaticTestingValues.JRoom);
            var memberships = await room.GetMembershipsAsync();

            memberships.Should().NotBeNull();
            memberships.Items.Any(x => x.RoomId == StaticTestingValues.JRoom).Should().BeTrue();
            memberships.Items.Any(x => x.PersonEmail == StaticTestingValues.JEmail).Should().BeTrue();
            memberships.Items.Count.Should().Be(2);
        }
    }
}
