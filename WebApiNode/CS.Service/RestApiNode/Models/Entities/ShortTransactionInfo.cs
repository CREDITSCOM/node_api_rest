using System;

namespace CS.Service.RestApiNode.Models
{
    public class ShortTransactionInfo
    {
        public decimal Amount { get; set; }
        public UInt16 Currency { get; set; }
        public decimal Fee { get; set; }
        public string TransactionId { get; set; }
        /// <summary>
        /// Base58
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Base58
        /// </summary>
        public string Target { get; set; }
        public DateTime TimeCreation { get; set; }
        public Byte Type { get; set; }
        public string UserData { get; set; }
    }
}
