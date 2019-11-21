using Microsoft.VisualStudio.TestTools.UnitTesting;
using WargaminAPI;

namespace Tests
{
    [TestClass]
    public class ClassTestEnigma
    {
        [TestMethod]
        public void TestEncrypting()
        {
            string test = "Hello, World!";
            string key = "test";
            string expectedEncrypt = "rwl71YCs4yz5M7j+YjmNCEswFz2tTObWabwC/80RQtWReTjpV42sG58n8iYLkI/d";
            string result = Enigma.Encrypt(test, key);

            Assert.AreEqual(expectedEncrypt, result);
        }
        [TestMethod]
        public void TestDecrypting()
        {

        }
    }
}
