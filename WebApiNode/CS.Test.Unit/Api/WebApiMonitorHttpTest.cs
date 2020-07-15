using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using CS.Service.RestApiNode.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CS.Test.Unit.Api
{
    [TestClass]
    public class WebApiMonitorHttpTest
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

        private RequestKeyApiModel requestKeyApi = new RequestKeyApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            PublicKey = BaseNet.PublicKey
        };

        private RequestTransactionApiModel requestTranApi = new RequestTransactionApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
        };

        private RequestTransactionsApiModel requestTransApi = new RequestTransactionsApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            PublicKey = BaseNet.PublicKey,
            Limit = BaseNet.Limit,
            Offset = BaseNet.Offset
        };

        private RequestFilteredListModel requestFilterApi = new RequestFilteredListModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            Queries = new List<SingleQueryModel>()
        };

        private RequestTokensApiModel requestTokenApi = new RequestTokensApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort,
            PublicKey = BaseNet.PublicKey
        };

        private RequestGetterApiModel requestGetterApi = new RequestGetterApiModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort
        };

        private RequestBlocksModel requestBlocksApi = new RequestBlocksModel()
        {
            NetworkAlias = BaseNet.NetworkAlias,
            NetworkIp = BaseNet.NetworkIp,
            NetworkPort = BaseNet.NetworkPort
        };

        private RequestNodeInfoModel requestNodeInfoApi = new RequestNodeInfoModel()
        {
        };


        private void MethodHttpTest<TModel>(object request, string method) where TModel : AbstractResponseApiModel
        {
            using (var client = new HttpClient())
            {
                var data = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var responseRaw = client.PostAsync(baseUrl + method, data).Result;

                var responseString = responseRaw.Content.ReadAsStringAsync().Result;
                Assert.IsTrue(!string.IsNullOrWhiteSpace(responseString));

                var responseObj = JsonConvert.DeserializeObject<TModel>(responseString);
                Assert.IsNotNull(responseObj);

                Debug.WriteLine(responseString);

                Assert.IsTrue(responseObj.Success);
            }
        }

        [TestMethod]
        public void GetBalanceHttpTest()
        {
            requestKeyApi.MethodApi = "GetBalance";
            MethodHttpTest<BalanceResponseApiModel>(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetWalletInfoHttpTest()
        {
            requestKeyApi.MethodApi = "GetWalletInfo";
            MethodHttpTest<ResponseApiModel>(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetContractHttpTest()
        {
            requestKeyApi.MethodApi = "GetContract";
            requestKeyApi.PublicKey = "127zYkv7sy8t3tYKLbmwKe41c8CtyoqmX2aP8s2J3EcG";
            MethodHttpTest<ResponseApiModel>(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetTransactionInfoHttpTest()
        {
            requestTranApi.MethodApi = "GetTransactionInfo";
            requestTranApi.TransactionId = "12267766.1";
            MethodHttpTest<TransactionInfo>(requestTranApi, requestTranApi.MethodApi);
        }

        // the test is always failed "Invalid method name: 'FilteredTransactionsListGet !!!"
        [TestMethod]
        public void GetFilteredTransactionsListHttpTest()
        {
            requestFilterApi.MethodApi = "GetFilteredTransactionsList";
            requestFilterApi.Flagg = "out";
            MethodHttpTest<FilteredTransactionsResponseModel>(requestFilterApi, requestFilterApi.MethodApi);
        }

        [TestMethod]
        public void GetTransactionsByWalletHttpTest()
        {
            requestTransApi.MethodApi = "GetTransactionsByWallet";
            MethodHttpTest<WalletTransactionsResponseApiModel>(requestTransApi, requestTransApi.MethodApi);
        }

        [TestMethod]
        public void GetTokenBalanceHttpTest()
        {
            requestTokenApi.MethodApi = "GetTokenBalance";
            requestTokenApi.Tokens = "VFR";
            MethodHttpTest<TokensResponseApiModel>(requestTokenApi, requestTokenApi.MethodApi);
        }

        [TestMethod]
        public void GetListTransactionByBlockHttpTest()
        {
            requestGetterApi.MethodApi = "GetListTransactionByBlock";
            requestGetterApi.BlockId = 143;
            MethodHttpTest<ResponseApiModel>(requestGetterApi, requestGetterApi.MethodApi);
        }

        [TestMethod]
        public void GetListTransactionByLastBlockHttpTest()
        {
            requestGetterApi.MethodApi = "GetListTransactionByLastBlock";
            MethodHttpTest<ResponseApiModel>(requestGetterApi, requestGetterApi.MethodApi);
        }

        // method is not implemented yet
        [TestMethod]
        public void GetTransactionByInnerIdHttpTest()
        {
            requestGetterApi.MethodApi = "GetTransactionByInnerId";
            requestGetterApi.InnerId = 1;
            MethodHttpTest<ResponseApiModel>(requestGetterApi, requestGetterApi.MethodApi);
        }

        [TestMethod]
        public void GetBlocksHttpTest()
        {
            requestBlocksApi.MethodApi = "GetBlocks";
            requestBlocksApi.BeginSequence = 0;
            requestBlocksApi.EndSequence = 1000;
            MethodHttpTest<ResponseBlocksModel>(requestBlocksApi, requestBlocksApi.MethodApi);
        }

        // the succes depends on the monitor config
        [TestMethod]
        public void GetNodeInfoHttpTest()
        {
            requestNodeInfoApi.MethodApi = "GetNodeInfo";
            MethodHttpTest<ResponseNodeInfoModel>(requestNodeInfoApi, requestNodeInfoApi.MethodApi);
        }
    }
}
