using System;

namespace CS.Service.RestApiNode.Models
{
    public partial class WalletDataResponseApiModel : AbstractResponseApiModel
    {
        public WalletDataResponseApiModel()
        {
            Delegated = new DelegatedStructure();
        }

        public Decimal Balance { get; set; }

        public long LastTransaction { get; set; }

        public DelegatedStructure Delegated { get; set; }
    }
}
