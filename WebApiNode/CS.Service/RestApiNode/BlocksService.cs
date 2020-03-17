using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;
using NodeAPIClient.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode
{
    public class BlocksService
    {
        public IConfiguration Configuration { get; }

        public BlocksService(IConfiguration configuration)
        {
            Configuration = configuration;
            instance = new NodeAPIClient.Services.GetBlockService();
        }

        NodeAPIClient.Services.GetBlockService instance;

        /// <summary>   Returns the range of blocks as requested in the given arguments. </summary>
        ///
        /// <remarks>   Aae, 17.03.2020. </remarks>
        ///
        /// <param name="model">    The request model. </param>
        ///
        /// <returns>   The blocks range serialized into JSON string. </returns>

        public string GetBlocksRange(RequestBlocksModel model)
        {
            string config_addr = "";
            string config_port = "";
            string config_timeout = "";

            string network = model.NetworkAlias;
            if (!string.IsNullOrWhiteSpace(network))
            {
                int srvnum = 1;
                string config_path = $"ApiNode:servers:{network}:s{srvnum}:";
                config_addr = Configuration[config_path + "Ip"];
                config_port = Configuration[config_path + "ExecutorPort"];
                config_timeout = Configuration[config_path + "TimeOut"];
            }
            else
            {
                config_addr = model.NetworkIp;
                config_port = model.NetworkPort;
                if (string.IsNullOrWhiteSpace(config_addr))
                {
                    config_addr = "127.0.0.1";
                }
            }
            ushort port;
            int timeout;
            if(!ushort.TryParse(config_port, out port))
            {
                port = 9070;
            }
            if(!int.TryParse(config_timeout, out timeout))
            {
                timeout = 60000;
            }
            instance.RemoteNodeIp = config_addr;
            instance.RemoteNodePort = port;
            instance.RequestTimeout = timeout;

            var blocks = instance.GetBlocksRange(model.BeginSequence, model.EndSequence);
         
            ResponseBlocksModel result = new ResponseBlocksModel();
            result.Success = true;
            GetBlockService.BlockContent content = new GetBlockService.BlockContent();
            content.ConsensusInfo = model.ConsensusInfo;
            content.Transactions = model.Transactions;
            content.ContractsApproval = model.ContractsApproval;
            content.Signatures = model.Signatures;
            content.Hashes = model.Hashes;
            return GetBlockService.ToJson(blocks, content, false);
        }
    }
}
