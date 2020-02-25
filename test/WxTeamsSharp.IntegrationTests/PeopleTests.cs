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
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class PeopleTests
    {
        private readonly IWxTeamsApi _wxTeamsApi;

        public PeopleTests()
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
        public async Task ShouldGetMyDetails()
        {
            var me = await _wxTeamsApi.GetMyUserDetails();

            me.Should().NotBeNull();
            me.NickName.Should().Be("TeamsSharpEx");
            me.Emails.Any(x => x == "SparklesTheUnicorn@webex.bot").Should().BeTrue();
            me.Type.Should().Be(PersonType.Bot);
        }

        [Fact]
        public async Task ShouldGetPersonById()
        {
            var person = await _wxTeamsApi.GetUserAsync(StaticTestingValues.JId);

            person.Should().NotBeNull();
            person.Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public async Task ShouldSendMessageToPerson()
        {
            var person = await _wxTeamsApi.GetUserAsync(StaticTestingValues.JId);
            var message = await person.SendMessageAsync("**Woot**");

            message.Should().NotBeNull();
            message.Markdown.Should().Be("**Woot**");
        }

        [Fact]
        public async Task ShouldGetPeopleByEmail()
        {
            var people = await _wxTeamsApi.GetUsersByEmailAsync(StaticTestingValues.JEmail);

            people.Should().NotBeNull();
            people.Items.Count.Should().Be(1);
            people.Items.First().Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public async Task ShouldGetPeopleByDisplayName()
        {
            var people = await _wxTeamsApi.GetUsersByDisplayNameAsync("Joshua Barton");

            people.Should().NotBeNull();
            people.Items.Count.Should().Be(1);
            people.Items.First().Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public async Task ShouldGetPeopleByIdList()
        {
            var people = await _wxTeamsApi.GetUsersByIdListAsync(new string[] { StaticTestingValues.JId });

            people.Should().NotBeNull();
            people.Items.Count.Should().Be(1);
            people.Items.First().Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public void ShouldGetPeopleByOrgId()
        {
            Func<Task> result = async () => await _wxTeamsApi.GetUsersByOrgIdAsync(StaticTestingValues.JOrgId);

            result.Should().Throw<TeamsApiException>("Admin only").WithMessage("Email, displayName, or id list should be specified.");
        }
    }
}
