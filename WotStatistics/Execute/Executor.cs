using DSharpPlus;
using Execute;
using System;
using System.Linq;
using System.Threading.Tasks;
using WargaminAPI;
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
        static readonly string INFO_CLAN = "&clan";
        static readonly string HELP = "&help";
        static readonly string PHRASE = "DisToken";
        static void Main(string[] args)
        {
            resourceMan = new System.Resources.ResourceManager("Execute.Settings", typeof(Settings).Assembly);
            MainTask(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
        static async Task MainTask(string[] args)
        {
            string token = WargaminAPI.Enigma.Decrypt(resourceMan.GetString("DisToken"), PHRASE);
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.MessageCreated += async e =>
            {
                string message = e.Message.Content;
                if (message.StartsWith("&"))
                {
                    if (message.ToLower().StartsWith(HELP))
                    {
                        string helpMsg = e.Author.Mention + "```css\nПомните: [] - обязательный параметр  \n" +
                                "&stats_player [type_of_game] [your_nick] - показывает вашу статистику\n" +
                                "&clan [type_of_game] [clan_name] - показывает инфу про клан\n" +
                                "Где [type_of_game] - список игр варгейминг:\n" +
                                "- WoT\n- WoW\n ```";
                        await e.Message.RespondAsync(helpMsg);
                    }
                    else
                    {
                        string typeOfGame = parseTypeOfGame(message);
                        if (typeOfGame == null || typeOfGame == "")
                        {

                            await e.Message.RespondAsync(e.Author.Mention + ",\nВы не указали, какая игра. Введите &help для помощи.");
                        }
                        else if (typeOfGame.ToLower() == "wow")
                        {
                                await e.Message.RespondAsync(e.Author.Mention + ",\nВ связи с тем, что разработчики по неизвестным причинам " + 
                                "сделали очень скудный набор публичный данных для разработчиков, адекватную статистику для WoWS сделать невозможно!\n");
                        }
                        else if (typeOfGame.ToLower() == "wot")
                        {
                            PlayerInfo operations = new PlayerInfo();

                            if (message.ToLower().StartsWith(STAT_PLAYER))
                            {
                                string playerNickname = parseName(message, STAT_PLAYER, typeOfGame, true);
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
                            else if (message.ToLower().StartsWith(INFO_CLAN))
                            {
                                ClanInfo ops = new ClanInfo();
                                ClanWoT clan = new ClanWoT();
                                clan.ClanName = parseName(message, INFO_CLAN, typeOfGame, false);
                                try
                                {
                                    ops.GetClan(clan);
                                }
                                catch (ClanNotFound ex)
                                {
                                    await e.Message.RespondAsync(e.Author.Mention + "\n" + ex.Message);
                                }
                                catch
                                {
                                    string infoMsg = e.Author.Mention + "\nНапишите GRAF или hunterlan об ошибке и пропишите все ваши действия.";
                                    await e.Message.RespondAsync(infoMsg);
                                }
                                try
                                {
                                    ops.GetStat(clan);
                                    await e.Message.RespondAsync(e.Author.Mention + clan.ToString());
                                }
                                catch (ClanNotFound ex)
                                {
                                    await e.Message.RespondAsync(e.Author.Mention + "\n" + ex.Message);
                                }
                                catch
                                {
                                    string infoMsg = e.Author.Mention + "\nНапишите GRAF или hunterlan об ошибке и пропишите все ваши действия.";
                                    await e.Message.RespondAsync(infoMsg);
                                }


                            }
                        }
                    }
                }
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        static string parseName(string message, string command, string typeOfGame, bool needToRemoveSpace)
        {
            message = message.Replace(command, "");
            message = message.Replace(typeOfGame, "");
            message = message.TrimStart();
            message = message.TrimEnd();

            return message;
        }

        static string parseTypeOfGame(string message)
        {
            string typeOfGame = "";

            typeOfGame = message.Split(' ').Skip(1).FirstOrDefault();

            return typeOfGame;
        }
    }
}
