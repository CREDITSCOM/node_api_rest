using CS.Service.Monitor;
using CS.Service.RestApiNode.Models;
using NodeApi;

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
                Time = Utils.UnixTimeStampToDateTime(tr.TimeCreation),
                Status = "Success",
                //  Signature = Utils.ConvertHash(tr.Signature),
                // ExtraFee = tr.ExtraFee?.Select(e => new TxFee(FormatAmount(e.Sum), e.Comment)).ToList()
            };



            return tInfo;
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
