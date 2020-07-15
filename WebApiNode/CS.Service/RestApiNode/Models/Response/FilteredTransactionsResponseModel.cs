using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class FilteredTransactionsResponseModel : AbstractResponseApiModel
    {
        public FilteredTransactionsResponseModel()
        {
            QuerieResponses = new List<QueryResponseItem>();
        }

        public ICollection<QueryResponseItem> QuerieResponses { get; set; }
    }
}
