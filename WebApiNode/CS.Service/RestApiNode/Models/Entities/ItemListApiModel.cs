using System;

namespace CS.Service.RestApiNode.Models
{
    public class ItemListApiModel
    {
        public string PublicKey { get; set; }

        public bool IsCs { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public Decimal Amount { get; set; }
    }
}
