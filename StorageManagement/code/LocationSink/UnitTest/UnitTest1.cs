using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Double d = 10;
            BoxTest(d);
            Assert.AreEqual(d, 11);
        }


        public void BoxTest(Double d)
        {
            d = d + 1;
        }
    }
}
