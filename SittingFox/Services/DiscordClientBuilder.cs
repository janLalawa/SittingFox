namespace SittingFox.Services;

public interface IDiscordClientBuilder
{
    DiscordSocketClient Client { get; }
    CommandService Commands { get; }
    Task<IDiscordClientBuilder> WithToken(string token);
}

public class DiscordClientBuilder(DiscordSocketClient client, CommandService commands) : IDiscordClientBuilder
{
    public DiscordSocketClient Client { get; } = client;
    public CommandService Commands { get; } = commands;

    public async Task<IDiscordClientBuilder> WithToken(string token)
    {
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();
        return this;
    }
}