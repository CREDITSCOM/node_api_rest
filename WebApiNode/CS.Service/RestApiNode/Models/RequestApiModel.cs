using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class RequestApiModel : RequestTransactionApiModel
    {
        public RequestApiModel()
        {
            TokenParams = new List<TokenParamsApiModel>();
        }


        //public string AuthKey { get; set; }


        //public string NetworkAlias { get; set; }


        //public string NetworkIp { get; set; }

        //public string NetworkPort { get; set; }

        public string PublicKey { get; set; }

      //  public string PublicKey { get; set; }


        public decimal Amount { get; set; }

        public String AmountAsString { get; set; }

        public Decimal Fee { get; set; }

        public String FeeAsString { get; set; }

        public string ReceiverPublicKey { get; set; }


        public string TokenPublicKey { get; set; }

        public string TokenMethod { get; set; }


        /// <summary>
        /// Исходный код смарт контракта - необходим при деплое
        /// </summary>
        public string SmartContractSource { get; set; }

        public ICollection<TokenParamsApiModel> TokenParams { get; set; }

        /// <summary>
        /// Base58
        /// </summary>
        public string UserData { get; set; }

        public string TransactionSignature { get; set; }

       // public string TransactionId { get; set; }


        public Boolean DelegateEnable { get; set; }

        public Boolean DelegateDisable { get; set; }

        public DateTimeOffset? DateExpired { get; set; }
    }

    public class RequestFilteredListModel : AbstractRequestApiModel
    {
        public string Flagg { get; set; }

        public IEnumerable<SingleQueryModel> Queries{ get; set; }
    }

    public class SingleTokenQueryModel
    {
        public string TokenAddress { get; set; }
        public string FromId { get; set; } 
    }

    public class SingleQueryModel
    {
        public string Address { get; set; }
        public string FromId { get; set; }

        public IEnumerable<SingleTokenQueryModel> TokenQueries { get; set; }
    }

}
