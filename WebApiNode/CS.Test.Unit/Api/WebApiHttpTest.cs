using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using CS.Service.RestApiNode.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace CS.Test.Unit.Api
{
    [TestClass]
    public class WebApiHttpTest
    {
        private static string baseUrl = "http://localhost:60476/api/monitor/";

        private RequestKeyApiModel requestKeyApi = new RequestKeyApiModel()
        {
            NetworkAlias = "TestNet",
            NetworkIp = "68.183.230.109",
            NetworkPort = "9090",
            PublicKey = "H5ptdUUfjJBGiK2X3gN2EzNYxituCUUnXv2tiMdQKP3b"
        };

        private RequestTransactionApiModel requestTranApi = new RequestTransactionApiModel()
        {
            NetworkAlias = "TestNet",
            NetworkIp = "68.183.230.109",
            NetworkPort = "9090"
        };

        private RequestFilteredListModel requestFilterApi = new RequestFilteredListModel()
        {
            NetworkAlias = "TestNet",
            NetworkIp = "68.183.230.109",
            NetworkPort = "9090"
        };

        private void MethodHttpTest(object request, string method)
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var responseRaw = client.PostAsync(baseUrl + method, data).Result;

                var responseString = responseRaw.Content.ReadAsStringAsync().Result;
                Assert.IsTrue(!string.IsNullOrWhiteSpace(responseString));

                var responseObj = JsonConvert.DeserializeObject<BalanceResponseApiModel>(responseString);
                Assert.IsNotNull(responseObj);

                Debug.WriteLine(responseString);

                Assert.IsTrue(responseObj.Success);
            }
        }

        [TestMethod]
        public void GetBalanceHttpTest()
        {
            requestKeyApi.MethodApi = "GetBalance";
            MethodHttpTest(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetWalletInfoHttpTest()
        {
            requestKeyApi.MethodApi = "GetWalletInfo";
            MethodHttpTest(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetContractHttpTest()
        {
            requestKeyApi.MethodApi = "GetContract";
            MethodHttpTest(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetTransactionInfoHttpTest()
        {
            requestTranApi.MethodApi = "GetTransactionInfo";
            requestTranApi.TransactionId = "0";
            MethodHttpTest(requestTranApi, requestTranApi.MethodApi);
        }

        [TestMethod]
        public void GetFilteredTransactionsListHttpTest()
        {
            requestFilterApi.MethodApi = "GetFilteredTransactionsList";
            requestFilterApi.Flagg = "out";
            MethodHttpTest(requestFilterApi, requestFilterApi.MethodApi);
        }
    }
}
