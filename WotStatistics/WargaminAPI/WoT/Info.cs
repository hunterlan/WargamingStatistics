using System;
using WargaminAPI.Properties;
using System.IO;
using System.Net;

namespace WargaminAPI.WoT
{
    public class Info
    {
        protected readonly string appID;
        protected string urlRequest;
        protected System.Resources.ResourceManager resourceMan;
        private readonly string PHRASE = "AppID";
        public Info()
        {
            resourceMan = new System.Resources.ResourceManager("WargaminAPI.Properties.Resources", typeof(Resources).Assembly);
            appID = WargaminAPI.Enigma.Decrypt(resourceMan.GetString("application_id"), PHRASE);
        }
        protected string GetResponse(string urlRequest)
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

        protected DateTime ConvertFromTimestamp(int timestap)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestap).ToUniversalTime();
        }
        protected double CountWinRate(int wins, int countBattles)
        {
            double winRate = (double)wins / countBattles;
            return winRate;
        }
    }
}
