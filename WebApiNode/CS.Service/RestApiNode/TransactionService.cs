using CS.NodeApi.Api;
using CS.Service.RestApiNode.Constants;
using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SauceControl.Blake2Fast;
using NodeApi;

namespace CS.Service.RestApiNode
{
    public class TransactionService
    {
        public IConfiguration Configuration { get; }

        public TransactionService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public API.Client InitContextBC(string aliasNetwork = "MainNet", int numServer = 1)
        {
            if (String.IsNullOrEmpty(aliasNetwork))
                aliasNetwork = "MainNet";

            string configStr = $"ApiNode:servers:{aliasNetwork}:s{numServer}:";

            Console.WriteLine(configStr + "Port");
            //Console.WriteLine(Configuration[configStr + "Port"]);
            //Console.WriteLine(Configuration[configStr + "TimeOut"]);

            var ip = Configuration[configStr + "Ip"];
            var port = Configuration[configStr + "Port"];
            var timeout = Configuration[configStr + "TimeOut"];

            return BCTransactionTools.CreateContextBC(ip, 
                Int32.Parse(port),
                Int32.Parse(timeout)
                );
        }

        public API.Client GetClientByModel(AbstractRequestApiModel model)
        {

            if(!String.IsNullOrEmpty(model.NetworkIp)&& !String.IsNullOrEmpty(model.NetworkPort))
            {
                return BCTransactionTools.CreateContextBC(model.NetworkIp,
                Int32.Parse(model.NetworkPort),
                30000
                );
            }

            var authUser = AuthDataService.ListAuthKey.FirstOrDefault(p => p.AuthKey == model.AuthKey);
            if (authUser==null)
                return null;

            return InitContextBC(model.NetworkAlias);
        }

        public  Transaction InitTransaction(RequestApiModel model)
        {
            Transaction transac = new Transaction();

            using (var client = GetClientByModel(model))
            {
                // отправитель
                var sourceByte = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey);
                transac.Source = sourceByte.ToArray();

                if (model.Fee == 0)
                {
                    Decimal res;
                    if (!string.IsNullOrWhiteSpace(model.FeeAsString) && Decimal.TryParse(model.FeeAsString.Replace(",", "."), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out res))
                        transac.Fee = BCTransactionTools.EncodeFeeFromDouble(Decimal.ToDouble(res));
                    else
                        transac.Fee = BCTransactionTools.EncodeFeeFromDouble(Decimal.ToDouble(model.Fee));
                }
                else
                {
                    transac.Fee = BCTransactionTools.EncodeFeeFromDouble(Decimal.ToDouble(model.Fee));
                }

                //transac.Fee = BCTransactionTools.EncodeFeeFromDouble(Decimal.ToDouble(model.Fee));
                
                transac.Currency = 1;
                // ЗАПРОС механизм присваивания транзакции
                transac.Id = client.WalletDataGet(sourceByte.ToArray()).WalletData.LastTransactionId + 1;
                if (!String.IsNullOrEmpty(model.UserData))
                    transac.UserFields = SimpleBase.Base58.Bitcoin.Decode(model.UserData).ToArray();
                //  transac.UserFields = Convert.FromBase64String(model.UserData);

                if (model.MethodApi == ApiMethodConstant.TransferCs)
                {
                    if (model.Amount == 0)
                    {
                        Decimal res;
                        if (Decimal.TryParse(model.AmountAsString.Replace(",", "."), NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out res))
                            transac.Amount = BCTransactionTools.GetAmountByDouble_C(res);
                        else
                            transac.Amount = BCTransactionTools.GetAmountByDouble_C(model.Amount);
                    }
                    else
                    {
                        transac.Amount = BCTransactionTools.GetAmountByDouble_C(model.Amount);
                    }

                    transac.Target = SimpleBase.Base58.Bitcoin.Decode(model.ReceiverPublicKey).ToArray();
                }
                else if (model.MethodApi == ApiMethodConstant.SmartDeploy)
                {
                    transac.Amount = BCTransactionTools.GetAmountByDouble_C(0);
                    //  Byte[] byteSource = sourceByte;
                    IEnumerable<Byte> sourceCodeByte = sourceByte.ToArray();

                    sourceCodeByte = sourceCodeByte.Concat(BitConverter.GetBytes(transac.Id).Take(6));
                    // ЗАПРОС
                    var byteCodes =
                        client.SmartContractCompile(model.SmartContractSource);
                    if (byteCodes.Status.Code > 0)
                    {
                        throw new Exception(byteCodes.Status.Message);
                    }
                    foreach (var item in byteCodes.ByteCodeObjects)
                    {
                        sourceCodeByte = sourceCodeByte.Concat(item.ByteCode);
                    }

                    transac.Target = Blake2s.ComputeHash(sourceCodeByte.ToArray());
                    transac.SmartContract = new SmartContractInvocation()
                    {
                        SmartContractDeploy = new SmartContractDeploy()
                        {
                            SourceCode = model.SmartContractSource,
                            ByteCodeObjects = byteCodes.ByteCodeObjects
                        },
                    };
                }
                else if (model.MethodApi == ApiMethodConstant.SmartMethodExecute)
                {
                    var listParam = new List<Variant>();

                    foreach (var item in model.TokenParams)
                    {
                        var param = new Variant();

                        if (item.ValBool != null)
                            param.V_boolean = item.ValBool.Value;
                        else if (item.ValDouble != null)
                            param.V_double = item.ValDouble.Value;
                        else if (item.ValInt != null)
                            param.V_int = item.ValInt.Value;
                        else if (item.ValInt != null)
                            param.V_int = item.ValInt.Value;
                        else
                            param.V_string = item.ValString;
                        listParam.Add(param);

                    }

                    transac.Amount = BCTransactionTools.GetAmountByDouble_C(0); // количество токенов фиксируем в параметрах
                    transac.Target = SimpleBase.Base58.Bitcoin.Decode(model.TokenPublicKey).ToArray(); // в таргет публичный ключ токена
                    transac.SmartContract = new SmartContractInvocation()
                    {
                        Method = model.TokenMethod,
                        Params = listParam
                    };
                }
                else if (model.MethodApi == ApiMethodConstant.TransferToken)
                {
                    transac.Amount = BCTransactionTools.GetAmountByDouble_C(0); // количество токенов фиксируем в параметрах
                    transac.Target = SimpleBase.Base58.Bitcoin.Decode(model.TokenPublicKey).ToArray(); // в таргет публичный ключ токена
                    transac.SmartContract = new SmartContractInvocation()
                    {
                        Method = "transfer",
                        Params = new List<Variant>() {
                              new Variant(){
                                  // адресат перевода
                                   V_string = model.ReceiverPublicKey
                              },
                              new Variant()
                              {
                                   V_string = model.Amount.ToString().Replace(",",".")

                              }
                            }
                    };
                }
            }

            return transac;
        }

        public string PackTransactionByApiModel(RequestApiModel model)
        {
            var transac = InitTransaction(model);
            byte[] byteData;

            if (!model.DelegateEnable && !model.DelegateDisable)
            {
                byteData = BCTransactionTools.CreateByteHashByTransaction(transac);
            }
            else
            {
                byteData = BCTransactionTools.CreateByteHashByTransactionDelegation(transac);
            }

            var res = SimpleBase.Base58.Bitcoin.Encode(byteData);

            return res;
        }

        public string PackTransactionByApiModelDelegation(RequestApiModel model)
        {
            var transac = InitTransaction(model);
            var res = SimpleBase.Base58.Bitcoin.Encode(BCTransactionTools.CreateByteHashByTransactionDelegation(transac, model.DelegateEnable, model.DelegateDisable));

            return res;
        }

        public ResponseApiModel ExecuteTransaction(RequestApiModel request, int isDelegate=0)
        {
            ResponseApiModel response = null;

            using (var client = GetClientByModel(request))
            {
                //снова инициируем транзакцию
                var transac = InitTransaction(request);
                transac.Signature = SimpleBase.Base58.Bitcoin.Decode(request.TransactionSignature).ToArray();

                if (request.DelegateEnable)
                    transac.UserFields = new byte[15] { 0, 1, 5, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
                else if (request.DelegateDisable)
                    transac.UserFields = new byte[15] { 0, 1, 5, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0 };

                TransactionFlowResult result = client.TransactionFlow(transac);
                response = new ResponseApiModel();
                response.FlowResult = result;
                if (request.MethodApi == ApiMethodConstant.SmartDeploy)
                {
                    response.DataResponse.PublicKey = SimpleBase.Base58.Bitcoin.Encode(transac.Target);
                }
                if (result.Status.Code > 0)
                {
                    response.Success = false;
                    response.MessageError = result.Status.Message;
                }
                else
                {
                    response.Success = true;
                    response.TransactionInnerId = transac.Id;
                    if (result.Id != null)
                        response.TransactionId = $"{result.Id.PoolSeq}.{result.Id.Index + 1}";
                }

                response.Success = true;
                response.TransactionInnerId = transac.Id;
            }

            return response;
        }
    }
}
