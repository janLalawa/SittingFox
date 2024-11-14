from discord import app_commands, Interaction, Member
from discord.ext import commands
from discord.ext.commands import Bot


class RankupCog(commands.Cog):
    def __init__(self, bot: Bot) -> None:
        self.bot: Bot = bot

    @app_commands.command(name="promote")
    async def promote(
            self, interaction: Interaction, member: Member, rank: str
    ) -> None:
        # Promotion logic
        await interaction.response.send_message(f"Promoted {member.name} to {rank}")


async def setup(bot: Bot) -> None:
    await bot.add_cog(RankupCog(bot))
