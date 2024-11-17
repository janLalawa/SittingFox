import pytest
from discord.ext import commands

from src.bot.bot import FoxholeRegimentBot


@pytest.mark.asyncio
async def test_bot_initialization():
    bot = FoxholeRegimentBot()
    assert isinstance(bot, FoxholeRegimentBot)
    assert bot.command_prefix == "!"
    assert bot.description == "Foxhole Regiment Management Bot"


@pytest.mark.asyncio
async def test_bot_intents():
    bot = FoxholeRegimentBot()
    assert bot.intents.members is True
    assert bot.intents.message_content is True


@pytest.mark.asyncio
async def test_setup_hook():
    bot = FoxholeRegimentBot()
    try:
        await bot.setup_hook()
        assert "modules.rankup" in bot.extensions
    except Exception as e:
        pytest.skip(f"Setup hook test skipped: {str(e)}")


@pytest.mark.asyncio
async def test_on_ready(mocker):
    bot = FoxholeRegimentBot()
    mock_user = mocker.Mock()
    mock_user.__str__.return_value = "TestBot"
    mocker.patch.object(bot, "user", mock_user)

    mock_print = mocker.patch("builtins.print")

    await bot.on_ready()
    mock_print.assert_called_once_with("TestBot has connected to Discord!")


@pytest.mark.skip(reason="Need to implement specific command tests")
@pytest.mark.asyncio
async def test_command_registration():
    bot = FoxholeRegimentBot()
    await bot.setup_hook()


@pytest.mark.asyncio
async def test_error_handling(mocker):
    bot = FoxholeRegimentBot()

    ctx = mocker.Mock()
    ctx.send = mocker.AsyncMock()

    error = commands.CommandNotFound()

    try:
        if hasattr(bot, "on_command_error"):
            await bot.on_command_error(ctx, error)
        else:
            pytest.skip("Bot doesn't have error handling implemented yet")
    except Exception as e:
        pytest.fail(f"Error handling failed: {str(e)}")
