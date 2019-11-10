using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using WotStatistics.Exceptions;
using WotStatistics.Model;

namespace WotStatistics
{
    public class WargaminAPI
    {
        private readonly string appID;
        private string urlRequest;
        public WargaminAPI()
        {
            appID = Properties.Settings.Default.application_id;
        }

        public Player FindPlayer(string searchNickname)
        {
            //https://api.worldoftanks.ru/wot/account/list/?application_id=y0ur_a@@_id_h3r3search=nickname
            urlRequest = Properties.Settings.Default.url_find_player + appID + "&search=" + searchNickname;
            Player player = null;
            string resultResponse = GetResponse(urlRequest);
            dynamic parsed = JsonConvert.DeserializeObject(resultResponse);

            string status = parsed.status;
            if (status == "ok")
            {
                int count = parsed.meta.count;
                if(count > 0)
                {
                    player = new Player
                    {
                        Nickname = parsed.data[0].nickname,
                        Id = parsed.data[0].account_id
                    };
                }
                else
                {
                    throw new PlayerNotFound("Player wasn't found");
                }
            }
            else
            {
                string error = parsed.error.message;
                if (error == "NOT_ENOUGH_SEARCH_LENGTH")
                {
                    throw new PlayerNotFound("Minimum three characters required");
                }
                else if (error == "INVALID_SEARCH")
                {
                    throw new PlayerNotFound("Invalid search");
                }
                else if (error == "SEARCH_NOT_SPECIFIED")
                {
                    throw new PlayerNotFound("Empty nickname");
                }
                else
                {
                    throw new Exception("Something went wrong.");
                }
            }

            return player;
        }

        public Statistics GetStatistic(Player currentPlayer)
        {
            //https://api.worldoftanks.ru/wot/account/info/?application_id=y0ur_a@@_id_h3r3&account_id=00111000

            Statistics playerStatistic = new Statistics();
            playerStatistic.PlayerId = currentPlayer.Id;
            urlRequest = Properties.Settings.Default.url_find_player + appID + "&account_id=" + playerStatistic.PlayerId;
            string resultResponse = GetResponse(urlRequest);
            dynamic parsed = JsonConvert.DeserializeObject(resultResponse);

            string status = parsed.status;
            if(status == "ok")
            {
                playerStatistic.Rating = parsed.global_rating;
                playerStatistic.Clan = parsed.clan_id; //TODO: write finding clan
                playerStatistic.Winrate = CountWinRate(parsed.all[0].wins, parsed.all[0].losses);
                playerStatistic.CountBattles = parsed.all[0].battles;
                playerStatistic.LastBattle = ConvertFromTimestamp(parsed.last_battle_time);
            }

            return playerStatistic;
        }

        private DateTime ConvertFromTimestamp(int timestap)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestap);
        }
        private double CountWinRate(int wins, int losses)
        {
            double winRate = (double)wins / losses;
            return winRate;
        }

        private string GetResponse(string urlRequest)
        {
            string resultResponse = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlRequest);
            request.ContentType = "application/json; charset=utf-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                resultResponse = sr.ReadToEnd();
            }

            return resultResponse;
        }
    }

}
