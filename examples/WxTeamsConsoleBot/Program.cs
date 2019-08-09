using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using WxTeamsConsoleBot.Models;
using WxTeamsConsoleBot.Services;
using WxTeamsSharp.Extensions;
using WxTeamsSharp.Interfaces.Api;

namespace WxTeamsConsoleBot
{
    internal class Program
    {
        private static void Main() => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            // Configure built-in DI via Microsoft.Extensions.DependencyInjection
            var serviceProvider = ConfigureServices();

            // Get services from DI container
            var api = serviceProvider.GetRequiredService<IWxTeamsApi>();
            var messageService = serviceProvider.GetRequiredService<MessageService>();
            var settings = serviceProvider.GetRequiredService<IOptions<Settings>>();

            // Setup Auth Token
            api.Initialize(settings.Value.BotToken);

            // Start our custom message service
            await messageService.StartAsync();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Settings>()
                .Build();

            // Add services
            services
                // Use the AddWxTeamsSharp extension to set up WxTeamsSharp in DI
                .AddWxTeamsSharp()
                // Set up our custom MessageService
                .AddSingleton<MessageService>()
                // Get Bot Token from user-secrets. You may do this from
                // an AppSettings.json etc
                .AddOptions()
                .Configure<Settings>(configuration);

            return services.BuildServiceProvider();
        }
    }
}
