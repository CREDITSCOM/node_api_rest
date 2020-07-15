using System;
using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class TransactionInfo : AbstractResponseApiModel
    {
        public string Id { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public DateTime Time { get; set; }
        public string Value { get; set; }
        public string Fee { get; set; }
        public string Currency { get; set; }
        public long InnerId { get; set; }
        public long Index { get; set; }
        public string Status { get; set; }
        public string BlockNum
        {
            get
            {
                if (Id == null || !Id.Contains(".")) return null;
                return Id.Split(".")[0];
            }
        }
        public bool Found { get; set; }
        public string UserData { get; set; }

        public List<TxProperty> Props = new List<TxProperty>();
        public string Signature { get; set; }
        public List<TxFee> ExtraFee { get; set; }

        public Extra Bundle { get; set; }
    }
}
