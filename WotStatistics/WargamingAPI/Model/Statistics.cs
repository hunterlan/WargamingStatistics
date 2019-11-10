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
    }
}
