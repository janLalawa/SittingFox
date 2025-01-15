namespace SittingFox.Modules.General;

public class GeneralModule(DiscordSocketClient client) : FoxModule(client)
{
    [Command("hello")]
    [Summary("Says hello to the user")]
    public async Task HelloCommand()
    {
        await ReplyAsync($"Hello {Context.User.Username}!");
    }

    [Command("ping")]
    [Summary("Returns the bot's latency")]
    public async Task PingCommand()
    {
        await ReplyAsync($"Pong! Latency: {Client.Latency}ms");
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