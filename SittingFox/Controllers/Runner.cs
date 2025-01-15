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

        await commands.AddModuleAsync<GeneralCommands>(services);
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

public class GeneralCommands : ModuleBase<SocketCommandContext>
{
    private readonly DiscordSocketClient _client;

    public GeneralCommands(DiscordSocketClient client)
    {
        _client = client;
    }

    [Command("hello")]
    [Summary("Says hello to the user")]
    public async Task HelloCommand()
    {
        await ReplyAsync($"Hello {Context.User.Username}!");
    }

    [Command("giverole")]
    [RequireUserPermission(GuildPermission.ManageRoles)]
    [RequireBotPermission(GuildPermission.ManageRoles)]
    public async Task GiveRole(SocketGuildUser user, [Remainder] string roleName)
    {
        var role = Context.Guild.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));

        if (role == null)
        {
            await ReplyAsync($"Role '{roleName}' not found!");
            return;
        }

        try
        {
            await user.AddRoleAsync(role);
            await ReplyAsync($"Successfully added role {role.Name} to {user.Username}!");
        }
        catch (Exception ex)
        {
            await ReplyAsync($"Error adding role: {ex.Message}");
        }
    }

    [Command("ping")]
    [Summary("Returns the bot's latency")]
    public async Task PingCommand()
    {
        await ReplyAsync($"Pong! Latency: {_client.Latency}ms");
        await ReplyAsync($"Pong! Latency: {Context.Client.Latency}ms");
    }

    [Command("info")]
    [Summary("Gets info about a user")]
    public async Task InfoCommand([Remainder] SocketGuildUser user = null)
    {
        user ??= Context.User as SocketGuildUser;

        var embed = new EmbedBuilder()
                    .WithTitle($"Info for {user.Username}")
                    .WithColor(Color.Blue)
                    .AddField("Joined Server", user.JoinedAt?.ToString() ?? "Unknown")
                    .AddField("Account Created", user.CreatedAt.ToString())
                    .AddField("Roles", string.Join(", ", user.Roles.Select(r => r.Name)))
                    .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                    .Build();

        await ReplyAsync(embed: embed);
    }
}