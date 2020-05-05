using System;
using System.Collections.Generic;
using System.Text;

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

        public string MessageError { get; set; }
    }

}
