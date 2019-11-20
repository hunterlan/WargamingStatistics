using System;
using System.Collections.Generic;
using System.Text;

namespace WargaminAPI.Model
{
    public class Clan
    {
        public int ID { get; set; }
        public string ClanName { get; set; }

        public string ClanTag { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CountMembers { get; set; }

        public float FbEloRating { get; set; }

        public float GmEloRating { get; set; }

        public float GlobalRating { get; set; }
    }
}
