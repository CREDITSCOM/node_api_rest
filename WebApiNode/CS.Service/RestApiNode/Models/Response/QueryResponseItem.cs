using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class QueryResponseItem
    {
        public QueryResponseItem()
        {
            Transactions = new List<ShortTransactionInfo>();
            TransfersList = new List<QueryTokenResponseItem> ();
        }
        public string QueryAddress { get; set; }

        public ICollection<ShortTransactionInfo> Transactions { get; set; }

        public ICollection<QueryTokenResponseItem> TransfersList { get; set; }
    }
}
