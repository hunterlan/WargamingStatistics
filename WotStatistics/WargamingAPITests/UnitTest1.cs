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
            var privateObject = new PrivateObject(operations);
            int timestamp = 1573338539;
            DateTime expectedTime = new DateTime(2019, 11, 9, 22, 28, 59);
            DateTime resultTime = (DateTime)privateObject.Invoke("ConvertFromTimestamp", timestamp);
            Assert.AreEqual(expectedTime, resultTime);
        }

        [TestMethod]
        public void TestGetNameClan()
        {
            WargaminAPI operations = new WargaminAPI();
            string idClan = "61318";
            string expectedName = "Coyotes / Alpha";

            var privateObject = new PrivateObject(operations);
            string resultName = (string)privateObject.Invoke("GetClan", idClan);

            Assert.AreEqual(expectedName, resultName);
        }
    }
}
