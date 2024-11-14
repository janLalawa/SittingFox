import asyncio

from src.bot.bot import FoxholeRegimentBot
from src.config import settings


async def main() -> None:
    bot = FoxholeRegimentBot()
    await bot.start(settings.DISCORD_TOKEN)


if __name__ == "__main__":
    asyncio.run(main())
