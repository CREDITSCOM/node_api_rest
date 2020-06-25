using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    /// <summary>
    /// Модель одиночный элемент
    /// </summary>

    public class DataResponseApiModel
    {
        public string PublicKey { get; set; }

        /// <summary>
        /// Base58
        /// </summary>
        public string TransactionPackagedStr { get; set; }

        public Decimal ActualSum { get; set; }
        public Decimal RecommendedFee { get; set; }
    }
}
