using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Client;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Client;
using WxTeamsSharp.Models.Events;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Models.Licenses;
using WxTeamsSharp.Models.Memberships;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Organizations;
using WxTeamsSharp.Models.People;
using WxTeamsSharp.Models.ResourceGroups;
using WxTeamsSharp.Models.Rooms;
using Xunit;

namespace WxTeamsSharp.UnitTests
{
    public class WxTeamsApiTests
    {
        [Theory]
        [InlineData("RoomMembership1", true)]
        [InlineData("RoomMembership2", false)]
        public async Task ShouldReturn_Valid_RoomMembership(string file, bool isModerator)
        {
            var content = GetResource(file);
            var api = GetApi<RoomMembership>(HttpMethod.Get, $"{WxTeamsConstants.MembershipsUrl}/MyRoomId", content);

            var roomMembership = await api.GetMembershipAsync("MyRoomId");

            roomMembership.Created.Should().Be(new DateTimeOffset(2015, 10, 18, 14, 26, 16, 203, TimeSpan.Zero));
            roomMembership.Id.Should().Be("Y2lzY29zcGFyazovL3VzL01FTUJFUlNISVAvMGQwYzkxYjYtY2U2MC00NzI1LWI2ZDAtMzQ1NWQ1ZDExZWYzOmNkZTFkZDQwLTJmMGQtMTFlNS1iYTljLTdiNjU1NmQyMjA3Yg");
            roomMembership.IsModerator.Should().Be(isModerator);
            roomMembership.PersonDisplayName.Should().Be("John Andersen");
            roomMembership.PersonEmail.Should().Be("john.andersen@example.com");
            roomMembership.PersonId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY");
            roomMembership.PersonOrgId.Should().Be("Y2lzY29zcGFyazovL3VzL09SR0FOSVpBVElPTi85NmFiYzJhYS0zZGNjLTExZTUtYTE1Mi1mZTM0ODE5Y2RjOWE");
            roomMembership.RoomId.Should().Be("Y2lzY29zcGFyazovL3VzL1JPT00vYmJjZWIxYWQtNDNmMS0zYjU4LTkxNDctZjE0YmIwYzRkMTU0");
        }

        [Fact]
        public async Task ShouldReturn_Valid_RoomMemberships()
        {
            var content = GetResource("RoomMemberships");
            var api = GetApi<RoomMembership>(HttpMethod.Get, $"{WxTeamsConstants.MembershipsUrl}?roomId=MyRoomId", content);

            var itemResults = await api.GetRoomMembershipsAsync("MyRoomId");

            itemResults.Items.Count.Should().Be(2);
            itemResults.TeamsApi.Should().NotBeNull();
            itemResults.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldDelete_Message_AndReturn_OK()
        {
            var content = string.Empty;
            var api = GetApi<ResponseMessage>(HttpMethod.Delete, $"{WxTeamsConstants.MessagesUrl}/MyMessageId", content);

            var response = await api.DeleteMessageAsync("MyMessageId");

            response.Message.Should().Be("OK");
        }

        [Fact]
        public async Task ShouldSend_AndReturn_Message()
        {
            var message = MessageBuilder.New()
                .SendToRoom("0")
                .WithMarkdown("**hi**")
                .Build();

            var content = message.ToJson();
            var api = GetApi<Message>(HttpMethod.Post, WxTeamsConstants.MessagesUrl, content, content);

            var result = await api.SendMessageAsync(message);

            result.Markdown.Should().Be("**hi**");
            result.RoomId.Should().Be("0");
        }

        [Theory]
        [InlineData("Message1", RoomType.Direct)]
        [InlineData("Message2", RoomType.Group)]
        public async Task ShouldReturn_Valid_MessageObject(string fileName, RoomType roomType)
        {
            var content = GetResource(fileName);
            var api = GetApi<Message>(HttpMethod.Get, $"{WxTeamsConstants.MessagesUrl}/MyMessageId", content);

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
        public async Task ShouldReturn_Valid_RoomMessages()
        {
            var content = GetResource("Messages");
            var api = GetApi<Message>(HttpMethod.Get, $"{WxTeamsConstants.MessagesUrl}?roomId=MyRoomId", content);

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

        [Fact]
        public async Task ShouldReturn_Valid_DirectMessages_ByEmail()
        {
            var content = GetResource("Messages");
            var api = GetApi<Message>(HttpMethod.Get, $"{WxTeamsConstants.MessagesUrl}/direct?personEmail=example%40email.com", content);

            var listResult = await api.GetDirectMessagesAsync("example@email.com");

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

        [Fact]
        public async Task ShouldReturn_Valid_DirectMessages_ById()
        {
            var content = GetResource("Messages");
            var api = GetApi<Message>(HttpMethod.Get, $"{WxTeamsConstants.MessagesUrl}/direct?personId=MyId", content);

            var listResult = await api.GetDirectMessagesAsync("MyId");

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
        [InlineData("Event1", EventResource.Messages, EventType.Created, RoomType.Group)]
        [InlineData("Event2", EventResource.Memberships, EventType.Updated, RoomType.Direct)]
        public async Task ShouldReturn_Valid_EventObject(string fileName, EventResource eventResource, EventType eventType, RoomType roomType)
        {
            // Arrange
            var content = GetResource(fileName);
            var api = GetApi<Event>(HttpMethod.Get, $"{WxTeamsConstants.EventsUrl}/MyEventId", content);

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

        [Fact]
        public async Task ShouldReturn_Valid_Events()
        {
            var content = GetResource("Events");
            var api = GetApi<Event>(HttpMethod.Get, $"{WxTeamsConstants.EventsUrl}", content);

            var listResult = await api.GetEventsAsync();
            var teamsEvent = listResult.Items.First();

            listResult.HasNextPage.Should().BeFalse();
            listResult.NotFoundIds.Should().BeNull();
            listResult.TeamsApi.Should().NotBeNull();
            listResult.TeamsApi.TeamsClient.Should().NotBeNull();
            listResult.Items.Count.Should().Be(2);

            teamsEvent.Id.Should().Be("Y2lzY29zcGFyazovL3VzL0VWRU5UL2JiY2ViMWFkLTQzZjEtM2I1OC05MTQ3LWYxNGJiMGM0ZDE1NAo");
            teamsEvent.ActorId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY");
            teamsEvent.EventData.Id.Should().Be("Y2lzY29zcGFyazovL3VzL01FU1NBR0UvOTJkYjNiZTAtNDNiZC0xMWU2LThhZTktZGQ1YjNkZmM1NjVk");
            teamsEvent.EventData.PersonEmail.Should().Be("matt@example.com");
            teamsEvent.EventData.PersonId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY");
            teamsEvent.EventData.RoomId.Should().Be("Y2lzY29zcGFyazovL3VzL1JPT00vYmJjZWIxYWQtNDNmMS0zYjU4LTkxNDctZjE0YmIwYzRkMTU0");
            teamsEvent.EventData.RoomType.Should().Be(RoomType.Group);
            teamsEvent.EventData.Text.Should().StartWith("PROJECT UPDATE - A");

        }

        [Fact]
        public async Task ShouldReturn_Valid_Licenses()
        {
            var content = GetResource("Licenses");
            var api = GetApi<License>(HttpMethod.Get, WxTeamsConstants.LicensesUrl, content);

            var itemsResult = await api.GetLicensesAsync();

            itemsResult.Items.Count.Should().Be(2);
            itemsResult.TeamsApi.Should().NotBeNull();
            itemsResult.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturn_Valid_License()
        {
            var content = GetResource("License");
            var api = GetApi<License>(HttpMethod.Get, $"{WxTeamsConstants.LicensesUrl}/MyLicenseId", content);

            var license = await api.GetLicenseAsync("MyLicenseId");

            license.TeamsApi.Should().NotBeNull();
            license.TeamsApi.TeamsClient.Should().NotBeNull();
            license.ConsumedUnits.Should().Be(8);
            license.Id.Should().Be("Y2lzY29zcGFyazovL3VzL0xJQ0VOU0UvOTZhYmMyYWEtM2RjYy0xMWU1LWExNTItZmUzNDgxOWNkYzlh");
            license.Name.Should().Be("Webex Teams Calling");
            license.TotalUnits.Should().Be(42);

        }

        [Fact]
        public async Task ShouldReturn_Valid_MeetingDetails()
        {
            var content = GetResource("MeetingDetails");
            var api = GetApi<MeetingDetails>(HttpMethod.Get, $"{WxTeamsConstants.RoomsUrl}/MyRoomId/meetingInfo", content);

            var meeting = await api.GetMeetingDetailsAsync("MyRoomId");

            meeting.CallInTollFreeNumber.Should().Be("+1-866-432-9903");
            meeting.CallInTollNumber.Should().Be("+1-408-525-6800");
            meeting.MeetingLink.Should().Be("https://cisco.webex.com/m/37a7d3a8-6563-487f-9577-cd029101c087");
            meeting.MeetingNumber.Should().Be("201632887");
            meeting.RoomId.Should().Be("Y2lzY29zcGFyazovL3VzL1JPT00vYmJjZWIxYWQtNDNmMS0zYjU4LTkxNDctZjE0YmIwYzRkMTU0");
            meeting.SipAddress.Should().Be("201632887@cisco.webex.com");
            meeting.TeamsApi.Should().NotBeNull();
            meeting.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturn_Valid_Organization()
        {
            var content = GetResource("Organization");
            var api = GetApi<Organization>(HttpMethod.Get, $"{WxTeamsConstants.OrganizationsUrl}/MyOrgId", content);

            var org = await api.GetOrganizationAsync("MyOrgId");

            org.Created.Should().Be(new DateTimeOffset(2015, 10, 18, 14, 26, 16, TimeSpan.Zero));
            org.DisplayName.Should().Be("Acme, Inc.");
            org.Id.Should().Be("Y2lzY29zcGFyazovL3VzL09SR0FOSVpBVElPTi85NmFiYzJhYS0zZGNjLTExZTUtYTE1Mi1mZTM0ODE5Y2RjOWE");
            org.TeamsApi.Should().NotBeNull();
            org.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturn_Valid_Organizations()
        {
            var content = GetResource("Organizations");
            var api = GetApi<Organization>(HttpMethod.Get, WxTeamsConstants.OrganizationsUrl, content);

            var orgs = await api.GetOrganizationsAsync();

            orgs.Items.Count.Should().Be(2);
            orgs.TeamsApi.Should().NotBeNull();
            orgs.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldDelete_Person_AndReturn_OK()
        {
            var content = string.Empty;
            var api = GetApi<ResponseMessage>(HttpMethod.Delete, $"{WxTeamsConstants.PeopleUrl}/MyPersonId", content);

            var response = await api.DeleteUserAsync("MyPersonId");

            response.Message.Should().Be("OK");
        }

        [Fact]
        public async Task ShouldUpdate_Person_AndReturn_Person()
        {
            var userId = "Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY";
            var content = GetResource("Person1");
            var GETapi = GetApi<Person>(HttpMethod.Get, $"{WxTeamsConstants.PeopleUrl}/{userId}", content);
            var user = await GETapi.GetUserAsync(userId);

            user.FirstName.Should().Be("John");
            user.LastName.Should().Be("Andersen");
            user.Licenses.Any(x => x == "Y2lzY29zcGFyazovL3VzL0xJQ0VOU0UvOTZhYmMyYWEtM2RjYy0xMWU1LWExNTItZmUzNDgxOWNkYzlh").Should().BeTrue();
            user.Licenses.Any(x => x == "Y2lzY29zcGFyazovL3VzL0xJQ0VOU0UvOTZhYmMyYWEtM2RjYy0xMWU1LWIyNjMtMGY0NTkyYWRlZmFi").Should().BeTrue();
            user.Emails.Any(x => x == "john.andersen@example.com").Should().BeTrue();
    

            var update = PersonUpdater.StartUpdate(user)
                .AddLicense("MyLicense")
                .AddRole("MyRole")
                .UpdateFirstName("Jake")
                .UpdateLastName("Johnson")
                .UpdateEmail("jake@johnson.com")
                .Build();

            var UPDATEapi = GetApi<Person>(HttpMethod.Put, $"{WxTeamsConstants.PeopleUrl}/{userId}", update.ToJson(), update.ToJson());
            var person = await UPDATEapi.UpdateUserAsync(update);

            person.Emails.Any(x => x == "jake@johnson.com").Should().BeTrue();
            person.FirstName.Should().Be("Jake");
            person.LastName.Should().Be("Johnson");
            person.Licenses.Any(x => x == "Y2lzY29zcGFyazovL3VzL0xJQ0VOU0UvOTZhYmMyYWEtM2RjYy0xMWU1LWExNTItZmUzNDgxOWNkYzlh").Should().BeTrue();
            person.Licenses.Any(x => x == "Y2lzY29zcGFyazovL3VzL0xJQ0VOU0UvOTZhYmMyYWEtM2RjYy0xMWU1LWIyNjMtMGY0NTkyYWRlZmFi").Should().BeTrue();
            person.Licenses.Any(x => x == "MyLicense").Should().BeTrue();
            person.Roles.Any(x => x == "Y2lzY29zcGFyazovL3VzL1JPTEUvOTZhYmMyYWEtM2RjYy0xMWU1LWExNTItZmUzNDgxOWNkYzlh").Should().BeTrue();
            person.Roles.Any(x => x == "Y2lzY29zcGFyazovL3VzL1JPTEUvOTZhYmMyYWEtM2RjYy0xMWU1LWIyNjMtMGY0NTkyYWRlZmFi").Should().BeTrue();
            person.Roles.Any(x => x == "MyRole").Should().BeTrue();
        }

        [Theory]
        [InlineData("Person1", false, true, PersonStatus.Inactive, PersonType.Person)]
        [InlineData("Person2", true, false, PersonStatus.Active, PersonType.Bot)]
        public async Task ShouldReturn_Valid_Person(string fileName, bool invitePending, bool loginEnabled,
            PersonStatus status, PersonType type)
        {
            var content = GetResource(fileName);
            var api = GetApi<Person>(HttpMethod.Get, $"{WxTeamsConstants.PeopleUrl}/UserId", content);

            var user = await api.GetUserAsync("UserId");

            user.Avatar.Should().Be("https://1efa7a94ed21783e352-c62266528714497a17239ececf39e9e2.ssl.cf1.rackcdn.com/V1~54c844c89e678e5a7b16a306bc2897b9~wx29yGtlTpilEFlYzqPKag==~1600");
            user.Created.Should().Be(new DateTimeOffset(2015, 10, 18, 14, 26, 16, TimeSpan.Zero));
            user.DisplayName.Should().Be("John Andersen");
            user.Emails.First().Should().Be("john.andersen@example.com");
            user.FirstName.Should().Be("John");
            user.Id.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY");
            user.InvitePending.Should().Be(invitePending);
            user.LastActivity.Should().Be(new DateTimeOffset(2015, 10, 18, 14, 26, 16, 028, TimeSpan.Zero));
            user.LastModified.Should().Be(new DateTimeOffset(2015, 10, 18, 14, 26, 16, TimeSpan.Zero));
            user.LastName.Should().Be("Andersen");
            user.Licenses.Count.Should().Be(2);
            user.Licenses.Any(x => x == "Y2lzY29zcGFyazovL3VzL0xJQ0VOU0UvOTZhYmMyYWEtM2RjYy0xMWU1LWExNTItZmUzNDgxOWNkYzlh").Should().BeTrue();
            user.Licenses.Any(x => x == "Y2lzY29zcGFyazovL3VzL0xJQ0VOU0UvOTZhYmMyYWEtM2RjYy0xMWU1LWIyNjMtMGY0NTkyYWRlZmFi").Should().BeTrue();
            user.LoginEnabled.Should().Be(loginEnabled);
            user.NickName.Should().Be("John");
            user.OrgId.Should().Be("Y2lzY29zcGFyazovL3VzL09SR0FOSVpBVElPTi85NmFiYzJhYS0zZGNjLTExZTUtYTE1Mi1mZTM0ODE5Y2RjOWE");
            user.PhoneNumbers.Any(x => x.Type == "work" && x.Value == "+1 408 526 7209").Should().BeTrue();
            user.Roles.Count.Should().Be(2);
            user.Roles.Any(x => x == "Y2lzY29zcGFyazovL3VzL1JPTEUvOTZhYmMyYWEtM2RjYy0xMWU1LWExNTItZmUzNDgxOWNkYzlh").Should().BeTrue();
            user.Roles.Any(x => x == "Y2lzY29zcGFyazovL3VzL1JPTEUvOTZhYmMyYWEtM2RjYy0xMWU1LWIyNjMtMGY0NTkyYWRlZmFi").Should().BeTrue();
            user.Status.Should().Be(status);
            user.Timezone.Should().Be("America/Denver");
            user.Type.Should().Be(type);
            user.TeamsApi.Should().NotBeNull();
            user.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturn_Valid_People()
        {
            var content = GetResource("People");
            var api = GetApi<Person>(HttpMethod.Get, WxTeamsConstants.PeopleUrl, content);

            var users = await api.GetUsersAsync();
            users.Items.Count.Should().Be(2);
            users.HasNextPage.Should().BeFalse();
            users.TeamsApi.Should().NotBeNull();
            users.TeamsApi.TeamsClient.Should().NotBeNull();

        }

        [Fact]
        public async Task ShouldReturn_Valid_ResourceGroup()
        {
            var content = GetResource("ResourceGroup");
            var api = GetApi<ResourceGroup>(HttpMethod.Get, $"{WxTeamsConstants.ResourceGroupsUrl}/ResourceGroupId", content);

            var resourceGroup = await api.GetResourceGroupAsync("ResourceGroupId");

            resourceGroup.Error.Should().BeNull();
            resourceGroup.HasError.Should().BeFalse();
            resourceGroup.Id.Should().Be("Y2lzY29zcGFyazovL3VzL1JFU09VUkNFX0dST1VQL2RlZmF1bHQ");
            resourceGroup.Name.Should().Be("Resource Group 1");
            resourceGroup.OrgId.Should().Be("Y2lzY29zcGFyazovL3VzL09SR0FOSVpBVElPTi85NmFiYzJhYS0zZGNjLTExZTUtYTE1Mi1mZTM0ODE5Y2RjOWE");
            resourceGroup.TeamsApi.Should().NotBeNull();
            resourceGroup.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldReturn_Valid_ResourceGroups()
        {
            var content = GetResource("ResourceGroupList");
            var api = GetApi<ResourceGroup>(HttpMethod.Get, WxTeamsConstants.ResourceGroupsUrl, content);

            var itemResults = await api.GetResourceGroupsAsync();

            itemResults.Items.Count.Should().Be(1);
            itemResults.Items.First().Name.Should().Be("Resource Group 1");
            itemResults.TeamsApi.Should().NotBeNull();
            itemResults.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Room1", RoomType.Group, false)]
        [InlineData("Room2", RoomType.Direct, true)]
        public async Task ShouldReturn_Valid_Room(string fileName, RoomType roomType, bool isLocked)
        {
            var content = GetResource(fileName);
            var api = GetApi<Room>(HttpMethod.Get, $"{WxTeamsConstants.RoomsUrl}/RoomId", content);

            var room = await api.GetRoomAsync("RoomId");

            room.CreatorId.Should().Be("Z2lZY29ZcGFyaZovL3VZL1JPT11vNGQZNjA5ZjktMDg5Zi0ZYWM1LWI0MjMtNDc1Njg5NjUZYZRk");
            room.HasError.Should().BeFalse();
            room.Id.Should().Be("Z2lZY29ZcGFyaZovL3VZL1JPT11vNGQZNjA5ZjktMDg5Zi0ZYWM1LWI0MjMtNDc1Njg5NjUZYZRk");
            room.IsLocked.Should().Be(isLocked);
            room.SipAddress.Should().BeNull();
            room.TeamId.Should().BeEmpty();
            room.Title.Should().Be("Something Something Darkside");
            room.Type.Should().Be(roomType);
            room.TeamsApi.Should().NotBeNull();
            room.TeamsApi.TeamsClient.Should().NotBeNull();
        }

        private WxTeamsApi GetApi<TEntity>(HttpMethod method, string endpoint, string content, string json = null, MultipartFormDataContent formData = null)
        {
            var mockBaseClient = new Mock<IWxBaseClient>();
            mockBaseClient.Setup(x => x.GetHttpResponse<TEntity>(method, endpoint, json, formData))
                .ReturnsAsync(new HttpResponseMessage { Content = new StringContent(content) });

            var logger = new Mock<ILogger<WxTeamsApi>>();
            var deserializer = new CustomJsonDeserializer();
            var teamsClient = new TeamsClient(deserializer, mockBaseClient.Object);
            return new WxTeamsApi(teamsClient, deserializer, logger.Object);
        }


        private string GetResource(string filename)
            => File.ReadAllText($"Resources/{filename}.json");
    }
}
