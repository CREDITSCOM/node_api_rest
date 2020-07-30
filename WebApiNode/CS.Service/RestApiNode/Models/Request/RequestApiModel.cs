using System;
using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class RequestApiModel : RequestTransactionApiModel
    {
        public RequestApiModel()
        {
            TokenParams = new List<TokenParamsApiModel>();
            ContractParams = new List<TokenParamsApiModel>();
        }

        public string PublicKey { get; set; }

        public decimal Amount { get; set; }

        public String AmountAsString { get; set; }

        public Decimal Fee { get; set; }

        public String FeeAsString { get; set; }

        public string ReceiverPublicKey { get; set; }


        public string TokenPublicKey { get; set; }

        public string TokenMethod { get; set; }

        public string ContractPublicKey { get; set; }

        public string ContractMethod { get; set; }

        public string SmartContractSource { get; set; }

        public ICollection<TokenParamsApiModel> TokenParams { get; set; }

        public ICollection<TokenParamsApiModel> ContractParams { get; set; }

        public string UserData { get; set; }

        public string TransactionSignature { get; set; }

        public Boolean DelegateEnable { get; set; }

        public Boolean DelegateDisable { get; set; }

        public DateTimeOffset? DateExpired { get; set; }
    }
}
