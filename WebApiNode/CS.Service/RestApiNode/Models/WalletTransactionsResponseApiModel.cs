using System;
using System.Collections.Generic;
using System.Text;

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
