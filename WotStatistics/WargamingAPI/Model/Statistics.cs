using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotStatistics.Model
{
    public class Statistics
    {
        public int PlayerId { get; set; }

        public DateTime LastBattle { get; set; }

        public double Winrate { get; set; }

        public string Clan { get; set; }

        public int CountBattles { get; set; }

        public int Rating { get; set; }

        public override string ToString()
        {
            string statistic = "Последний бой был " + LastBattle.ToString() +
                "  \nПроцент побед: " + Math.Round((Winrate * 100), 2) + "%  \n";
            if(Clan != null)
            {
                statistic += "Клан: " + Clan + "  \n"; 
            }
            else
            {
                statistic += "В клане не состоит" + "  \n";
            }

            statistic += "Количество боев: " + CountBattles + "  \n" + 
                "Личный рейтинг: " + Rating;

            return statistic;
        }
    }
}
