using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class RequestSourceCodeApiModel : AbstractRequestApiModel
    {
        public bool Compressed { get; set; }
    }
}
