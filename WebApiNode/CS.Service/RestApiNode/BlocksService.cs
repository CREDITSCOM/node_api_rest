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
        ParseRequestService Parser { get; }

        public BlocksService(ParseRequestService provider)
        {
            Parser = provider;
            instance = new NodeAPIClient.Services.GetBlockService();
        }

        NodeAPIClient.Services.GetBlockService instance;

        /// <summary>   Returns the range of blocks as requested in the given arguments. </summary>
        ///
        /// <remarks>   Aae, 17.03.2020. </remarks>
        ///
        /// <param name="request">    The request model. </param>
        ///
        /// <returns>   The blocks range serialized into JSON string. </returns>

        public string GetBlocksRange(RequestBlocksModel request)
        {
            instance.RemoteNodeIp = Parser.GetNetworkIp(request);
            instance.RemoteNodePort = Parser.GetExecutorPort(request);
            instance.RequestTimeout = Parser.GetRequestTimeout(request);

            var blocks = instance.GetBlocksRange(request.BeginSequence, request.EndSequence);
         
            ResponseBlocksModel result = new ResponseBlocksModel();
            result.Success = true;
            GetBlockService.BlockContent content = new GetBlockService.BlockContent();
            content.ConsensusInfo = request.ConsensusInfo;
            content.Transactions = request.Transactions;
            content.ContractsApproval = request.ContractsApproval;
            content.Signatures = request.Signatures;
            content.Hashes = request.Hashes;
            return GetBlockService.ToJson(blocks, content, false);
        }
    }
}
