using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public partial class WalletTransactionsResponseApiModel : AbstractResponseApiModel
    {
        public WalletTransactionsResponseApiModel()
        {
            Transactions = new List<TransactionApiModel>();
        }

        public ICollection<TransactionApiModel> Transactions { get; set; }
    }
}
