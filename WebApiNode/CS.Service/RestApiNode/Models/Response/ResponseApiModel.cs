using System;
using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public partial class ResponseApiModel : AbstractResponseApiModel
    {
        public ResponseApiModel()
        {
            DataResponse = new DataResponseApiModel();
            ListItem = new List<ItemListApiModel>();
        }

        public long? TransactionInnerId { get; set; }

        public Decimal Amount { get; set; }

        public DataResponseApiModel DataResponse { get; set; }

        public Decimal ActualSum { get; set; }
        public Decimal ActualFee { get; set; }

        public List<EFeeItem> ExtraFee { get; set; }


        public ICollection<ItemListApiModel> ListItem { get; set; }


        public TransactionInfo TransactionInfo { get; set; }

        public TransactionInfo[] ListTransactionInfo { get; set; }
     
        public long BlockId { get; set; }

        public string TransactionId { get; set; }
    }
}
