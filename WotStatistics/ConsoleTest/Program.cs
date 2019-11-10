using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WotStatistics;
using WotStatistics.Model;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            WargaminAPI players = new WargaminAPI();
            string myNick = "Andreijudo";

            Player result = players.FindPlayer(myNick);
            Statistics stat = players.GetStatistic(result);
        }
    }
}
