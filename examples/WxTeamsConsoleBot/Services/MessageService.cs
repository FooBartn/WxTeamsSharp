using System;
using System.Threading;
using System.Threading.Tasks;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Models.Messages;

namespace WxTeamsConsoleBot.Services
{
    public class MessageService
    {
        private readonly IWxTeamsApi _wxTeamsApi;
        private CancellationTokenSource _token;

        public MessageService(IWxTeamsApi wxTeamsApi)
        {
            _wxTeamsApi = wxTeamsApi;
        }

        public async Task StartAsync()
        {
            _token = new CancellationTokenSource();

            while (!_token.IsCancellationRequested)
            {
                var newMessage = MessageBuilder.New()
                    .SendToRoom("Y2lzY29zcGFyazovL3VzL1JPT00vOTg4ODAyOTAtYThiOC0xMWU5LWI5YWMtYWZjZDIwMDFjODI0")
                    .WithMarkdown("Sent from **ConsoleBot**")
                    .Build();

                await _wxTeamsApi.SendMessageAsync(newMessage);

                await Task.Delay(TimeSpan.FromSeconds(10));
            }

        }

        public void Stop() => _token.Cancel();
    }
}
