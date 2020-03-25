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

    public class RequestContractValidationModel : AbstractRequestApiModel
    {
        public string SourceString { get; set; }
    }

    public class RequestTransactionApiModel : RequestSourceCodeApiModel
    {
        public string TransactionId { get; set; }
    }

    public class RequestSourceCodeApiModel : AbstractRequestApiModel
    {
        public bool CompressString { get; set; }
    }

    public class RequestKeyApiModel : RequestSourceCodeApiModel
    {
        public string PublicKey { get; set; }
    }

    public class TokenParamsApiModel
    {
        public TokenParamsApiModel()
        {
            ValBool = null;
            ValDouble = null;
            ValInt = null;
            ValString = null;
        }

        public string Name { get; set; }


        public Double? ValDouble { get; set; }

        public string ValString { get; set; }

        public bool? ValBool { get; set; }

        public int? ValInt { get; set; }


    }



}
