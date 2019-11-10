using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WotStatistics;

namespace WargamingAPITests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFindPlayer()
        {
            Players players = new Players();
            List<string> nicknames = new List<string> { "GraF", "Hunterlan2000" };

            List<Player> playersExcepted = new List<Player>
            {
                new Player
                {
                    Nickname = "GraF",
                    Id = 1270648
                },
                new Player
                {
                    Nickname = "Hunterlan2000",
                    Id = 30373360
                }
            };

            List<Player> playersResult = new List<Player>();
            for(int i = 0; i < nicknames.Count; i++)
            {
                playersExcepted.Add(players.FindPlayer(nicknames[i]));
            }

            Assert.AreEqual(playersExcepted, playersResult);
        }
    }
}
