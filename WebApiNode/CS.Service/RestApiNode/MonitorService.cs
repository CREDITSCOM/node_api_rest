using CS.Service.RestApiNode.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Linq;
using CS.NodeApi.Api;
using NodeApi;


namespace CS.Service.RestApiNode
{

    public partial class MonitorService : TransactionService
    {

        public MonitorService(IConfiguration configuration) : base(configuration)
        {
        }

        public BalanceResponseApiModel GetBalance(RequestKeyApiModel model)
        {
            var response = new BalanceResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var publicKeyByte = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey);

                var balanceCS = client.WalletBalanceGet(publicKeyByte.ToArray());
                var amount = BCTransactionTools.GetDecimalByAmount(balanceCS.Balance);
                decimal delOut = 0;
                decimal delIn = 0;
                if (balanceCS.Delegated != null)
                {
                    if (balanceCS.Delegated.Outgoing != null)
                    {
                        delOut = BCTransactionTools.GetDecimalByAmount(balanceCS.Delegated.Outgoing);
                    }
                    if (balanceCS.Delegated.Outgoing != null)
                    {
                        delIn = BCTransactionTools.GetDecimalByAmount(balanceCS.Delegated.Incoming);
                    }
                }

                response.Balance = amount;
                response.DelegatedOut = delOut;
                response.DelegatedIn = delIn;

                if (balanceCS.Status.Code > 0)
                    throw new Exception(balanceCS.Status.Message);

                var balanceToken = client.TokenBalancesGet(publicKeyByte.ToArray());
                if (balanceToken.Status.Code > 0)
                    throw new Exception(balanceToken.Status.Message);

                foreach (var item in balanceToken.Balances)
                {
                    if (String.IsNullOrEmpty(item.Balance))
                        continue;
                     
                    var item1 = new Token();
                    item1.Alias = item.Code;
                    item1.Amount = Decimal.Parse(item.Balance, CultureInfo.InvariantCulture);
                    item1.PublicKey = SimpleBase.Base58.Bitcoin.Encode(item.Token);
                    item1.Name = item.Name;

                    response.Tokens.Add(
                        item1
                    );
                }
            }

            return response;
        }

        public TransactionInfo GetTransaction(RequestTransactionApiModel model)
        {
            var response = new TransactionInfo();

            using (var client = GetClientByModel(model))
            {
                var trId = BCTransactionTools.GetTransactionIdByStr(model.TransactionId);//GetTransactionId(id);
                var tr = client.TransactionGet(trId);
                var tInfo = ToTransactionInfo(0, trId, tr.Transaction.Trxn);
                tInfo.Found = tr.Found;
                response = tInfo;
            }

            return response;
        }


        public FilteredTransactionsResponseModel GetFilteredTransactions(RequestFilteredListModel model)
        {
            var res = new FilteredTransactionsResponseModel();
            using (var client = GetClientByModel(model))
            {
                var args = new API.FilteredTransactionsListGet_args();
                args.GeneralQuery = new TransactionsQuery();
                args.GeneralQuery.Flag = Convert.ToInt16((model.Flagg == "in") ? 1 : (model.Flagg == "out" ? 2 : 3));
                args.GeneralQuery.Queries = new List<SingleQuery>();
                foreach(var qry in model.Queries)
                {
                    //var nq = new SingleQuery();
                    var sq = new SingleQuery();
                    sq.FromId = BCTransactionTools.GetTransactionIdByStr(qry.FromId);
                    sq.RequestedAddress = SimpleBase.Base58.Bitcoin.Decode(qry.Address).ToArray();
                    args.GeneralQuery.Queries.Add(sq);
                }

                var qResult = client.FilteredTransactionsListGet(args.GeneralQuery);
                if(model.Queries.Count() != qResult.QueryResponse.Count)
                {
                    throw new Exception("Query and response lists are not syncronized");
                }

                foreach(var qRep in qResult.QueryResponse)
                {
                    var qRes = new QueryResponseItem();
                    qRes.QueryAddress = SimpleBase.Base58.Bitcoin.Encode(qRep.RequestedAddress);
                    foreach(var tr in qRep.Transactions)
                    {
                        var newTr = new ShortTransactionInfo();
                        newTr.Amount = BCTransactionTools.GetDecimalByAmount(tr.Amount);
                        newTr.Currency = Convert.ToUInt16(tr.Currency);
                        newTr.Fee = Convert.ToDecimal(Utils.ConvertCommission(tr.Fee.Commission));
                        if(model.Flagg == "in")
                        {
                            newTr.Source = SimpleBase.Base58.Bitcoin.Encode(tr.Source);
                        }
                        else if (model.Flagg == "out")
                        {
                            newTr.Target = SimpleBase.Base58.Bitcoin.Encode(tr.Target);
                        }
                        else
                        {
                            newTr.Source = SimpleBase.Base58.Bitcoin.Encode(tr.Source);
                            newTr.Target = SimpleBase.Base58.Bitcoin.Encode(tr.Target);
                        }
                        //newTr.TimeCreation = 
                        newTr.TransactionId = Convert.ToString(tr.Id.PoolSeq) + "." + Convert.ToString(tr.Id.Index);
                        //newTr.Type =
                        qRes.Transactions.Add(newTr);
                    }
                    res.QuerieResponses.Add(qRes);
                }
            }
            return res;
        }
        public SmartSourceCode GetContractByAddress(RequestKeyApiModel model)
        {
            var response = new SmartSourceCode();

            using (var client = GetClientByModel(model))
            {
                var publicKeyByte = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey);
                var result = client.SmartContractGet(publicKeyByte.ToArray());
                if (result.SmartContract != null && result.SmartContract.SmartContractDeploy != null)
                {
                    response.SourceString = result.SmartContract.SmartContractDeploy.SourceCode;
                }
            }

            return response;
        }


        public SmartContractMethodsModel GetContractMethods(RequestKeyApiModel model)
        {
            var response = new SmartContractMethodsModel();

            using (var client = GetClientByModel(model))
            {
                var publicKeyByte = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey);
                var preResult = client.SmartContractGet(publicKeyByte.ToArray());
                if (preResult.SmartContract != null && preResult.SmartContract.SmartContractDeploy != null)
                {
                    //response.SourceString = result.SmartContract.SmartContractDeploy.SourceCode;
                    var finalResult = client.ContractAllMethodsGet(preResult.SmartContract.SmartContractDeploy.ByteCodeObjects);
                    foreach (var met in finalResult.Methods)
                    {
                        var method = new ContractMethod();
                        method.Name = met.Name;
                        method.ReturnType = met.ReturnType;
                        foreach (var arg in met.Arguments)
                        {
                            var ar = new ModelMethodArgument();
                            ar.Name = arg.Name;
                            ar.Type = arg.Type;
                            foreach (var ann in arg.Annotations)
                            {
                                var an = new ModelAnnotation();
                                an.Name = ann.Name;
                                an.Arguments = ann.Arguments;
                                ar.Annotations.Add(an);
                            }
                            method.Arguments.Add(ar);
                        }
                        foreach (var ann in met.Annotations)
                        {
                            var an = new ModelAnnotation();
                            an.Name = ann.Name;
                            an.Arguments = ann.Arguments;
                            method.Annotations.Add(an);
                        }
                        response.Methods.Add(method);
                    }
                }
            }

            return response;
        }

        public ContractValidationResponse ValidateContract(RequestContractValidationModel model)
        {
            var response = new ContractValidationResponse();

            using (var client = GetClientByModel(model))
            {
                var result = client.SmartContractCompile(model.SourceString);
                if (result.ByteCodeObjects != null)
                {
                    response.Deploy = new ContractDeploy();
                }
                foreach (var a in result.ByteCodeObjects)
                {
                    var bc = new BCObject();
                    bc.Name = a.Name;
                    bc.ByteCode = a.ByteCode;

                    response.Deploy.ByteCodeObjects.Add(bc);
                }
                response.Deploy.SourceCode = model.SourceString;
                response.TokenStandard = result.TokenStandard;
            }

            return response;
        }
        public WalletTransactionsResponseApiModel GetWalletTransactions(RequestTransactionsApiModel model)
        {
            var response = new WalletTransactionsResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var pKey = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey).ToArray();
                var trxs = client.TransactionsGet(pKey, model.Offset, model.Limit);
                foreach(var tr in trxs.Transactions)
                {
                    response.Transactions.Add(ApiToShorttransaction(tr));
                }
            }

            return response;
        }

        public ResponseApiModel GetListTransactionByBlockId(RequestGetterApiModel model)
        {
            var response = new ResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                bool roundFee = false;
                var result = client.PoolTransactionsGet(model.BlockId, model.Offset, model.Limit);
                var listResult = result.Transactions.Select((t, i) => ToTransactionInfo(i + model.Offset + 1, t.Id, t.Trxn, roundFee)).ToArray();
                response.BlockId = model.BlockId;
                response.ListTransactionInfo = listResult;
            }

            return response;
        }


        public WalletDataResponseApiModel GetWalletData(RequestKeyApiModel model)
        {
            var response = new WalletDataResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var publicKeyByte = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey);
                var result = client.WalletDataGet(publicKeyByte.ToArray());
 

                response.Balance = BCTransactionTools.GetDecimalByAmount(result.WalletData.Balance);
                response.LastTransaction = result.WalletData.LastTransactionId;
                var dStr = new DelegatedStructure();
                if (result.WalletData.Delegated != null) {
                    dStr.Incoming = BCTransactionTools.GetDecimalByAmount(result.WalletData.Delegated.Incoming);
                    dStr.Outgoing = BCTransactionTools.GetDecimalByAmount(result.WalletData.Delegated.Outgoing);
                    foreach (var it in result.WalletData.Delegated.Donors)
                    {
                        var item = new DelegatedInfo();
                        item.PublicKey = SimpleBase.Base58.Bitcoin.Encode(it.Wallet);
                        item.Sum = BCTransactionTools.GetDecimalByAmount(it.Sum);
                        item.ValidUntil = it.ValidUntil;
                        dStr.Donors.Add(item);
                    }
                    foreach (var it in result.WalletData.Delegated.Recipients)
                    {
                        var item = new DelegatedInfo();
                        item.PublicKey = SimpleBase.Base58.Bitcoin.Encode(it.Wallet);
                        item.Sum = BCTransactionTools.GetDecimalByAmount(it.Sum);
                        item.ValidUntil = it.ValidUntil;
                        dStr.Recipients.Add(item);
                    }
                }
                response.Delegated = dStr;
            }

            return response;
        }


        public ResponseApiModel GetLastBlockId(RequestGetterApiModel model)
        {
            var response = new ResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var res = client.SyncStateGet();
                response.BlockId = res.LastBlock;
            }

            return response;
        }

        private string GetTokenStd(int tokenStandard)
        {
            switch (tokenStandard)
            {
                case 0: return "Not a token";
                case 1: return "Basic";
                case 2: return "Extended";
                case 3: return "Basic.v1";
                default: return tokenStandard.ToString();
            }
        }

        public ContractInfo ToContractInfo(SmartContract sc, SmartContractDataResult data, int index = 0)
        {
            var cInfo = new ContractInfo
            {
                Index = index,
                Address = SimpleBase.Base58.Bitcoin.Encode(sc.Address),
                SourceCode = Utils.FormatSrc(sc.SmartContractDeploy.SourceCode),
                TokenStandard = GetTokenStd(sc.SmartContractDeploy.TokenStandard),
                IsToken = sc.SmartContractDeploy.TokenStandard > 0,
                HashState = sc.SmartContractDeploy.HashState,
                ByteCodeLen = sc.SmartContractDeploy.ByteCodeObjects?.Sum(b => b.ByteCode.Length) ?? 0,
                Deployer = SimpleBase.Base58.Bitcoin.Encode(sc.Deployer),
                CreateTime = Utils.UnixTimeStampToDateTime(sc.CreateTime),
                TxCount = sc.TransactionsCount
            };
            if (data == null) return cInfo;
            var methods = data.Methods.Select(
                m => $"{Utils.SimplifyJavaType(m.ReturnType)} {m.Name}({string.Join(", ", m.Arguments.Select(a => $"{Utils.SimplifyJavaType(a.Type)} {a.Name}"))});"
            ).ToArray();
            cInfo.Methods = string.Join('\n', methods);
            var variables = data.Variables.Select(v => $"{Utils.Type(v.Value)} {v.Key} = {Utils.VarToStr(v.Value)};").ToArray();
            cInfo.Variables = string.Join('\n', variables);

            cInfo.MethodDescriptions = data.Methods
                //.Where(m => !string.IsNullOrEmpty(m.ReturnType) && m.Name.StartsWith("get", StringComparison.InvariantCultureIgnoreCase))
                .Select(m => new
                {
                    ReturnType = Utils.SimplifyJavaType(m.ReturnType),
                    m.Name,
                    Arguments = m.Arguments.Select(a => new
                    {
                        Key = Utils.SimplifyJavaType(a.Type).ToUpper(),
                        a.Name,
                        Value = string.Empty
                    }).ToArray()
                })
                .ToArray();

            return cInfo;
        }

        //public ResponseFeeModel GetActualFee(RequestFeeModel)
        //{
        //    var res = new ResponseFeeModel();
        //    res.fee = 0.008740M;
        //}
        public ResponseApiModel GetContract(RequestKeyApiModel model)
        {
            var response = new ResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var address = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey).ToArray();

                SmartContractGetResult smartContractGetResult = client.SmartContractGet(null);
                var smartContractDataResult = client.SmartContractDataGet(address);

                var contractInfo = ToContractInfo(smartContractGetResult.SmartContract, smartContractDataResult);
                contractInfo.Found = smartContractGetResult.Status.Code == 0;
                if (contractInfo.Found && contractInfo.IsToken)
                {
                    var tokenInfo = client.TokenInfoGet(address);
                    if (tokenInfo?.Token != null)
                        contractInfo.Token = $"{tokenInfo.Token.Name} ({tokenInfo.Token.Code})";
                }

                foreach (var item in contractInfo.Methods.Split('\n'))
                {
                    response.ListItem.Add(new ItemListApiModel()
                    {
                        Name = item,
                    });
                }
            }

            return response;
        }
    }
}
