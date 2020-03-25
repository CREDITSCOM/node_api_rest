using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public abstract class AbstractRequestApiModel
    {
        public string AuthKey { get; set; }


        public string NetworkAlias { get; set; }


        public string NetworkIp { get; set; }

        public string NetworkPort { get; set; }

        public string PublicKey { get; set; }

    }

    public abstract class RequestTransactionApiModel : AbstractRequestApiModel
    {
        public string TransactionId { get; set; }
    }

    public abstract class RequestSourceCodeApiModel : RequestTransactionApiModel
    {
        public bool CompressString { get; set; }
    }
}

