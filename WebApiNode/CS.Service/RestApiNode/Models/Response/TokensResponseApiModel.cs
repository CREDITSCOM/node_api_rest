using System.Collections.Generic;

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
