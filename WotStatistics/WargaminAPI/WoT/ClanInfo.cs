using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WargaminAPI.Model;

namespace WargaminAPI.WoT
{
    public class ClanInfo : Info
    {
        public bool GetClan(Clan currentClan)
        {

            if (currentClan.ID == -1)
            {
                return false;
            }
            else
            {
                urlRequest = resourceMan.GetString("uri_get_clan") + appID + "&clan_id=" + currentClan.ID;
                string resultResponse = GetResponse(urlRequest);
                JObject parsed = JObject.Parse(resultResponse);

                string status = (string)parsed["status"];
                if (status == "ok")
                {
                    currentClan.ClanName = (string)parsed["data"][currentClan.ID]["name"];
                    currentClan.ClanTag = (string)parsed["data"][currentClan.ID]["tag"];
                    currentClan.CountMembers = int.Parse((string)parsed["data"][currentClan.ID]["members_count"]);
                    currentClan.CreatedAt = ConvertFromTimestamp(
                        int.Parse((string)parsed["data"][currentClan.ID]["created_at"]));
                }
                return true;
            }
        }


        public bool GetStat(Clan currentClan)
        {
            urlRequest = resourceMan.GetString("uri_get_stat_clan") + appID + "&clan_id=" + currentClan.ID;
            string resultResponse = GetResponse(urlRequest);
            JObject parsed = JObject.Parse(resultResponse);

            string status = (string)parsed["status"];
            if (status == "ok")
            {
                string efficiency = (string)parsed["data"][currentClan.ID]["efficiency"]["value"];
                string globalMapStat = (string)parsed["data"][currentClan.ID]["gm_elo_rating"]["value"];
                string fbStat = (string)parsed["data"][currentClan.ID]["fb_elo_rating"]["value"];
                if (efficiency != null)
                {
                    currentClan.GlobalRating = float.Parse(efficiency);
                }
                else
                {
                    currentClan.GlobalRating = -1;
                }
                if (globalMapStat != null)
                {
                    currentClan.GmEloRating = float.Parse(globalMapStat);
                }
                else
                {
                    currentClan.GmEloRating = -1;
                }
                if (fbStat != null)
                { 
                    currentClan.FbEloRating = (float)parsed["data"][currentClan.ID]["fb_elo_rating"]["value"];
                }
                else
                {
                    currentClan.FbEloRating = -1;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
