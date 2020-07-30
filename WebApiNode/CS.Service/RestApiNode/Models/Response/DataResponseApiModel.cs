using System;

namespace CS.Service.RestApiNode.Models
{
    public class DataResponseApiModel
    {
        public string PublicKey { get; set; }

        public string TransactionPackagedStr { get; set; }

        public Decimal ActualSum { get; set; }

        public Decimal RecommendedFee { get; set; }
        public string SmartContractResult { get; set; }
    }
}
