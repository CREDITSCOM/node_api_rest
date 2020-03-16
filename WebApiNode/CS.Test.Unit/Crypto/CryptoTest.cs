using CS.NodeApi.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NodeApi_4_3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SauceControl.Blake2Fast;
using NodeApi;

namespace CS.Test.Unit.Crypto
{
    [TestClass]
    public class CryptoTest
    {
        [TestMethod]
        public void Crypto1Test()
        {
            var sourceStr = "FeFjpcsfHErXPk5HkfVcwH6zYaRT2xNytDTeSjfuVywt";
            var sourceByte = SimpleBase.Base58.Bitcoin.Decode(sourceStr).ToArray();
            var privateByte = SimpleBase.Base58.Bitcoin.Decode("ohPH5zghdzmRDxd978r7y6r8YFoTcKm1MgW2gzik3omCuZLysjwNjTd9hnGREFyQHqhShoU4ri7q748UgdwZpzA").ToArray();


            using (var _api = BCTransactionTools.CreateContextBC("165.22.220.8", 9090, 60000))
            {
                //   WalletDataGetResult WalletData = _api._connect.WalletDataGet(sourceByte);
                //   long transactionId = WalletData.WalletData.LastTransactionId + 1;
                var transactionId = _api.WalletDataGet(sourceByte).WalletData.LastTransactionId + 1;
                //     var connect = _api._connect;
                //   for (int i = 0; i <= 19; i++)
                {

                    transactionId++;
                    Transaction transac = new Transaction
                    {
                        Id = transactionId,
                        Source = sourceByte,
                        Amount = BCTransactionTools.GetAmountByDouble_C(0.1M),
                        Fee = BCTransactionTools.EncodeFeeFromDouble(0.1),

                        // если перевод смарта то таргет публичник смарта
                        Target = SimpleBase.Base58.Bitcoin.Decode("FY8J5uSb2D3qX3iwUSvcUSGvrBGAvsrXxKxMQdFfpdmm").ToArray(),
                        // если перевод CS - то адресат назначения.
                        //"DM79n9Lbvm3XhBf1LwyBHESaRmJEJez2YiL549ArcHDC"),
                        // UserFields = model.UserData


                    };
                    transac.UserFields = new byte[5];
                    #region ДЕПЛОЙ СМАРТА (убрать if при необходимости)
                    if (1 == 2)
                    {
                        string codeSmart = "";
                        Byte[] byteSource = transac.Source;
                        //code
                        var byteCodes =
                            _api.SmartContractCompile(codeSmart);
                        if (byteCodes.Status.Code > 0)
                        {
                            throw new Exception(byteCodes.Status.Message);
                        }
                        foreach (var item in byteCodes.ByteCodeObjects)
                        {
                            byteSource.Concat(item.ByteCode);
                        }
                        transac.Target = Blake2s.ComputeHash(byteSource);
                        transac.SmartContract = new SmartContractInvocation()
                        {
                            SmartContractDeploy = new SmartContractDeploy()
                            {
                                SourceCode = codeSmart,
                                ByteCodeObjects = byteCodes.ByteCodeObjects
                            }

                        };
                    }

                    #endregion


                    #region ВЫЗОВ МЕТОДА СМАРТА (убрать if при необходимости)

                    if (1 == 2)
                    {
                        transac.SmartContract = new SmartContractInvocation()
                        {
                            Method = "transfer",
                            Params = new List<Variant>() {
                              new Variant(){
                                  // адресат перевода
                                   V_string = "DM79n9Lbvm3XhBf1LwyBHESaRmJEJez2YiL549ArcHDC"
                             //      { Value = "DM79n9Lbvm3XhBf1LwyBHESaRmJEJez2YiL549ArcHDC", Key = "STRING" },
                           //           new Credtis_Api_Connect.Model.ParamsCreateModel { Value = "0.5", Key = "STRING" }

                              },
                              new Variant()
                              {
                                   V_string = "0.5"

                              }
                            }

                        };
                    }

                    #endregion



                    transac.Currency = 1;
                    Console.WriteLine(SimpleBase.Base58.Bitcoin.Encode(BCTransactionTools.CreateByteHashByTransaction(transac)));

                    Rebex.Security.Cryptography.Ed25519 crypt = new Rebex.Security.Cryptography.Ed25519();
                    crypt.FromPrivateKey(privateByte);
                   // var b58 = new SimpleBase.Base58();
                 

                    transac.Signature = crypt
                        .SignMessage(BCTransactionTools.CreateByteHashByTransaction(transac));

                    Console.WriteLine();
                    Console.WriteLine(SimpleBase.Base58.Bitcoin.Encode(transac.Signature));
                  //  Console.WriteLine(BitConverter.ToString(transac.Signature)); // работает в старом вариант


                    //  var  _api1 = BCTransactionTools.CreateContextBC("165.22.220.8", 9090, 6000);
                    //   var res = _api.TransactionFlow(transac);
                    Console.WriteLine();
                  //  Console.WriteLine(BitConverter.ToString(BCTransactionTools.CreateByteHashByTransaction(transac)).Replace("-",""));
                    //      = Res1;
                }

            }
        }
    }
}
