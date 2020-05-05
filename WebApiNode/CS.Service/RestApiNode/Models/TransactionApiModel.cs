using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public partial class TransactionApiModel
    {
        public string Id { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public DateTime Time { get; set; }
        public string Sum { get; set; }
        public string Fee { get; set; }
        public string Currency { get; set; }
        public long InnerId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
