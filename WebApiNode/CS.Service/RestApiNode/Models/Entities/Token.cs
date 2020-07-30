using System;

namespace CS.Service.RestApiNode.Models
{
    public class Token
    {
        public string PublicKey { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public Decimal Amount { get; set; }
    }
}
