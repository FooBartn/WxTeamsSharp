using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Extensions;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Models.Exceptions;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class TeamsTests : IDisposable
    {
        private readonly IWxTeamsApi _wxTeamsApi;

        public TeamsTests()
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
        public async Task ShouldGetTeams()
        {
            var teams = await _wxTeamsApi.GetTeamsAsync();

            teams.Should().NotBeNull();
            teams.Items.Count.Should().BeGreaterOrEqualTo(1);
            teams.Items.Any(x => x.Id == StaticTestingValues.WobblyTeam).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetTeam()
        {
            var team = await _wxTeamsApi.GetTeamAsync(StaticTestingValues.WobblyTeam);

            team.Should().NotBeNull();
            team.Name.Should().Be("Wobbly Weasels");
        }

        [Fact]
        public async Task ShouldGetTeamMemberships()
        {
            var team = await _wxTeamsApi.GetTeamAsync(StaticTestingValues.WobblyTeam);
            var members = await _wxTeamsApi.GetTeamMembershipsAsync(team.Id);

            team.Should().NotBeNull();
            team.Name.Should().Be("Wobbly Weasels");
            members.Should().NotBeNull();
            members.Items.Any(x => x.PersonEmail == "SparklesTheUnicorn@webex.bot").Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetTeamMembershipsViaObject()
        {
            var team = await _wxTeamsApi.GetTeamAsync(StaticTestingValues.WobblyTeam);
            var members = await team.GetMembershipsAsync();

            team.Should().NotBeNull();
            team.Name.Should().Be("Wobbly Weasels");
            members.Should().NotBeNull();
            members.Items.Any(x => x.PersonEmail == "SparklesTheUnicorn@webex.bot").Should().BeTrue();
        }

        [Fact]
        public async Task ShouldCreate_Update_And_DeleteTeam()
        {
            var name = "Test Team1";
            var updatedName = "Super Test Team";

            var team = await _wxTeamsApi.CreateTeamAsync(name);
            team.Should().NotBeNull();
            team.Name.Should().Be(name);

            var updated = await _wxTeamsApi.UpdateTeamAsync(team.Id, updatedName);
            updated.Name.Should().Be(updatedName);

            var deleted = await _wxTeamsApi.DeleteTeamAsync(team.Id);
            deleted.Message.Should().Be("OK");

            Func<Task> getTeam = async () => await _wxTeamsApi.GetTeamAsync(team.Id);
            await getTeam.Should().ThrowAsync<TeamsApiException>()
                .WithMessage("Could not find teams.");
        }

        [Fact]
        public async Task ShouldCreateTeam_AddMember_DeleteMember_AndDeleteTeam()
        {
            var name = "Test Team2";
            var team = await _wxTeamsApi.CreateTeamAsync(name);
            team.Should().NotBeNull();
            team.Name.Should().Be(name);

            var member = await _wxTeamsApi.AddUserToTeamAsync(team.Id, StaticTestingValues.JEmail, true);
            member.IsModerator.Should().BeTrue();
            member.PersonEmail.Should().Be(StaticTestingValues.JEmail);

            var deletedMember = await _wxTeamsApi.RemoveUserFromTeamAsync(member.Id);
            deletedMember.Message.Should().Be("OK");

            var deleted = await _wxTeamsApi.DeleteTeamAsync(team.Id);
            deleted.Message.Should().Be("OK");

            Func<Task> getTeam = async () => await _wxTeamsApi.GetTeamAsync(team.Id);
            await getTeam.Should().ThrowAsync<TeamsApiException>()
                .WithMessage("Could not find teams.");
        }

        [Fact]
        public async Task ShouldCreateTeam_AddMember_DeleteMember_AndDeleteTeam_ViaObject()
        {
            var name = "Test Team3";
            var team = await _wxTeamsApi.CreateTeamAsync(name);
            team.Should().NotBeNull();
            team.Name.Should().Be(name);

            var member = await team.AddUserAsync(StaticTestingValues.JId, true);
            member.IsModerator.Should().BeTrue();
            member.PersonEmail.Should().Be(StaticTestingValues.JEmail);

            var deletedMember = await team.RemoveUserAsync(member.PersonId);
            deletedMember.Message.Should().Be("OK");

            var deleted = await team.DeleteAsync();
            deleted.Message.Should().Be("OK");

            Func<Task> getTeam = async () => await _wxTeamsApi.GetTeamAsync(team.Id);
            await getTeam.Should().ThrowAsync<TeamsApiException>()
                .WithMessage("Could not find teams.");
        }

        public void Dispose()
        {
            var teams = _wxTeamsApi.GetTeamsAsync().GetAwaiter().GetResult();
            var testTeams = teams.Items.Where(x => x.Name.Contains("Test Team"));

            foreach (var testTeam in testTeams)
            {
                testTeam.DeleteAsync().GetAwaiter().GetResult();
            }
        }
    }
}
