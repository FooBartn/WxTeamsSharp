[![Build status](https://ci.appveyor.com/api/projects/status/s3wub78ddch27qmx/branch/master?svg=true)](https://ci.appveyor.com/project/FooBartn/wxteamssharp/branch/master)

## WxTeamsSharp: A Webex Teams .NET C# Library

A .NET Core 3.1 Library to help developers communicate easily with the Webex Teams API. 

Available on GitHub and [NuGet](https://www.nuget.org/packages/WxTeamsSharp/):

    Install-Package WxTeamsSharp

> **Notice:** 2.0.0 was a major update with a paradigm shift from static to DI.
> You will want to check both of the examples for ways to get started.

## Basic Usage

Take a look at the [Integration Tests](https://github.com/FooBartn/WxTeamsSharp/tree/master/test/WxTeamsSharp.IntegrationTests) and [Example Projects](https://github.com/FooBartn/WxTeamsSharp/tree/master/examples). They should cover most of the ways you can use the library. If all else fails, there's always the [API Reference](https://foobartn.github.io/WxTeamsSharp/api/index.html)!


### Example

#### DI Setup
Be sure to check out the [WxTeamsWebhookReciever](https://github.com/FooBartn/WxTeamsSharp/tree/master/examples/WxTeamsWebhookReceiver) for an ASP.NET Core example
and [WxTeamsConsoleBot](https://github.com/FooBartn/WxTeamsSharp/tree/master/examples/WxTeamsConsoleBot) for .NET Core Console example.
```csharp
 public void ConfigureServices(IServiceCollection services)
        {
            ... 

            services.AddWxTeamsSharp();

            ...
        }
```

#### Code Example
```csharp
public class TeamsService
    {
        private readonly IWxTeamsApi _wxTeamsApi;

        public TeamsService(IConfiguration configuration, IWxTeamsApi wxTeamsApi)
        {
            var token = configuration.GetSection("BotToken").Value;
            _wxTeamsApi = wxTeamsApi;
            _wxTeamsApi.Initialize(token);
        }

        public async Task HandleCreatedMessage(WebhookData<Message> webhookData)
        {
            var person = await _wxTeamsApi.GetUserAsync(webhookData.Data.AuthorId);
            var room = await _wxTeamsApi.CreateRoomAsync("test");
            await room.DeleteAsync();

            // The Message Created event will also trigger off a message created by the bot
            // Unless you want to end up with an endless loop of messages, you have to make
            // sure you're not responding to yourself.

            // At the same time, it's probably a good idea to consider not letting your bot
            // respond to other bots at all. Only people.
            if (person.Type != PersonType.Bot)
            {
                var message = await _wxTeamsApi.GetMessageAsync(webhookData.Data.Id);

                var newMessage = MessageBuilder.New()
                    .SendToRoom(message.RoomId)
                    .WithMarkdown("**Hi!**")
                    .Build();

                await _wxTeamsApi.SendMessageAsync(newMessage);
            }
        }
    }
```
