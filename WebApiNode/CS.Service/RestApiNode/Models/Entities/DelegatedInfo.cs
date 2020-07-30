using System;

namespace CS.Service.RestApiNode.Models
{
    public partial class DelegatedInfo
    {
        public string PublicKey { get; set; }

        public Decimal Sum { get; set; }

        public long ValidUntil { get; set; }
    }
}
