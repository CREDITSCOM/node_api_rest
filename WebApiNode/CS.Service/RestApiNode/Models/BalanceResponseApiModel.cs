using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class BalanceResponseApiModel : AbstractResponseApiModel
    {
        public BalanceResponseApiModel()
        {
            Tokens = new List<Token>();
        }

        public Decimal Balance { get; set; }


        public ICollection<Token> Tokens { get; set; }


        public Decimal DelegatedOut { get; set; }

        public Decimal DelegatedIn { get; set; }

    }
}
