using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.Test.Unit.Api
{
    [TestClass]
    public class ApiSmartContractTest
    {
        string privateKey = "4xVSMfNdGTdn32QHRoxx7GVUdRCUnqvpF5jDTxkvNzwpDNpiW27taPrQ9QjDacuH9GpU8SJA8XSR9Pb8o2H4GXp1";
        string publicKey = "9onQndywomSUr6iYKA2MS5pERcTJwEuUJys1iKNu13cH";
        string networkIp = "178.128.101.90";
        string networkPort = "9090";

        private ResponseApiModel ExecSmartContractMethod(string tokenPublicKey, string tokenMethod, IEnumerable<TokenParamsApiModel> methodParams)
        {
            var service = new TransactionService(null);
            var model = new RequestApiModel
            {
                NetworkIp = networkIp,
                NetworkPort = networkPort,
                PublicKey = publicKey,
                TokenPublicKey = tokenPublicKey,
                Amount = 0M,
                MethodApi = ApiMethodConstant.SmartMethodExecute,
                Fee = 0.1m,
                TokenMethod = tokenMethod
            };

            if(methodParams != null)
                foreach (var tokenParamsApiModel in methodParams)
                    model.TokenParams.Add(tokenParamsApiModel);

            string message = service.PackTransactionByApiModel(model).TransactionPackagedStr;//changed to model
            Assert.IsNotNull(message);

            Rebex.Security.Cryptography.Ed25519 crypt = new Rebex.Security.Cryptography.Ed25519();
            crypt.FromPrivateKey(SimpleBase.Base58.Bitcoin.Decode(privateKey).ToArray());
            model.TransactionSignature = SimpleBase.Base58.Bitcoin.Encode(
                crypt.SignMessage(SimpleBase.Base58.Bitcoin.Decode(message).ToArray())
            );

            return service.ExecuteTransaction(model);
        }

        [TestMethod]
        public void GetBlockchainTimeMillsExampleTest()
        {
            var response = ExecSmartContractMethod("8VieEB2awnWqzH56QDtsxpBh5R1sqfFbJgKtNdx6prSw", "getBlockchainTimeMillsExample", null);
            Assert.IsNotNull(response);

            Debug.WriteLine(response.FlowResult);
            Debug.WriteLine(response.Message);
        }

        [TestMethod]
        public void GetSeedExampleTest()
        {
            var response = ExecSmartContractMethod("8VieEB2awnWqzH56QDtsxpBh5R1sqfFbJgKtNdx6prSw", "getSeedExample", null);
            Assert.IsNotNull(response);

            Debug.WriteLine(response.FlowResult);
            Debug.WriteLine(response.Message);
        }

        [TestMethod]
        public void getBalanceExampleTest()
        {
            var prms = new Collection<TokenParamsApiModel>
            {
                new TokenParamsApiModel() {ValString = "9onQndywomSUr6iYKA2MS5pERcTJwEuUJys1iKNu13cH"}
            };
            var response = ExecSmartContractMethod("8VieEB2awnWqzH56QDtsxpBh5R1sqfFbJgKtNdx6prSw", "getBalanceExample", prms);
            Assert.IsNotNull(response);

            Debug.WriteLine(response.FlowResult);
            Debug.WriteLine(response.Message);
        }

        [TestMethod]
        public void sendTransactionExampleTest()
        {
            var prms = new Collection<TokenParamsApiModel>
            {
                new TokenParamsApiModel() {ValString = "H5ptdUUfjJBGiK2X3gN2EzNYxituCUUnXv2tiMdQKP3b" },
                new TokenParamsApiModel() {ValString = "9onQndywomSUr6iYKA2MS5pERcTJwEuUJys1iKNu13cH" },
                new TokenParamsApiModel() {ValDouble = 5.0 },
            };
            var response = ExecSmartContractMethod("8VieEB2awnWqzH56QDtsxpBh5R1sqfFbJgKtNdx6prSw", "sendTransactionExample", prms);
            Assert.IsNotNull(response);

            Debug.WriteLine(response.FlowResult);
            Debug.WriteLine(response.Message);
        }

        [TestMethod]
        public void invokeExternalContractExampleTest()
        {
            var prms = new Collection<TokenParamsApiModel>
            {
                new TokenParamsApiModel() {ValString = "8VieEB2awnWqzH56QDtsxpBh5R1sqfFbJgKtNdx6prSw" },
                new TokenParamsApiModel() {ValString = "getSeedExample" }
            };
            var response = ExecSmartContractMethod("8VieEB2awnWqzH56QDtsxpBh5R1sqfFbJgKtNdx6prSw", "invokeExternalContractExample", prms);
            Assert.IsNotNull(response);

            Debug.WriteLine(response.FlowResult);
            Debug.WriteLine(response.Message);
        }
    }
}
