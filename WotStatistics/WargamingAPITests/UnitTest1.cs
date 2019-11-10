using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WotStatistics;
using WotStatistics.Model;

namespace WargamingAPITests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFindPlayer()
        {
            WargaminAPI players = new WargaminAPI();
            List<string> nicknames = new List<string> { 
                "GraF", 
                "Hunterlan2000" 
            };

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
                playersResult.Add(players.FindPlayer(nicknames[i]));
            }

            for(int i = 0; i < playersResult.Count; i++)
            {
                Assert.AreEqual(playersExcepted[i].Nickname, playersResult[i].Nickname);
            }
        }
    }
}
