using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Models.People;
using Xunit;

namespace WxTeamsSharp.IntegrationTests
{
    public class AdminTests
    {
        public AdminTests()
        {
            // This is a 12 hour token. It becomes useless and has to be replaced after that time. So need to possibly move these tests
            // to a separate project and not include it outside of local testing.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            var token = configuration.GetSection("AdminBotToken").Value;
            WxTeamsApi.SetAuth(token);
        }

        [Fact]
        public async Task ShouldGetRoles()
        {
            var roles = await WxTeamsApi.GetRolesAsync();

            roles.Should().NotBeNull();
            roles.Items.Count.Should().BeGreaterOrEqualTo(5);
            roles.Items.Any(x => x.Id == StaticTestingValues.UserAdminRole).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetRole()
        {
            var roles = await WxTeamsApi.GetRoleAsync(StaticTestingValues.UserAdminRole);

            roles.Name.Should().Be("User Administrator");
        }

        [Fact]
        public async Task ShouldGetLicenses()
        {
            var licenses = await WxTeamsApi.GetLicensesAsync();

            licenses.Should().NotBeNull();
            licenses.Items.Count.Should().BeGreaterOrEqualTo(5);
            licenses.Items.Any(x => x.Id == StaticTestingValues.MessagingLicense).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetLicense()
        {
            var license = await WxTeamsApi.GetLicenseAsync(StaticTestingValues.MessagingLicense);

            license.Name.Should().Be("Messaging");
        }

        [Fact]
        public async Task ShouldGetOrganizations()
        {
            var organizations = await WxTeamsApi.GetOrganizationsAsync();

            organizations.Should().NotBeNull();
            organizations.Items.Count.Should().Be(1);
            organizations.Items.First().DisplayName.Should().Be("wxsandbox");
        }

        [Fact]
        public async Task ShouldGetOrganization()
        {
            var organization = await WxTeamsApi.GetOrganizationAsync(StaticTestingValues.OrganizationId);

            organization.DisplayName.Should().Be("wxsandbox");
        }

        [Fact]
        public async Task ShouldCreate_DeletePerson()
        {
            var person = await PersonBuilder.New()
                .WithEmail("Wheee@fakeaol.com")
                .WithDisplayName("TestUser")
                .Build()
                .CreateAsync();

            person.Should().NotBeNull();
            person.Emails.First().Should().Be("Wheee@fakeaol.com");
            person.NickName.Should().Be("TestUser");
            person.FirstName.Should().BeNullOrEmpty();
            person.LastName.Should().BeNullOrEmpty();

            var deleted = await person.DeleteAsync();
            deleted.Message.Should().Be("OK");
        }

        [Fact]
        public async Task ShouldUpdateFirstName()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.SpaceMonkeyId);
            person.FirstName.Should().Be("Nothing");

            var firstNameUpdated = await person.UpdateFirstNameAsync("Johnny");
            firstNameUpdated.FirstName.Should().Be("Johnny");

            await person.UpdateFirstNameAsync("Nothing");
        }

        [Fact]
        public async Task ShouldUpdateLastName()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.SpaceMonkeyId);

            person.LastName.Should().Be("Nothing");

            var firstNameUpdated = await person.UpdateLastNameAsync("Utah");
            firstNameUpdated.LastName.Should().Be("Utah");

            await person.UpdateLastNameAsync("Nothing");
        }

        [Fact]
        public async Task ShouldUpdateDisplayName()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.SpaceMonkeyId);

            person.Should().NotBeNull();
            person.DisplayName.Should().Be("NoDisplay");

            var displayNameUpdated = await person.UpdateDisplayNameAsync("Johnny Utah");
            displayNameUpdated.DisplayName.Should().Be("Johnny Utah");

            await person.UpdateDisplayNameAsync("NoDisplay");
        }

        [Fact]
        public async Task ShouldUpdateEmailAddress()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.SpaceMonkeyId);
            person.Emails.Any(x => x == "testingspacemonkey@gmail.com").Should().BeTrue();

            var updatedEmail = await person.UpdateEmailAsync("johnnyutah@nogmail.com");
            updatedEmail.Emails.Any(x => x == "johnnyutah@nogmail.com").Should().BeTrue();

            await person.UpdateEmailAsync("testingspacemonkey@gmail.com");
        }

        [Fact]
        public async Task ShouldUpdateAndReplaceRoles()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.SpaceMonkeyId);
            person.Roles.Any(x => x == StaticTestingValues.HelpDeskAdminId).Should().BeFalse();

            var roleAdded = await person.AddRoleAsync(StaticTestingValues.HelpDeskAdminId);
            roleAdded.Roles.Any(x => x == StaticTestingValues.HelpDeskAdminId).Should().BeTrue();

            var replacedRoles = await person.ReplaceRolesAsync(new List<string>());
            replacedRoles.Roles.Any().Should().BeFalse();
        }

        [Fact]
        public async Task ShouldUpdateAndReplaceLicenses()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.SpaceMonkeyId);
            person.Licenses.Any(x => x == StaticTestingValues.MessagingLicense).Should().BeFalse();

            var licenseAdded = await person.AddLicenseAsync(StaticTestingValues.MessagingLicense);
            licenseAdded.Licenses.Any(x => x == StaticTestingValues.MessagingLicense).Should().BeTrue();

            var replacedLicenses = await person.ReplaceLicensesAsync(new List<string>());
            replacedLicenses.Licenses.Any().Should().BeFalse();
        }

        [Fact]
        public async Task ShouldUpdateAvatar()
        {
            var person = await WxTeamsApi.GetUserAsync(StaticTestingValues.SpaceMonkeyId);

            var updatedPerson = await person.UpdateAvatarAsync("http://www.sandybeachinternational.com/wp-content/uploads/2018/11/cropped-beach-exotic-holiday-248797.jpg");
            updatedPerson.Avatar.Should().NotBeNullOrEmpty();

            var resetPerson = await person.UpdateAvatarAsync("https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/tyuleniy_oli_2016097_lrg_a.jpg");
            resetPerson.Avatar.Should().NotBe(updatedPerson.Avatar);
        }

        [Fact]
        public async Task ShouldGetResourceGroups()
        {
            var resourceGroups = await WxTeamsApi.GetResourceGroupsAsync();

            resourceGroups.Should().NotBeNull();
            resourceGroups.Items.Count.Should().Be(1);
        }

        [Fact]
        public async Task ShouldGetResourceGroup()
        {
            var resourceGroup = await WxTeamsApi.GetResourceGroupAsync(StaticTestingValues.DefaultResourceGroup);

            resourceGroup.Id.Should().Be(StaticTestingValues.DefaultResourceGroup);
        }

        [Fact]
        public async Task ShouldGetResourceGroupMemberships()
        {
            var memberships = await WxTeamsApi.GetResourceGroupMembershipsAsync();

            memberships.Should().NotBeNull();
            memberships.Items.Count.Should().Be(0);
        }
    }
}
