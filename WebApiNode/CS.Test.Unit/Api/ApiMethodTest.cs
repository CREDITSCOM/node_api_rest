using CS.Service.RestApiNode;
using CS.Service.RestApiNode.Constants;
using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using System.Linq;
using CS.NodeApi.Api;
using Newtonsoft.Json;

namespace CS.Test.Unit.Api
{
    /// <summary>
    /// Внутернние методы тестируем без обращения во вне.
    /// </summary>
    [TestClass]
    public class ApiMethodTest
    {
        public IConfiguration Configuration { get; }


        public ApiMethodTest()
        {
            var config = new Dictionary<string, string>
                            {
                                {"ApiNode:servers:MainNet:s1:Ip", "165.22.220.8"},
                                {"ApiNode:servers:MainNet:s1:Port", "9090"},
                                {"ApiNode:servers:MainNet:s1:TimeOut", "60000"},
                                 {"ApiNode:servers:TestNet:s1:Ip", "165.22.220.8"},
                                {"ApiNode:servers:TestNet:s1:Port", "9090"},
                                {"ApiNode:servers:TestNet:s1:TimeOut", "60000"}
                            };

            Configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(config)
//   .SetBasePath(Directory.GetCurrentDirectory())
// .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
.Build();
        }

        [TestMethod]
        public void RepeatGetBalanceTest()
        {
            for (int i = 0; i < 10; i++)
            {
                GetBalanceTest();
            }
        }

        [TestMethod]
        public void GetContractTest()
        {
            var service = new MonitorService(Configuration);
            var model = new RequestKeyApiModel();
            
            model.PublicKey = "12GfsQD7peXrxTsjyNVCJmpWWBJAYA9oKy9dGZHb5mJt";
            model.NetworkIp = "68.183.230.109";
            model.NetworkPort = "9090";
            
            var response = service.GetContract(model);
            Assert.IsNotNull(response);
            Console.WriteLine(JsonConvert.SerializeObject(response));
        }

        [TestMethod]
        public void GetBalanceTest()
        {
            var service = new MonitorService(Configuration);
            var model = new RequestKeyApiModel();
            model.PublicKey =
                //"CpXgMszs74C8ncSAyMiBEfZUhXtM6hyGBXRdXtG8aUUU";
                "H5ptdUUfjJBGiK2X3gN2EzNYxituCUUnXv2tiMdQKP3b";
            //"FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt";

            model.NetworkIp = "68.183.230.109";
            model.NetworkPort = "9090";

            model.MethodApi = ApiMethodConstant.GetBalance;
            model.NetworkAlias = /*"MainNet";*/"TestNet";
            //model.AuthKey = "87cbdd85-b2e0-4cb9-aebf-1fe87bf3afdd";
            var response = service.GetBalance(model);
            if (response.Tokens.Count > 1) Debug.WriteLine(response.Tokens.ToList()[0].Amount);
            Assert.IsNotNull(response);
            Debug.WriteLine(JsonConvert.SerializeObject(response));
        }


        [TestMethod]
        public void TransferTokenTest()
        {
            var service = new TransactionService(Configuration);
            var rand = new Random();
            for (int i = 0; i < 2; i++)
            {
                var privateKey = "ohPH5zghdzmRDxd978r7y6r8YFoTcKm1MgW2gzik3omCuZLysjwNjTd9hnGREFyQHqhShoU4ri7q748UgdwZpzA";

                var model = new RequestApiModel();
                model.PublicKey = "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt";
                model.ReceiverPublicKey = "HhhRGwgA3W5qcNFrLC3odC4GmbkQnhdEc5XPqBiRW3Wx";
                model.TokenPublicKey = "FY8J5uSb2D3qX3iwUSvcUSGvrBGAvsrXxKxMQdFfpdmm";
                model.Amount = Decimal.Parse((i + rand.Next(1, 5)).ToString());
                model.MethodApi = ApiMethodConstant.TransferToken;
                model.NetworkIp = "165.22.220.8";
                model.NetworkPort = "9090";
                model.NetworkAlias = "MainNet";
                model.Fee = 0.1m;
                string message = service.PackTransactionByApiModel(model).TransactionPackagedStr; //changed to model
                Assert.IsNotNull(message);

                Rebex.Security.Cryptography.Ed25519 crypt = new Rebex.Security.Cryptography.Ed25519();
                crypt.FromPrivateKey(SimpleBase.Base58.Bitcoin.Decode(privateKey).ToArray());
                model.TransactionSignature = SimpleBase.Base58.Bitcoin.Encode(
                      crypt.SignMessage(SimpleBase.Base58.Bitcoin.Decode(message).ToArray())
                      ); // подписываем транзакцию

                var response = service.ExecuteTransaction(model);
                Assert.IsNotNull(response);
            }
        }

        [TestMethod]
        public void FeeValueTest()
        {
            TransferCsTest(0.03m);
        }

        [TestMethod]
        public void TransferCsTest()
        {
            TransferCsTest(0.05m);
            TransferCsTest(0.07m, "0.1");
            TransferCsTest(0, "0.3");
        }

        private void TransferCsTest(decimal fee = 0.03m, string feeAsString = "")
        {
            var service = new TransactionService(Configuration);
            var privateKey = "3Ki86Y3dy8enEgM1LXL97oQ6zLnhVbjJPpWAdqhgkAh7uFab37ergRWJxyDDsa46ra3UiQXqe2rW7JrJPkekBWMs";
            var model = new RequestApiModel();
            model.PublicKey = "H5ptdUUfjJBGiK2X3gN2EzNYxituCUUnXv2tiMdQKP3b";
            model.NetworkAlias = "TestNet";
            model.ReceiverPublicKey = "9onQndywomSUr6iYKA2MS5pERcTJwEuUJys1iKNu13cH";
            model.Amount = 1m;
            model.MethodApi = ApiMethodConstant.TransferCs;
            model.Fee = fee;
            model.FeeAsString = feeAsString;

            model.NetworkIp = "68.183.230.109";
            model.NetworkPort = "9090";

            string message = service.PackTransactionByApiModel(model).TransactionPackagedStr;//changed to model
            Assert.IsNotNull(message);

            Rebex.Security.Cryptography.Ed25519 crypt = new Rebex.Security.Cryptography.Ed25519();
            crypt.FromPrivateKey(SimpleBase.Base58.Bitcoin.Decode(privateKey).ToArray());
            model.TransactionSignature = SimpleBase.Base58.Bitcoin.Encode(
                crypt.SignMessage(SimpleBase.Base58.Bitcoin.Decode(message).ToArray())
            ); // подписываем транзакцию

            var response = service.ExecuteTransaction(model);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void ExecuteDelegationTest()
        {
            var rand = new Random();
            // for (int i = 0; i < 30; i++)
            {
                var service = new TransactionService(Configuration);
                var privateKey = "ohPH5zghdzmRDxd978r7y6r8YFoTcKm1MgW2gzik3omCuZLysjwNjTd9hnGREFyQHqhShoU4ri7q748UgdwZpzA";
                // "3Ki86Y3dy8enEgM1LXL97oQ6zLnhVbjJPpWAdqhgkAh7uFab37ergRWJxyDDsa46ra3UiQXqe2rW7JrJPkekBWMs";
                var model = new RequestApiModel();
                model.PublicKey = "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt";
                model.NetworkAlias = "MainNet";
                model.AuthKey = "87cbdd85-b2e0-4cb9-aebf-1fe87bf3afdd";
                model.ReceiverPublicKey = "HhhRGwgA3W5qcNFrLC3odC4GmbkQnhdEc5XPqBiRW3Wx";//"9onQndywomSUr6iYKA2MS5pERcTJwEuUJys1iKNu13cH"; //"HhhRGwgA3W5qcNFrLC3odC4GmbkQnhdEc5XPqBiRW3Wx";// //
                model.Amount = 5m;//Decimal.Parse("0," + (i + rand.Next(1, 5)).ToString());
                model.MethodApi = ApiMethodConstant.TransferCs;
                model.Fee = 0.1m;

                // model.NetworkIp = "165.22.220.8";
                // model.NetworkPort = "9090";

                model.DelegateDisable = true;
                //   model.UserData =
                //  Convert.ToBase64String(Encoding.UTF8.GetBytes("sadf_#@!#$#;543534r42фвавафыва"));
                //Base58Check.Base58CheckEncoding.EncodePlain(Encoding.UTF8.GetBytes("{'test':'test'}"));
                //   Base58Check.Base58CheckEncoding.EncodePlain(Encoding.UTF8.GetBytes("sadf_#@!#$#;543534r42фвавафыва"));

                string message = service.PackTransactionByApiModelDelegation(model);
                Assert.IsNotNull(message);

                Rebex.Security.Cryptography.Ed25519 crypt = new Rebex.Security.Cryptography.Ed25519();
                crypt.FromPrivateKey(SimpleBase.Base58.Bitcoin.Decode(privateKey).ToArray());
                model.TransactionSignature = SimpleBase.Base58.Bitcoin.Encode(
                      crypt.SignMessage(SimpleBase.Base58.Bitcoin.Decode(message).ToArray())
                      ); // подписываем транзакцию

                var response = service.ExecuteTransaction(model, 2);


                Assert.IsNotNull(response);

            }
        }

        [TestMethod]
        public void SmartDeployTest()
        {
            var service = new TransactionService(Configuration);
            var privateKey = "ohPH5zghdzmRDxd978r7y6r8YFoTcKm1MgW2gzik3omCuZLysjwNjTd9hnGREFyQHqhShoU4ri7q748UgdwZpzA";
            var model = new RequestApiModel();
            model.PublicKey = "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt";
            // model.RecieverPublicKey = "DM79n9Lbvm3XhBf1LwyBHESaRmJEJez2YiL549ArcHDC";
            // model.TokenPublicKey = "FY8J5uSb2D3qX3iwUSvcUSGvrBGAvsrXxKxMQdFfpdmm";
            model.Amount = 0;
            model.MethodApi = ApiMethodConstant.SmartDeploy;
            model.Fee = 0.9m;
            model.SmartContractSource = GetDefaultSmartByCustomData("TS2", "TEST1", "55555");
            model.NetworkIp = "165.22.220.8";
            model.NetworkPort = "9090";
            model.NetworkAlias = "MainNet";
            string message = service.PackTransactionByApiModel(model).TransactionPackagedStr;//changed to model
            Assert.IsNotNull(message);

            Rebex.Security.Cryptography.Ed25519 crypt = new Rebex.Security.Cryptography.Ed25519();
            crypt.FromPrivateKey(SimpleBase.Base58.Bitcoin.Decode(privateKey).ToArray());
            model.TransactionSignature = SimpleBase.Base58.Bitcoin.Encode(
                crypt.SignMessage(SimpleBase.Base58.Bitcoin.Decode(message).ToArray())
            ); // подписываем транзакцию


            var response = service.ExecuteTransaction(model);


            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void SmartMethodExecuteTest()
        {
            var service = new TransactionService(Configuration);
            var privateKey = "ohPH5zghdzmRDxd978r7y6r8YFoTcKm1MgW2gzik3omCuZLysjwNjTd9hnGREFyQHqhShoU4ri7q748UgdwZpzA";
            var model = new RequestApiModel();
            model.PublicKey = "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt";
            model.TokenPublicKey = "FY8J5uSb2D3qX3iwUSvcUSGvrBGAvsrXxKxMQdFfpdmm";
            model.Amount = 0;
            model.MethodApi = ApiMethodConstant.SmartMethodExecute;
            model.Fee = 0.1m;
            model.NetworkIp = "165.22.220.8";
            model.NetworkPort = "9090";
            model.NetworkAlias = "MainNet";
            model.TokenMethod = "balanceOf";
            model.TokenParams.Add(new TokenParamsApiModel() { ValString = "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt" }); //кастомер

            string message = service.PackTransactionByApiModel(model).TransactionPackagedStr;//changed to model
            Assert.IsNotNull(message);

            Rebex.Security.Cryptography.Ed25519 crypt = new Rebex.Security.Cryptography.Ed25519();
            crypt.FromPrivateKey(SimpleBase.Base58.Bitcoin.Decode(privateKey).ToArray());
            model.TransactionSignature = SimpleBase.Base58.Bitcoin.Encode(
                crypt.SignMessage(SimpleBase.Base58.Bitcoin.Decode(message).ToArray())
            ); // подписываем транзакцию


            var response = service.ExecuteTransaction(model);


            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void AmountTest()
        {
            //var amount = AmountEx.from_double(100.123457891011121314);
            //var amount = AmountEx.from_decimal(100.123457891011121314);
            var amount = AmountEx.from_decimal(0.000000000000000123M);

        }

        public string GetDefaultSmartByCustomData(string code, string name, string amout)
        {

            string smart =
                @"

import java.math.BigDecimal;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

import com.credits.scapi.annotations.*;
import com.credits.scapi.v0.*;

import static java.math.BigDecimal.ROUND_FLOOR;
import static java.math.BigDecimal.ZERO;

public class Token" + code + @" extends SmartContract implements ExtensionStandard {

    private final String owner;
    private final BigDecimal tokenCost;
    private final int decimal;
    HashMap<String, BigDecimal> balances;
    private String name;
    private String symbol;
    private BigDecimal totalCoins;
    private HashMap<String, Map<String, BigDecimal>> allowed;
    private boolean frozen;

    public Token" + code + @"() {
        super();
        name = """ + name + @""";
        symbol = """ + code + @""";
        decimal = 3;
        tokenCost = new BigDecimal(0).setScale(decimal, ROUND_FLOOR);
        totalCoins = new BigDecimal(" + amout.ToString() + @").setScale(decimal, ROUND_FLOOR);
        owner = initiator;
        allowed = new HashMap<>();
        balances = new HashMap<>();
        balances.put(owner, new BigDecimal(" + amout.ToString() + @"L).setScale(decimal, ROUND_FLOOR));
    }

    @Override
    public int getDecimal() {
        return decimal;
    }

    @Override
    public void register() {
        balances.putIfAbsent(initiator, ZERO.setScale(decimal, ROUND_FLOOR));
    }

    @Override
    public boolean setFrozen(boolean isFrozen) {
        if (!initiator.equals(owner)) {
            throw new RuntimeException(""unable change frozen state! The wallet "" + initiator + "" is not owner"");
        }
        this.frozen = isFrozen;
        return true;
    }

    @Override
    public String getName() {
        return name;
    }

    @Override
    public String getSymbol() {
        return symbol;
    }

    @Override
    public String totalSupply() {
        return totalCoins.toString();
    }

    @Override
    public String balanceOf(String owner) {
        return getTokensBalance(owner).toString();
    }

    @Override
    public String allowance(String owner, String spender) {
        if (allowed.get(owner) == null) {
            return ""0"";
        }
        BigDecimal amount = allowed.get(owner).get(spender);
        return amount != null ? amount.toString() : ""0"";
    }

    @Override
    public boolean transfer(String to, String amount) {
        contractIsNotFrozen();
        if (!to.equals(initiator)) {
            BigDecimal decimalAmount = toBigDecimal(amount);
            BigDecimal sourceBalance = getTokensBalance(initiator);
            BigDecimal targetTokensBalance = getTokensBalance(to);
            if(targetTokensBalance == null)
            {
                targetTokensBalance = new BigDecimal(0).setScale(decimal, ROUND_FLOOR);
            }
            if (sourceBalance.compareTo(decimalAmount) < 0) {
                throw new RuntimeException(""the wallet "" + initiator + "" doesn't have enough tokens to transfer"");
            }
            balances.put(initiator, sourceBalance.subtract(decimalAmount));
            balances.put(to, targetTokensBalance.add(decimalAmount));
        }
        return true;
    }

    @Override
    public boolean transferFrom(String from, String to, String amount) {
        contractIsNotFrozen();

        if (!from.equals(to)) {
            BigDecimal sourceBalance = getTokensBalance(from);
            BigDecimal targetTokensBalance = getTokensBalance(to);
            if(targetTokensBalance == null)
            {
                targetTokensBalance = new BigDecimal(0).setScale(decimal, ROUND_FLOOR);
            }
            BigDecimal decimalAmount = toBigDecimal(amount);
            if (sourceBalance.compareTo(decimalAmount) < 0)
                throw new RuntimeException(""unable transfer tokens! The balance of "" + from + "" less then "" + amount);

            Map<String, BigDecimal> spender = allowed.get(from);
            if (spender == null || !spender.containsKey(initiator))
                throw new RuntimeException(""unable transfer tokens! The wallet "" + from + "" not allow transfer tokens for "" + to);

            BigDecimal allowTokens = spender.get(initiator);
            if (allowTokens.compareTo(decimalAmount) < 0) {
                throw new RuntimeException(""unable transfer tokens! Not enough allowed tokens. For the wallet "" + initiator + "" allow only "" + allowTokens + "" tokens"");
            }

            spender.put(initiator, allowTokens.subtract(decimalAmount));
            balances.put(from, sourceBalance.subtract(decimalAmount));
            balances.put(to, targetTokensBalance.add(decimalAmount));
        }
        return true;
    }

    @Override
    public void approve(String spender, String amount) {
        initiatorIsRegistered();
        BigDecimal decimalAmount = toBigDecimal(amount);
        Map<String, BigDecimal> initiatorSpenders = allowed.get(initiator);
        if (initiatorSpenders == null) {
            Map<String, BigDecimal> newSpender = new HashMap<>();
            newSpender.put(spender, decimalAmount);
            allowed.put(initiator, newSpender);
        } else {
            BigDecimal spenderAmount = initiatorSpenders.get(spender);
            initiatorSpenders.put(spender, spenderAmount == null ? decimalAmount : spenderAmount.add(decimalAmount));
        }
    }

    @Override
    public boolean burn(String amount) {
        contractIsNotFrozen();
        BigDecimal decimalAmount = toBigDecimal(amount);
        if (!initiator.equals(owner))
            throw new RuntimeException(""can not burn tokens! The wallet "" + initiator + "" is not owner"");
        if (totalCoins.compareTo(decimalAmount) < 0) totalCoins = ZERO;
        else totalCoins = totalCoins.subtract(decimalAmount);
        return true;
    }

    public void payable(String amount, String currency) {
        contractIsNotFrozen();
        BigDecimal decimalAmount = toBigDecimal(amount);
        if (totalCoins.compareTo(decimalAmount) < 0) throw new RuntimeException(""not enough tokes to buy"");
        balances.put(initiator, Optional.ofNullable(balances.get(initiator)).orElse(ZERO).add(decimalAmount));
        totalCoins = totalCoins.subtract(decimalAmount);
    }

    @Override
    public boolean buyTokens(String amount) {
        return false;
    }

    private void contractIsNotFrozen() {
        if (frozen) throw new RuntimeException(""unavailable action! The smart-contract is frozen"");
    }

    private void initiatorIsRegistered() {
        if (!balances.containsKey(initiator))
            throw new RuntimeException(""unavailable action! The wallet "" + initiator + "" is not registered"");
    }

    private BigDecimal toBigDecimal(String stringValue) {
        return new BigDecimal(stringValue).setScale(decimal, ROUND_FLOOR);
    }

    private BigDecimal getTokensBalance(String address) {
        return balances.get(address);
    }
}
";
            return smart;
        }
    }
}
