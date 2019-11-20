using Newtonsoft.Json.Linq;

namespace WargaminAPI.WoT
{
    public class ClanInfo : Info
    {
        public string GetClan(string id)
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
    }
}
