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

        public string MethodApi { get; set; }

    }

}

