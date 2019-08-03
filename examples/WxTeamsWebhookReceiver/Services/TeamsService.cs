using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WxTeamsSharp.Api;
using WxTeamsSharp.Enums;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Webhooks;

namespace WxTeamsWebhookReceiver.Services
{
    public class TeamsService
    {
        public TeamsService(IConfiguration configuration)
        {
            var token = configuration.GetSection("BotToken").Value;
            WxTeamsApi.SetAuth(token);
        }

        public async Task HandleCreatedMessage(WebhookData<Message> webhookData)
        {
            var person = await WxTeamsApi.GetUserAsync(webhookData.Data.AuthorId);

            // The Message Created event will also trigger off a message created by the bot
            // Unless you want to end up with an endless loop of messages, you have to make
            // sure you're not responding to yourself.

            // At the same time, it's probably a good idea to consider not letting your bot
            // respond to other bots at all. Only people.
            if (person.Type != PersonType.Bot)
            {
                var message = await webhookData.Data.GetFullMessageAsync();

                await MessageBuilder.New()
                    .SendToRoom(message.RoomId)
                    .WithMarkdown("**Hi!**")
                    .Build()
                    .SendAsync();
            }
        }
    }
}
