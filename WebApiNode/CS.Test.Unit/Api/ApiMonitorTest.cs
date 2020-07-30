using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Test.Unit.Api
{
    [TestClass]
    internal class ApiMonitorTest : ApiBaseTest
    {
        [TestMethod]
        public void GetBalanceTest()
        {
            MethodTest(requestKeyApi);
        }
    }
}
