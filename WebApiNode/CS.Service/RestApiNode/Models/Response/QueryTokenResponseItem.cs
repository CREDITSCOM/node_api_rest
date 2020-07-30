using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class QueryTokenResponseItem
    {
        public QueryTokenResponseItem()
        {
            Transfers = new List<TokenTransferInfo>();
        }
        public string TokenAddress { get; set; }
        public string TokenName { get; set; }
        public string TokenTiker { get; set; }

        public ICollection<TokenTransferInfo> Transfers { get; set; }
    }
}
