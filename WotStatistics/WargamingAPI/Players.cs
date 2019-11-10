using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

namespace WotStatistics
{
    public class Players
    {
        private readonly string appID;
        private string urlRequest;
        public Players()
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
                player = new Player
                {
                    Nickname = temp.data[0].nickname,
                    Id = temp.data[0].account_id
                };
            }
            else
            {

                string error = temp.error.message;
                if (error == "NOT_ENOUGH_SEARCH_LENGTH")
                {
                    throw new Exception("Write at least 3 symbols.");
                }
                else if (error == "INVALID_SEARCH")
                {
                    throw new Exception("Invalid search");
                }
                else if (error == "SEARCH_NOT_SPECIFIED")
                {
                    throw new Exception("Empty nickname");
                }
                else
                {
                    throw new Exception("Something went wrong.");
                }

            }

            return player;
        }
    }

    public class Player
    {
        public string Nickname { get; set; }

        public int Id { get; set; }
    }

}
