using System;
using System.Collections.Generic;
using System.Text;
using NodeApi;

namespace CS.Service.RestApiNode.Models
{
    class ResponseFeeModel : AbstractResponseApiModel
    {
        public Decimal fee { get; set; }
    }
}
