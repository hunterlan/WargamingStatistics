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
            WargaminAPI operations = new WargaminAPI();
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
                playersResult.Add(operations.FindPlayer(nicknames[i]));
            }

            for(int i = 0; i < playersResult.Count; i++)
            {
                Assert.AreEqual(playersExcepted[i].Nickname, playersResult[i].Nickname);
            }
        }

        [TestMethod]
        public void TestConvertFromTimestamp()
        {
            WargaminAPI operations = new WargaminAPI();
            int timestamp = 1573338539;
            DateTime expectedTime = new DateTime(2019, 11, 9, 22, 28, 59);
            DateTime resultTime = operations.ConvertFromTimestamp(timestamp);
            Assert.AreEqual(expectedTime, resultTime);
        }
    }
}
