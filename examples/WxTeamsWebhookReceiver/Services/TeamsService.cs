using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Webhooks;

namespace WxTeamsWebhookReceiver.Services
{
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
}
