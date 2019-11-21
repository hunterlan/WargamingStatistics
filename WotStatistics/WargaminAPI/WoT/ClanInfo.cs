using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WargaminAPI.Model;

namespace WargaminAPI.WoT
{
    public class ClanInfo : Info
    {
        public bool GetClan(Clan currentClan)
        {
                urlRequest = resourceMan.GetString("uri_get_clan_by_name") + appID + "&search=" + currentClan.ClanName;
                string resultResponse = GetResponse(urlRequest);
                JObject parsed = JObject.Parse(resultResponse);

                string status = (string)parsed["status"];
                if (status == "ok")
                {
                    try
                    {
                        currentClan.ID = int.Parse((string)parsed["data"][0]["clan_id"]);
                        currentClan.ClanTag = (string)parsed["data"][0]["tag"];
                        currentClan.CountMembers = int.Parse((string)parsed["data"][0]["members_count"]);
                        currentClan.CreatedAt = ConvertFromTimestamp(
                        int.Parse((string)parsed["data"][0]["created_at"]));
                    }
                    catch(Exception ex)
                {

                }
                    return true;
                }
                else
                {
                    return false;
                }
        }

        public string GetNameClan(string id)
        {
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
                    return (string)parsed["data"][id]["name"];
                }
                else
                {
                    return null;
                }
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
                string efficiency = (string)parsed["data"][currentClan.ID.ToString()]["efficiency"]["value"];
                string globalMapStat = (string)parsed["data"][currentClan.ID.ToString()]["gm_elo_rating"]["value"];
                string fbStat = (string)parsed["data"][currentClan.ID.ToString()]["fb_elo_rating"]["value"];
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
                    currentClan.FbEloRating = (float)parsed["data"][currentClan.ID.ToString()]["fb_elo_rating"]["value"];
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
