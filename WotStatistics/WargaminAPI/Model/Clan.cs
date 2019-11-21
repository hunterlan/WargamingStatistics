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

        public override string ToString()
        {
            string text =
                "```Клан: " + ClanName + "\nКлан-тег: " + ClanTag +
                "\nБыл создан " + CreatedAt.ToUniversalTime().ToString() + " по Гринвичу" +
                 "\nКоличество танкистов в клане: " + CountMembers;
            if(GlobalRating <= 0)
            {
                text += "\nЗначение глобального рейтинга отсутсвует.\n";
            }
            else
            {
                text += "\nЗначение глобального рейтинга: " + GlobalRating + "\n";
            }
            if(GmEloRating <= 0)
            {
                text += "Значение эло рейтинга на глобальной карте отсутсвует.\n";
            }
            else
            {
                text += "Значение эло рейтинга на глобальной карте " + GmEloRating + "\n";
            }
            if(FbEloRating <= 0)
            {
                text += "Значение эло рейтинга в укрепрайоне отсутсвует.\n";
            }
            else
            {
                text += "Значение эло рейтинга в укрепрайоне " + FbEloRating + "\n";
            }
            text += "```";
            return text;
        }
    }
}
