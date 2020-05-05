using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class TokensResponseApiModel : AbstractResponseApiModel
    {
        public TokensResponseApiModel()
        {
            Tokens = new List<Token>();
        }

        public ICollection<Token> Tokens { get; set; }

    }
}
