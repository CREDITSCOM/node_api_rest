using System;

namespace CS.Service.RestApiNode.Models
{
    public class TokenParamsApiModel
    {
        public TokenParamsApiModel()
        {
            ValBool = null;
            ValDouble = null;
            ValInt = null;
            ValString = null;
        }

        public string Name { get; set; }

        public Double? ValDouble { get; set; }

        public string ValString { get; set; }

        public bool? ValBool { get; set; }

        public int? ValInt { get; set; }
    }
}
