import discord
from discord.ext import commands


class FoxholeRegimentBot(commands.Bot):
    def __init__(self) -> None:
        intents = discord.Intents.default()
        intents.members = True
        intents.message_content = True

        super().__init__(
            command_prefix="!",
            intents=intents,
            description="Foxhole Regiment Management Bot",
        )

    async def setup_hook(self) -> None:
        await self.load_extension("modules.rankup")

    async def on_ready(self) -> None:
        print(f"{self.user} has connected to Discord!")
