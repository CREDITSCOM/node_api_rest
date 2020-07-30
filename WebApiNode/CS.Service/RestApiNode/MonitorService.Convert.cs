using CS.Service.Monitor;
using CS.Service.RestApiNode.Models;
using NodeApi;
using System;

namespace CS.Service.RestApiNode
{
    public partial class MonitorService
    {
        public TransactionInfo ToTransactionInfo(long idx, TransactionId id, Transaction tr, bool roundFee = false)
        {
            var tInfo = new TransactionInfo
            {
                Index = idx,
                Id = GetTxId(id),
                Value = FormatAmount(tr.Amount),
                FromAccount = SimpleBase.Base58.Bitcoin.Encode(tr.Source),
                ToAccount = SimpleBase.Base58.Bitcoin.Encode(tr.Target),
                Currency = "CS",
                //Fee = Utils.ConvertCommission(tr.Fee.Commission, roundFee),
                Fee = Utils.FeeByIndex(tr.Fee.Commission),
                InnerId = tr.Id,
                UserData = System.Text.Encoding.UTF8.GetString(tr.UserFields), 
                Time = Utils.UnixTimeStampToDateTime(tr.TimeCreation),
                Status = "Success",//?????????????????????????????????
                Signature = Utils.ConvertHash(tr.Signature),
                Bundle = (tr.SmartContract != null || tr.SmartInfo != null) ? new Extra
                {
                    Contract = (tr.SmartContract != null) ? new SmartContractModel {
                        ForgetNewState = tr.SmartContract.ForgetNewState,
                        Execute = (tr.SmartContract.Method != null) ? new SmartContractExecuteModel
                        {
                            Method = tr.SmartContract.Method
                        } : null,
                        Deploy = (tr.SmartContract.SmartContractDeploy != null) ? new SmartContractDeployModel
                        {
                            SourceCode = tr.SmartContract.SmartContractDeploy.SourceCode,
                            TokenStandard = tr.SmartContract.SmartContractDeploy.TokenStandard,
                            HashState = tr.SmartContract.SmartContractDeploy.HashState
                        } : null

                    } : null,
                    ContractInfo = (tr.SmartInfo != null) ? new SmartInfo {
                        //SmartDeploy = null,
                        //SmartExecution = null
                    } : null
                } : null
                // ExtraFee = tr.ExtraFee?.Select(e => new TxFee(FormatAmount(e.Sum), e.Comment)).ToList()
            };
            if(tr.SmartContract!=null && tr.SmartContract.Params.Count > 0)
            {
                foreach (var a in tr.SmartContract.Params) 
                {
                    tInfo.Bundle.Contract.Execute.Params.Add(a.ToString());
                }
            }

            return tInfo;
        }

        public TransactionApiModel ApiToShorttransaction(SealedTransaction tr)
        {
            var val = Convert.ChangeType(tr.Trxn.Type, tr.Trxn.Type.GetTypeCode());
            var wtr = new TransactionApiModel
            {
                Id = GetTxId(tr.Id),
                FromAccount = SimpleBase.Base58.Bitcoin.Encode(tr.Trxn.Source),
                ToAccount = SimpleBase.Base58.Bitcoin.Encode(tr.Trxn.Target),
                Time = Utils.UnixTimeStampToDateTime(tr.Trxn.TimeCreation),
                Sum = FormatAmount(tr.Trxn.Amount),
                Fee = Utils.FeeByIndex(tr.Trxn.Fee.Commission),
                Currency = (tr.Trxn.Currency == 1) ? "CS" : "",
                InnerId = tr.Trxn.Id,
                Type = val.ToString(),
                Status = "Success"
            };

            return wtr;
        }

        private static string GetTxId(TransactionId id)
        {
            return id == null ? null : $"{id.PoolSeq}.{id.Index + 1}";
        }

        public static string FormatAmount(Amount value)
        {
            if (value == null) return string.Empty;
            if (value.Fraction == 0) return $"{value.Integral}.0";
            var fraction = value.Fraction.ToString();
            while (fraction.Length < 18)
                fraction = "0" + fraction;
            return $"{value.Integral}.{fraction.TrimEnd('0')}";
        }

        public static decimal GetAmount(Amount value)
        {
            if (value == null) return 0;
            return value.Integral + decimal.Multiply(value.Fraction, 0.000000000000000001M);
        }
    }
}
