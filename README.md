## Purpose

Create a .NET Standard Library to make communicating with the Webex Teams API easier

## Basic Usage

Will continue to update user friend documentation. In the meantime, reviewing the integration tests 
and [API docs](https://foobartn.github.io/WxTeamsSharp/api/index.html) may provide insight.

### Setting the Authentication Token is Required

```WxTeamsApi.SetAuth("MyToken");```

### Rooms

Get All Rooms: ```var rooms = await WxTeamsApi.GetRoomsAsync();```

Get A Single Room: ```var room = await WxTeamsApi.GetRoomAsync("RoomId");```

Create A Room: ```var room = await WxTeamsApi.CreateRoomAsync("MyRoomName");```

Send A Message via Room Object: ```var message = await room.SendMessageAsync("**hi!**);```

Add A User to via Room Object: ```var membership = await room.AddUserAsync("UserId");```

Delete A Room via Room Object: ```await room.DeleteAsync();```

### Webhooks

Get All Webhooks: ```var webhooks = await WxTeamsApi.GetWebhooksAsync();```

Get A Single Webhook: ```var webhook = await WxTeamsApi.GetWebhookAsync("MyWebhookId");```

Create Webhook: ```var webhook = await WxTeamsApi.CreateWebhookAsync("MyWebhook", "http://example.com/myWebhookReceiver", WebhookResource.Messages, EventType.Created, filter: "roomId=MyRoomId");```

Delete Webhook: ```await webhook.DeleteAsync();```
