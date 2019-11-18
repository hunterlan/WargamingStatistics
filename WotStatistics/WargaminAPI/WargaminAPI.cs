﻿using Newtonsoft.Json;
using System;
using WargaminAPI.Model;
using WargaminAPI.Exceptions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using WargaminAPI.Properties;

namespace WotStatistics
{
    public class WargaminAPI
    {
        private readonly string appID;
        private string urlRequest;
        System.Resources.ResourceManager resourceMan;
        public WargaminAPI()
        {
            resourceMan = new System.Resources.ResourceManager("WargaminAPI.Properties.Resources", typeof(Resources).Assembly);
            appID = resourceMan.GetString("application_id");
        }

        public Player FindPlayer(string searchNickname)
        {
            //https://api.worldoftanks.ru/wot/account/list/?application_id=y0ur_a@@_id_h3r3search=nickname
            urlRequest = resourceMan.GetString("url_find_player") + appID + "&search=" + searchNickname;
            Player player = null;
            string resultResponse = GetResponse(urlRequest);
            dynamic parsed = JsonConvert.DeserializeObject(resultResponse);

            string status = parsed.status;
            if (status == "ok")
            {
                int count = parsed.meta.count;
                if (count > 0)
                {
                    player = new Player
                    {
                        Nickname = parsed.data[0].nickname,
                        Id = parsed.data[0].account_id
                    };
                }
                else
                {
                    throw new PlayerNotFound("Игрок не найден");
                }
            }
            else
            {
                string error = parsed.error.message;
                if (error == "NOT_ENOUGH_SEARCH_LENGTH")
                {
                    throw new PlayerNotFound("Минимум три символа требуется");
                }
                else if (error == "INVALID_SEARCH")
                {
                    throw new PlayerNotFound("Неверный поиск");
                }
                else if (error == "SEARCH_NOT_SPECIFIED")
                {
                    throw new PlayerNotFound("Пустой никнейм");
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
            urlRequest = resourceMan.GetString("uri_get_stat") + appID + "&account_id=" + playerStatistic.PlayerId;
            string resultResponse = GetResponse(urlRequest);
            JObject parsed = JObject.Parse(resultResponse);

            string status = (string)parsed["status"];
            if (status == "ok")
            {
                playerStatistic.Rating = (int)parsed["data"][playerStatistic.PlayerId.ToString()]["global_rating"];
                string clanID = (string)parsed["data"][playerStatistic.PlayerId.ToString()]["clan_id"];
                playerStatistic.Clan = GetClan(clanID);
                playerStatistic.CountBattles = (int)parsed["data"][playerStatistic.PlayerId.ToString()]["statistics"]["all"]["battles"];
                playerStatistic.Winrate = CountWinRate(
                    (int)parsed["data"][playerStatistic.PlayerId.ToString()]["statistics"]["all"]["wins"],
                    playerStatistic.CountBattles);
                playerStatistic.LastBattle = ConvertFromTimestamp((int)parsed["data"][playerStatistic.PlayerId.ToString()]["last_battle_time"]);
            }

            return playerStatistic;
        }

        private string GetClan(string id)
        {
            string nameClan = "";

            if (id == null)
            {
                return null;
            }
            else
            {
                urlRequest = resourceMan.GetString("uri_get_clan") + appID + "&clan_id=" + id;
                string resultResponse = GetResponse(urlRequest);
                JObject parsed = JObject.Parse(resultResponse);

                string status = (string)parsed["status"];
                if (status == "ok")
                {
                    nameClan = (string)parsed["data"][id]["name"];
                }
            }

            return nameClan;
        }

        private DateTime ConvertFromTimestamp(int timestap)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestap).ToUniversalTime();
        }
        private double CountWinRate(int wins, int countBattles)
        {
            double winRate = (double)wins / countBattles;
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