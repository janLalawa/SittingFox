using System.Reflection;
using SittingFox.Modules.General;
using Constants = SittingFox.Common.Constants;

namespace SittingFox.Controllers;

public class Runner(
    IConfiguration configuration,
    DiscordSocketClient client,
    CommandService commands,
    IDiscordClientBuilder clientBuilder,
    IServiceProvider services)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await clientBuilder.WithToken(configuration["Discord:Token"]);

        client.MessageReceived += HandleMessageAsync;
        client.Log += Logger.Log;
        client.Ready += Ready;

        await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await client.StopAsync();
    }

    private Task Ready()
    {
        Console.WriteLine($"Bot is connected as {client.CurrentUser.Username}");
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(SocketMessage messageParam)
    {
        if (messageParam is not SocketUserMessage message) return;
        if (message.Author.IsBot) return;

        var argPos = 0;
        if (!(message.HasCharPrefix(Constants.CommandPrefix, ref argPos) ||
              message.HasMentionPrefix(client.CurrentUser, ref argPos)))
            return;

        var context = new SocketCommandContext(client, message);
        var result = await commands.ExecuteAsync(context, argPos, services);

        if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            await context.Channel.SendMessageAsync($"Error: {result.ErrorReason}");
    }
}
