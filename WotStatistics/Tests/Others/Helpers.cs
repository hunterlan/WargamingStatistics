using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WargaminAPI;
using WargaminAPI.Model;
using WargaminAPI.WoT;

namespace Tests
{
    [TestClass]
    public class Helpers
    {
        string Encrypted = "rwl71YCs4yz5M7j+YjmNCEswFz2tTObWabwC/80RQtWReTjpV42sG58n8iYLkI/d";
        string sourceText = "Hello, World!";
        string key = "test";
        [TestMethod]
        public void TestDecrypting()
        {
            string result = Enigma.Decrypt(Encrypted, key);

            Assert.AreEqual(sourceText, result);
        }
    }

}
