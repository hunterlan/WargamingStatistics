using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WotStatistics;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Players players = new Players();
            string myNick = "Hunterlan2000";

            Player result = players.FindPlayer(myNick);
        }
    }
}
