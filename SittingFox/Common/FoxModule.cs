namespace SittingFox.Common;

public abstract class FoxModule(DiscordSocketClient client) : ModuleBase<SocketCommandContext>
{
    protected DiscordSocketClient Client { get; } = client;

    public virtual void Load()
    {
    }
}
