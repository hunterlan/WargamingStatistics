using DSharpPlus;
using Execute;
using System;
using System.Linq;
using System.Threading.Tasks;
using WargaminAPI.Exceptions;
using WargaminAPI.Model;
using WargaminAPI.WoT;

namespace WargamingStat
{
    class Executor
    {
        static DiscordClient discord;
        static System.Resources.ResourceManager resourceMan;
        static readonly string STAT_PLAYER = "&stats_player";
        static readonly string HELP = "&help";
        static readonly string PHRASE = "DisToken";
        static void Main(string[] args)
        {
            resourceMan = new System.Resources.ResourceManager("Execute.Settings", typeof(Settings).Assembly);
            Console.WriteLine(WargaminAPI.Enigma.Encrypt("9adf6dc175f22b26d8f812ca4dd7d7bb", "AppID"));
            //MainTask(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainTask(string[] args)
        {
            string token = WargaminAPI.Enigma.Decrypt(resourceMan.GetString("DisToken"), PHRASE);
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = resourceMan.GetString("DisToken"),
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.MessageCreated += async e =>
            {
                string message = e.Message.Content;
                if(message.ToLower().StartsWith(HELP))
                {
                    string helpMsg = e.Author.Mention + "```css\nПомните: [] - обязательный параметр  \n" +
                            "&stats_player [type_of_game] [your_nick] - показывает вашу статистику\n" +
                            "Где [type_of_game] - список игр варгейминг:\n" +
                            "- WoT\n- WoW\n ```";
                    await e.Message.RespondAsync(helpMsg);
                }
                else
                {
                    string typeOfGame = parseTypeOfGame(message);
                    if (typeOfGame == null || typeOfGame == "")
                    {
                        
                        await e.Message.RespondAsync(e.Author.Mention +",\nВы не указали, какая игра. Введите &help для помощи.");
                    }
                    else if (typeOfGame.ToLower() == "wow")
                    {
                        await e.Message.RespondAsync("В разработке");
                    }
                    else if (typeOfGame.ToLower() == "wot")
                    {
                        PlayerInfo operations = new PlayerInfo();

                        if (message.ToLower().StartsWith(STAT_PLAYER))
                        {
                            string playerNickname = parseNickname(message, STAT_PLAYER, typeOfGame);
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
                                string infoMsg = e.Author.Mention + "Напишите GRAF или hunterlan об ошибке и пропишите все ваши действия.";
                                await e.Message.RespondAsync(infoMsg);
                            }
                            if (foundPlayer)
                            {
                                Statistics playerStatistics = operations.GetStatistic(currentPlayer);
                                await e.Message.RespondAsync(e.Author.Mention + "\n```Игрок: " + currentPlayer.ToString() +
                                    "  \n" + playerStatistics.ToString() + "```");
                            }
                        }
                    }
                }
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static string parseNickname(string message, string command, string typeOfGame)
        {
            string nickname = "";

            message = message.Replace(command, "");
            message = message.Replace(typeOfGame, "");
            nickname = message.Replace(" ", "");

            return nickname;
        }

        static string parseTypeOfGame(string message)
        {
            string typeOfGame = "";

            typeOfGame = message.Split(' ').Skip(1).FirstOrDefault();

            return typeOfGame;
        }
    }
}
