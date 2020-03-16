using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CS.Test.Unit
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            for (int i=0; i<10; i++)
            Console.WriteLine(Guid.NewGuid().ToString());
        }
    }
}
