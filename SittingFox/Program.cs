using Microsoft.Extensions.DependencyInjection;
using SittingFox.Controllers;

namespace SittingFox;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
                       .ConfigureServices((context, services) =>
                       {
                           services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                                                                 .SetBasePath(AppContext.BaseDirectory)
                                                                 .AddJsonFile("appsettings.json")
                                                                 .Build());

                           services.AddSingleton(_ => new DiscordSocketClient(new DiscordSocketConfig
                           {
                               GatewayIntents = GatewayIntents.MessageContent |
                                                GatewayIntents.Guilds |
                                                GatewayIntents.GuildMessages |
                                                GatewayIntents.GuildMembers,
                               AlwaysDownloadUsers = true
                           }));
                           services.AddSingleton<CommandService>();
                           services.AddSingleton<IDiscordClientBuilder, DiscordClientBuilder>();

                           services.AddHostedService<Runner>();
                       })
                       .Build();

        await host.RunAsync();
    }
}