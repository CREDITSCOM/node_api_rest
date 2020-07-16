using CS.Service.RestApiNode.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Test.Unit.Api
{
    [TestClass]
    public class ApiMonitorHttpTest : ApiBaseTest
    {
        [TestMethod]
        public void GetBalanceHttpTest()
        {
            requestKeyApi.MethodApi = "GetBalance";
            MethodHttpTest<RequestKeyApiModel, BalanceResponseApiModel>(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetWalletInfoHttpTest()
        {
            requestKeyApi.MethodApi = "GetWalletInfo";
            MethodHttpTest<RequestKeyApiModel, ResponseApiModel>(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetContractHttpTest()
        {
            requestKeyApi.MethodApi = "GetContract";
            requestKeyApi.PublicKey = "127zYkv7sy8t3tYKLbmwKe41c8CtyoqmX2aP8s2J3EcG";
            MethodHttpTest<RequestKeyApiModel, ResponseApiModel>(requestKeyApi, requestKeyApi.MethodApi);
        }

        [TestMethod]
        public void GetTransactionInfoHttpTest()
        {
            requestTranApi.MethodApi = "GetTransactionInfo";
            requestTranApi.TransactionId = "12267766.1";
            MethodHttpTest<RequestTransactionApiModel, TransactionInfo>(requestTranApi, requestTranApi.MethodApi);
        }

        // the test is always failed "Invalid method name: 'FilteredTransactionsListGet !!!"
        [TestMethod]
        public void GetFilteredTransactionsListHttpTest()
        {
            requestFilterApi.MethodApi = "GetFilteredTransactionsList";
            requestFilterApi.Flagg = "out";
            MethodHttpTest<RequestFilteredListModel, FilteredTransactionsResponseModel>(requestFilterApi, requestFilterApi.MethodApi);
        }

        [TestMethod]
        public void GetTransactionsByWalletHttpTest()
        {
            requestTransApi.MethodApi = "GetTransactionsByWallet";
            MethodHttpTest<RequestTransactionsApiModel, WalletTransactionsResponseApiModel>(requestTransApi, requestTransApi.MethodApi);
        }

        [TestMethod]
        public void GetTokenBalanceHttpTest()
        {
            requestTokenApi.MethodApi = "GetTokenBalance";
            requestTokenApi.Tokens = "VFR";
            MethodHttpTest<RequestTokensApiModel, TokensResponseApiModel>(requestTokenApi, requestTokenApi.MethodApi);
        }

        [TestMethod]
        public void GetListTransactionByBlockHttpTest()
        {
            requestGetterApi.MethodApi = "GetListTransactionByBlock";
            requestGetterApi.BlockId = 143;
            MethodHttpTest<RequestGetterApiModel, ResponseApiModel>(requestGetterApi, requestGetterApi.MethodApi);
        }

        [TestMethod]
        public void GetListTransactionByLastBlockHttpTest()
        {
            requestGetterApi.MethodApi = "GetListTransactionByLastBlock";
            MethodHttpTest<RequestGetterApiModel, ResponseApiModel>(requestGetterApi, requestGetterApi.MethodApi);
        }

        // method is not implemented yet
        [TestMethod]
        public void GetTransactionByInnerIdHttpTest()
        {
            requestGetterApi.MethodApi = "GetTransactionByInnerId";
            requestGetterApi.InnerId = 1;
            MethodHttpTest<RequestGetterApiModel, ResponseApiModel>(requestGetterApi, requestGetterApi.MethodApi);
        }

        [TestMethod]
        public void GetBlocksHttpTest()
        {
            requestBlocksApi.MethodApi = "GetBlocks";
            requestBlocksApi.BeginSequence = 0;
            requestBlocksApi.EndSequence = 1000;
            MethodHttpTest<RequestBlocksModel, ResponseBlocksModel>(requestBlocksApi, requestBlocksApi.MethodApi);
        }

        // the succes depends on the monitor config
        [TestMethod]
        public void GetNodeInfoHttpTest()
        {
            requestNodeInfoApi.MethodApi = "GetNodeInfo";
            MethodHttpTest<RequestNodeInfoModel, ResponseNodeInfoModel>(requestNodeInfoApi, requestNodeInfoApi.MethodApi);
        }
    }
}
