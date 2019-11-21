using Microsoft.VisualStudio.TestTools.UnitTesting;
using WargaminAPI;
using WargaminAPI.Model;
using WargaminAPI.WoT;

namespace Tests
{
    [TestClass]
    public class ClassTestPlayer
    {
        PlayerInfo ops;
        public ClassTestPlayer()
        {
            ops = new PlayerInfo();
        }
        [TestMethod]
        public void TestFindPlayer()
        {
            string[] nicknames = { "Hunterlan2000", "_ToPkOt" };

            Player[] expectedResult =
            {
                new Player
                {
                    Nickname = nicknames[0],
                    Id = 30373360
                },
                new Player
                {
                    Nickname = nicknames[1],
                    Id = 97339715
                }
            };

            Player[] realResult = new Player[2];
            for(int i = 0; i < 2; i++)
            {
                realResult[i] = ops.FindPlayer(nicknames[i]);
            }

            for(int i = 0; i < 2; i++)
            {
                Assert.AreEqual(expectedResult[i], realResult[i]);
            }
        }
    }
}
