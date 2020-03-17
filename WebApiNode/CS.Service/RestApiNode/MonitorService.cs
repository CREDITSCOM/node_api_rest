using CS.Service.RestApiNode.Models;
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

        public ResponseApiModel GetBalance(RequestApiModel model)
        {
            var response = new ResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var publicKeyByte = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey);

                var balanceCS = client.WalletBalanceGet(publicKeyByte.ToArray());
                var amount = BCTransactionTools.GetDecimalByAmount(balanceCS.Balance);
                var delOut = BCTransactionTools.GetDecimalByAmount(balanceCS.Delegated.Outgoing);
                var delIn = BCTransactionTools.GetDecimalByAmount(balanceCS.Delegated.Incoming);
                response.ListItem.Add(
                    new ItemListApiModel()
                    {
                        IsCs = true,
                        Amount = amount,
                        AmountAsString = amount.ToString(CultureInfo.InvariantCulture),
                        DelegatedOut = delOut,
                        DelegatedOutAsString = delOut.ToString(CultureInfo.InvariantCulture),
                        DelegatedIn = delIn,
                        DelegatedInAsStirng = delIn.ToString(CultureInfo.InvariantCulture),

                    }
                );

                if (balanceCS.Status.Code > 0)
                    throw new Exception(balanceCS.Status.Message);

                var balanceToken = client.TokenBalancesGet(publicKeyByte.ToArray());
                if (balanceToken.Status.Code > 0)
                    throw new Exception(balanceToken.Status.Message);

                foreach (var item in balanceToken.Balances)
                {
                    if (String.IsNullOrEmpty(item.Balance))
                        continue;
                     
                    var item1 = new ItemListApiModel();
                    item1.Alias = item.Code;
                    item1.IsCs = false;
                    item1.Amount = Decimal.Parse(item.Balance, CultureInfo.InvariantCulture);
                    item1.AmountAsString = item1.Amount.ToString(CultureInfo.InvariantCulture);
                    item1.PublicKey = SimpleBase.Base58.Bitcoin.Encode(item.Token);
                    item1.Name = item.Name;

                    response.ListItem.Add(
                        item1
                    );
                }
            }

            return response;
        }

        public ResponseApiModel GetTransaction(RequestTransactionApiModel model)
        {
            var response = new ResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var trId = BCTransactionTools.GetTransactionIdByStr(model.TransactionId);//GetTransactionId(id);
                var tr = client.TransactionGet(trId);
                var tInfo = ToTransactionInfo(0, trId, tr.Transaction.Trxn);
                tInfo.Found = tr.Found;
                response.TransactionInfo = tInfo;
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


        public WalletDataResponseApiModel GetWalletData(AbstractRequestApiModel model)
        {
            var response = new WalletDataResponseApiModel();

            using (var client = GetClientByModel(model))
            {
                var publicKeyByte = SimpleBase.Base58.Bitcoin.Decode(model.PublicKey);
                var result = client.WalletDataGet(publicKeyByte.ToArray());
 

                response.Balance = BCTransactionTools.GetDecimalByAmount(result.WalletData.Balance);
                response.LastTransaction = result.WalletData.LastTransactionId;
                var dStr = new DelegatedStructure();
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

        public ResponseApiModel GetContract(RequestGetterApiModel model)
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
