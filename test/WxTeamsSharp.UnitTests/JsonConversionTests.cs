using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Helpers;
using WxTeamsSharp.Interfaces.Rooms;
using WxTeamsSharp.Models.Events;
using WxTeamsSharp.Models.General;
using WxTeamsSharp.Models.Licenses;
using WxTeamsSharp.Models.Memberships;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Organizations;
using WxTeamsSharp.Models.People;
using WxTeamsSharp.Models.ResourceGroups;
using WxTeamsSharp.Models.Rooms;
using WxTeamsSharp.Models.Webhooks;
using Xunit;

namespace WxTeamsSharp.UnitTests
{
    public class JsonConversionTests
    {
        [Fact]
        public void ShouldReturnValidResourceGroupList()
        {
            var json = File.ReadAllText("Resources/ResourceGroupList.json");
            var list = JsonSettings.FromJson<ItemsResult<ResourceGroup>>(json);

            list.Should().NotBeNull();
            list.Items.Count.Should().Be(1);
            list.Items.First().Name.Should().Be("Resource Group 1");
        }

        [Fact]
        public void ShouldReturnValidResourceGroup()
        {
            var json = File.ReadAllText("Resources/ResourceGroup.json");
            var resourceGroup = JsonSettings.FromJson<ResourceGroup>(json);

            resourceGroup.Should().NotBeNull();
            resourceGroup.Name.Should().Be("Resource Group 1");
        }

        [Theory]
        [InlineData("Resources/Room1.json", RoomType.Group)]
        [InlineData("Resources/Room2.json", RoomType.Direct)]
        public void ShouldReturnValidRoom(string file, RoomType roomType)
        {
            var json = File.ReadAllText(file);
            IRoom room = JsonSettings.FromJson<Room>(json);

            room.Should().NotBeNull();
            room.IsLocked.Should().BeFalse();
            room.Title.Should().Be("Something Something Darkside");
            room.Id.Should().Be("Z2lZY29ZcGFyaZovL3VZL1JPT11vNGQZNjA5ZjktMDg5Zi0ZYWM1LWI0MjMtNDc1Njg5NjUZYZRk");
            room.CreatorId.Should().Be("Z2lZY29ZcGFyaZovL3VZL1JPT11vNGQZNjA5ZjktMDg5Zi0ZYWM1LWI0MjMtNDc1Njg5NjUZYZRk");
            room.TeamId.Should().BeEmpty();
            room.Type.Should().Be(roomType);
            room.LastActivity.Should().Be(new DateTimeOffset(2019, 6, 26, 20, 37, 34, 272, new TimeSpan(0, 0, 0)));
        }

        [Fact]
        public void ShouldReturnValidMeetingDetails()
        {
            var json = File.ReadAllText("Resources/MeetingDetails.json");
            IMeetingDetails meeting = JsonSettings.FromJson<MeetingDetails>(json);

            meeting.Should().NotBeNull();
            meeting.MeetingLink.Should().Be("https://cisco.webex.com/m/37a7d3a8-6563-487f-9577-cd029101c087");
            meeting.MeetingNumber.Should().Be("201632887");
            meeting.SipAddress.Should().Be("201632887@cisco.webex.com");
            meeting.RoomId.Should().Be("Y2lzY29zcGFyazovL3VzL1JPT00vYmJjZWIxYWQtNDNmMS0zYjU4LTkxNDctZjE0YmIwYzRkMTU0");
        }

        [Fact]
        public void ShouldReturnValidListResultRoomsWithError()
        {
            var json = File.ReadAllText("Resources/ItemResultErrors.json");
            var rooms = JsonSettings.FromJson<ItemsResult<Room>>(json);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().Be(2);
            rooms.Items.First().Title.Should().Be("Project Unicorn - Sprint 0");
            rooms.Items.First().Error.Should().BeNull();
            rooms.Items.Last().Error.Should().NotBeNull();
            rooms.Items.Last().Error.Title.Code.Should().Be("kms_failure");
        }

        [Fact]
        public void ShouldReturnValidListResultRooms()
        {
            var json = File.ReadAllText("Resources/Rooms.json");
            var rooms = JsonSettings.FromJson<ItemsResult<Room>>(json);

            rooms.Should().NotBeNull();
            rooms.Items.Count.Should().Be(1);
            rooms.Items.First().Title.Should().Be("Project Unicorn - Sprint 0");
        }

        [Fact]
        public void ShouldReturnValidEvents()
        {
            var json = File.ReadAllText("Resources/Events.json");
            var events = JsonSettings.FromJson<ItemsResult<Event>>(json);
            events.Should().NotBeNull();
            events.Items.Count.Should().Be(1);
        }

        [Fact]
        public void ShouldReturnValidPeople()
        {
            var json = File.ReadAllText("Resources/People.json");
            var people = JsonSettings.FromJson<ItemsResult<Person>>(json);
            people.Should().NotBeNull();
            people.Items.Count.Should().Be(1);

            var person = people.Items.First();
            person.FirstName.Should().Be("John");
            person.LastName.Should().Be("Andersen");
            person.LoginEnabled.Should().BeTrue();
            person.PhoneNumbers.Count.Should().Be(1);
            person.PhoneNumbers.First().Type.Should().Be("work");
            person.PhoneNumbers.First().Value.Should().Be("+1 408 526 7209");
            person.Status.Should().Be(PersonStatus.Inactive);
        }

        [Fact]
        public void ShouldReturnValidMessages()
        {
            var json = File.ReadAllText("Resources/Messages.json");
            var messages = JsonSettings.FromJson<ItemsResult<Message>>(json);
            messages.Should().NotBeNull();
            messages.Items.First().Text.Should().NotBeNull();
            messages.Items.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("Resources/Message1.json", RoomType.Direct)]
        public void ShouldReturnValidMessage(string file, RoomType roomType)
        {
            var json = File.ReadAllText(file);
            var message = JsonSettings.FromJson<Message>(json);
            message.Should().NotBeNull();
            message.RoomType.Should().Be(roomType);
            message.Text.Should().Be("piece");
        }

        [Theory]
        [InlineData("Resources/Event1.json", EventResource.Messages, EventType.Created)]
        [InlineData("Resources/Event2.json", EventResource.Memberships, EventType.Updated)]
        public void ShouldReturnValidEvent(string file, EventResource resourceType, EventType eventType)
        {
            var json = File.ReadAllText(file);
            var singleEvent = JsonSettings.FromJson<Event>(json);
            singleEvent.ActorId.Should().Be("Y2lzY29zcGFyazovL3VzL1BFT1BMRS9mNWIzNjE4Ny1jOGRkLTQ3MjctOGIyZi1mOWM0NDdmMjkwNDY");
            singleEvent.AppId.Should().Be("null");
            singleEvent.Resource.Should().Be(resourceType);
            singleEvent.Type.Should().Be(eventType);
        }

        [Theory]
        [InlineData("Resources/Event1.json", RoomType.Group)]
        [InlineData("Resources/Event2.json", RoomType.Direct)]
        public void ShouldReturnValidEventData(string file, RoomType roomType)
        {
            var json = File.ReadAllText(file);
            var singleEvent = JsonSettings.FromJson<Event>(json);
            var eventData = singleEvent.EventData;

            eventData.Should().NotBeNull();
            eventData.PersonEmail.Should().Be("matt@example.com");
            eventData.RoomType.Should().Be(roomType);
            eventData.Text.Should().StartWith("PROJECT UPDATE - A new project plan has been published");
        }

        [Fact]
        public void ShouldReturnValidLicense()
        {
            var json = File.ReadAllText("Resources/License.json");
            var license = JsonSettings.FromJson<License>(json);

            license.ConsumedUnits.Should().Be(8);
            license.TotalUnits.Should().Be(42);
            license.Name.Should().Be("Webex Teams Calling");
        }

        [Fact]
        public void ShouldReturnValidRoomMembership()
        {
            var json = File.ReadAllText("Resources/RoomMembership.json");
            var roomMembership = JsonSettings.FromJson<RoomMembership>(json);

            roomMembership.PersonEmail.Should().Be("john.andersen@example.com");
            roomMembership.PersonDisplayName.Should().Be("John Andersen");
        }

        [Fact]
        public void ShouldReturnValidOrganization()
        {
            var json = File.ReadAllText("Resources/Organization.json");
            var organization = JsonSettings.FromJson<Organization>(json);

            organization.DisplayName.Should().Be("Acme, Inc.");
        }

        [Fact]
        public void ShouldReturnValidWebhook()
        {
            var json = File.ReadAllText("Resources/Webhook.json");
            var webhook = JsonSettings.FromJson<Webhook>(json);

            webhook.Name.Should().Be("My Awesome Webhook");
            webhook.TargetUrl.Should().Be("https://example.com/mywebhook");
            webhook.Resource.Should().Be(WebhookResource.Messages);
            webhook.Event.Should().Be(EventType.Created);
            webhook.Filter.Should().Be("roomId=Y2lzY29zcGFyazovL3VzL1JPT00vYmJjZWIxYWQtNDNmMS0zYjU4LTkxNDctZjE0YmIwYzRkMTU0");
            webhook.Secret.Should().Be("86dacc007724d8ea666f88fc77d918dad9537a15");
            webhook.Status.Should().Be(WebhookStatus.Active);
        }

        [Fact]
        public void ShouldReturnValidWebhookParams()
        {
            var webhookParams = new WebhookParams
            {
                Name = "Test",
                Event = EventType.Created,
                Resource = WebhookResource.Messages,
                TargetUrl = "Url"
            };

            var json = webhookParams.ToJson();
            json.Should().Contain("created");
            json.Should().Contain("messages");
        }

        [Fact]
        public void ShouldReturnValidWebhookData()
        {
            var json = File.ReadAllText("Resources/WebhookPost.json");
            var webhookData = JsonConvert.DeserializeObject<WebhookData<Message>>(json);

            webhookData.Data.Should().BeOfType<Message>();
            webhookData.Data.Id.Should().Be("Y2lzY29zcGFyazovL3VzL01FU1NBR0UvOTJkYjNiZTAtNDNiZC0xMWU2LThhZTktZGQ1YjNkZmM1NjVk");
        }
    }
}
