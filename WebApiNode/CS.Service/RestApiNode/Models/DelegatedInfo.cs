using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public partial class DelegatedInfo
    {
        public string PublicKey { get; set; }

        public Decimal Sum { get; set; }

        public long ValidUntil { get; set; }
    }
}
