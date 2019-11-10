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
            //https://api.worldoftanks.ru/wot/account/list/?application_id=9adf6dc175f22b26d8f812ca4dd7d7bb&search=Hunterlan2000
            urlRequest = Properties.Settings.Default.url_find_player + appID + "&search=" + searchNickname;
            Player player = null;
            string resultResponse = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlRequest);
            request.ContentType = "application/json; charset=utf-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                resultResponse = sr.ReadToEnd();
            }


            dynamic temp = JsonConvert.DeserializeObject(resultResponse);

            string status = temp.status;
            if (status == "ok")
            {
                int count = temp.count;
                if(count > 0)
                {
                    player = new Player
                    {
                        Nickname = temp.data[0].nickname,
                        Id = temp.data[0].account_id
                    };
                }
                else
                {
                    throw new PlayerNotFound("Player wasn't found");
                }
            }
            else
            {
                string error = temp.error.message;
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
            Statistics playerStatistic = new Statistics();
            playerStatistic.PlayerId = currentPlayer.Id;

            return playerStatistic;
        }
    }

}
