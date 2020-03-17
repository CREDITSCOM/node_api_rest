using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode
{
    public class GetBlocksService
    {
        public IConfiguration Configuration { get; }

        public GetBlocksService(IConfiguration configuration)
        {
            Configuration = configuration;
            instance = new NodeAPIClient.Services.GetBlockService();
            instance.RemoteNodeIp = "165.22.242.197"; // do-lon4 testnet
            instance.RemoteNodePort = 9070;
        }

        NodeAPIClient.Services.GetBlockService instance;

        public ResponseBlocksApiModel GetBlocksRange(RequestBlockApiModel model)
        {
            var blocks = instance.GetBlocksRange(model.BeginSequence, model.EndSequence);
            ResponseBlocksApiModel result = new ResponseBlocksApiModel();
            return result;
        }
    }
}
