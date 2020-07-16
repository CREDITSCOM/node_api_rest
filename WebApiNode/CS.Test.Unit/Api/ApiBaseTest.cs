using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CS.Test.Unit.Api
{
    public class ApiBaseTest
    {
        private static string baseUrl = "http://localhost:60476/api/monitor/";

        private class BaseNet
        {
            public static string NetworkAlias { get; set; }

            public static string NetworkIp { get; set; }

            public static string NetworkPort { get; set; }

            public static string PublicKey { get; set; }

            public static long Limit { get; set; }

            public static long Offset { get; set; }
        }

        private class TestNetRemoteNet : BaseNet
        {
            public TestNetRemoteNet()
            {
                NetworkAlias = "TestNet";
                NetworkIp = "68.183.230.109";
                NetworkPort = "9090";
                PublicKey = "JfFyPGxxN7ygUNfM5if5TfGmjGuJ1BaZqrGsTKPsWnZ";
                Limit = 1000;
                Offset = 0;
            }
        }

        private TestNetRemoteNet testNetRemoteNet = new TestNetRemoteNet();

        private IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
                            {
                                {"ApiNode:servers:TestNet:s1:Ip", BaseNet.NetworkIp},
                                {"ApiNode:servers:TestNet:s1:Port", BaseNet.NetworkPort},
                                {"ApiNode:servers:TestNet:s1:TimeOut", "60000"}
                            }).Build();


        internal RequestKeyApiModel requestKeyApi = new RequestKeyApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            PublicKey = BaseNet.PublicKey
        };

        internal RequestTransactionApiModel requestTranApi = new RequestTransactionApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
        };

        internal RequestTransactionsApiModel requestTransApi = new RequestTransactionsApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            PublicKey = BaseNet.PublicKey,
            Limit = BaseNet.Limit,
            Offset = BaseNet.Offset
        };

        internal RequestFilteredListModel requestFilterApi = new RequestFilteredListModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            Queries = new List<SingleQueryModel>()
        };

        internal RequestTokensApiModel requestTokenApi = new RequestTokensApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            PublicKey = BaseNet.PublicKey
        };

        internal RequestGetterApiModel requestGetterApi = new RequestGetterApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort
        };

        internal RequestBlocksModel requestBlocksApi = new RequestBlocksModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort
        };

        internal RequestNodeInfoModel requestNodeInfoApi = new RequestNodeInfoModel()
        {
        };

        internal void MethodHttpTest<TRequest, TResponse>(
            TRequest request,
            string method)
            where TResponse : AbstractResponseApiModel
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var responseRaw = client.PostAsync(baseUrl + method, data).Result;

                var responseString = responseRaw.Content.ReadAsStringAsync().Result;
                Assert.IsTrue(!string.IsNullOrWhiteSpace(responseString));

                var responseObj = JsonConvert.DeserializeObject<TResponse>(responseString);
                Assert.IsNotNull(responseObj);

                Debug.WriteLine(responseString);
                Assert.IsTrue(responseObj.Success);
            }
        }

        internal void MethodTest<TRequest>(TRequest request)
        {
            var service = new MonitorService(config);
            var response = service.GetBalance(requestKeyApi);

            Debug.WriteLine(response.Balance);
            Assert.IsTrue(response.Success);
        }
    }
}
