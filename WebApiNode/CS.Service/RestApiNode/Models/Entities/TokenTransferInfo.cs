using System;
using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class TokenTransferInfo
    {
        public TokenTransferInfo()
        {
            ExtraFee = new List<EFeeItem>();
        }
        public string TokenAddress { get; set; }

        public string TokenCode { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public Decimal TokenAmount { get; set; }

        public decimal Fee { get; set; }

        public List<EFeeItem> ExtraFee { get; set; }

        public string TransferInitiator { get; set; }

        public string TransactionID { get; set; }

        public DateTime TimeCreation { get; set; }

        public string UserData { get; set; }
    }
}
