namespace SittingFox.Modules.Admin;

public class AdminModule(DiscordSocketClient client) : FoxModule(client)
{
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
}