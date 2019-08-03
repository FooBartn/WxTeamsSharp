# WxTeamsSharp Documentation

## Quick Start Notes:
Install from Package Management Console:

    Install-Package WxTeamsSharp

## Basic Usage

Take a look at the [Integration Tests](https://github.com/FooBartn/WxTeamsSharp/tree/master/test/WxTeamsSharp.IntegrationTests) for more examples. They should cover most of the ways you can use the library. If all else fails, there's always the [API Reference](https://foobartn.github.io/WxTeamsSharp/api/index.html)!


### Example

```csharp
// Set the Auth Token
WxTeamsApi.SetAuth("MyToken");

// Create a room
var room = await WxTeamsApi.CreateRoomAsync("MyRoomName");

// Get users
var users = await WxTeamsApi.GetUsersByEmailAsync("example@test.com");

// Get user
var user = users.FirstOrDefault(x => x.FirstName == "Bob");

// Add user to room
var membership = await room.AddUserAsync(user.Id);

// Make user moderator
var updatedMembership = await membership.UpdateAsync(true);

// Create webhook for this room
// To catch new messages
var webhook = await WxTeamsApi.CreateWebhookAsync(
    "Example Webhook", 
    "http://example.com/exampleWebhookReceiver", 
    WebhookResource.Messages, 
    EventType.Created, 
    filter: $"roomId={room.Id}");

// OR create the webhook via the room object
var webhook = await room
    .AddMessageCreatedWebhookAsync("Example Webhook", "http://example.com/exampleWebhookReceiver");

// Send a message to the room
var message = await room.SendMessageAsync("**Hello**");

// Delete Webhook
await webhook.DeleteAsync();

// Delete message
await message.DeleteAsync();

// Remove user from room via membership
await membership.DeleteAsync();

// OR

// Remove user from room via room object
await room.RemoveUserAsync("example@test.com");

// Delete room
room.DeleteAsync();

// Create Team
var team = await WxTeamsApi.CreateTeamAsync("My Team");

// Add user to team as moderator
var teamMember = await team.AddUserAsync(user.Id, true);

// Remove user from team
await team.RemoveUserAsync(user.Id);

// Delete team
await team.DeleteAsync();

```
