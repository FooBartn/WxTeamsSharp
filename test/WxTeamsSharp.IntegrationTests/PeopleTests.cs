using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Models.Exceptions;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class PeopleTests
    {
        public PeopleTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            var token = configuration.GetSection("BotToken").Value;
            WxTeamsApi.SetAuth(token);
        }

        [Fact]
        public async Task ShouldGetMyDetails()
        {
            var me = await WxTeamsApi.GetMyUserDetails();

            me.Should().NotBeNull();
            me.NickName.Should().Be("TeamsSharpEx");
            me.Emails.Any(x => x == "SparklesTheUnicorn@webex.bot").Should().BeTrue();
            me.Type.Should().Be(PersonType.Bot);
        }

        [Fact]
        public async Task ShouldGetPersonById()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.JId);

            person.Should().NotBeNull();
            person.Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public async Task ShouldSendMessageToPerson()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.JId);
            var message = await person.SendMessageAsync("**Woot**");

            message.Should().NotBeNull();
            message.Markdown.Should().Be("**Woot**");
        }

        [Fact]
        public async Task ShouldGetPeopleByEmail()
        {
            var people = await WxTeamsApi.GetUsersByEmailAsync("jbarton@netsyncnetwork.com");

            people.Should().NotBeNull();
            people.Items.Count.Should().Be(1);
            people.Items.First().Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public async Task ShouldGetPeopleByDisplayName()
        {
            var people = await WxTeamsApi.GetUsersByDisplayNameAsync("Joshua Barton");

            people.Should().NotBeNull();
            people.Items.Count.Should().Be(1);
            people.Items.First().Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public async Task ShouldGetPeopleByIdList()
        {
            var people = await WxTeamsApi.GetUsersByIdListAsync(new string[] { StaticTestingValues.JId });

            people.Should().NotBeNull();
            people.Items.Count.Should().Be(1);
            people.Items.First().Id.Should().Be(StaticTestingValues.JId);
        }

        [Fact]
        public void ShouldGetPeopleByOrgId()
        {
            Func<Task> result = async () => await WxTeamsApi.GetUsersByOrgIdAsync(StaticTestingValues.JOrgId);

            result.Should().Throw<TeamsApiException>("Admin only").WithMessage("Email, displayName, or id list should be specified.");
        }
    }
}
