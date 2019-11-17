using DSharpPlus;
using Execute;
using System.Threading.Tasks;
using WargaminAPI.Exceptions;
using WargaminAPI.Model;

namespace WotStatistics
{
    class Executor
    {
        static DiscordClient discord;
        static System.Resources.ResourceManager resourceMan;
        static void Main(string[] args)
        {
            resourceMan = new System.Resources.ResourceManager("Execute.Settings", typeof(Settings).Assembly);
            MainTask(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainTask(string[] args)
        {
            WargaminAPI operations = new WargaminAPI();
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = resourceMan.GetString("DisToken"),
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("&stats"))
                {
                    string response = e.Message.Content;
                    string playerNickname = parseNickname(response);
                    Player currentPlayer = new Player();
                    bool foundPlayer = false;
                    try
                    {
                        currentPlayer = operations.FindPlayer(playerNickname);
                        foundPlayer = true;
                    }
                    catch (PlayerNotFound ex)
                    {
                        await e.Message.RespondAsync(ex.Message);
                    }
                    catch
                    {
                        string infoMsg = "Напишите GRAF или hunterlan об ошибке и пропишите все ваши действия.";
                        await e.Message.RespondAsync(infoMsg);
                    }

                    if (foundPlayer)
                    {
                        Statistics playerStatistics = operations.GetStatistic(currentPlayer);
                        await e.Message.RespondAsync("Игрок: " + currentPlayer.ToString() +
                            "  \n" + playerStatistics.ToString());
                    }
                }
                else if (e.Message.Content.ToLower() == "&help")
                {
                    string helpMsg = "Помните: [] - обязательный параметр  \n" +
                    @"*&stats [your_nick]* - показывает вашу статистику";
                    await e.Message.RespondAsync(helpMsg);
                }
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static string parseNickname(string command)
        {
            string nickname = "";

            int i = 6;
            command = command.Replace(" ", "");
            while (i < command.Length)
            {
                nickname += command[i];
                i++;
            }

            return nickname;
        }
    }
}
