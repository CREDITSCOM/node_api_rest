using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;
using NodeAPIClient.Models;
using NodeAPIClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

            const long MaxBlockRangeSize = 100L;
            
            if(request.BeginSequence == 0)
            {
                request.BeginSequence = request.EndSequence;
            }
            if(request.EndSequence == 0)
            {
                request.EndSequence = request.BeginSequence;
            }

            if (request.EndSequence > request.BeginSequence && request.EndSequence - request.BeginSequence >= MaxBlockRangeSize)
            {
                request.EndSequence = request.BeginSequence + MaxBlockRangeSize;
            }
            else if(request.BeginSequence > request.EndSequence && request.BeginSequence - request.EndSequence >= MaxBlockRangeSize)
            {
                request.EndSequence = request.BeginSequence - MaxBlockRangeSize;
            }

            var blocksList = instance.GetBlocksRange(request.BeginSequence, request.EndSequence);
            if (!blocksList.Success)
                return blocksList.Message;
         
            ResponseBlocksModel result = new ResponseBlocksModel();
            result.Success = true;
            GetBlockService.BlockContent content = new GetBlockService.BlockContent();
            content.ConsensusInfo = request.ConsensusInfo;
            content.Transactions = request.Transactions;
            content.ContractsApproval = request.ContractsApproval;
            content.Signatures = request.Signatures;
            content.Hashes = request.Hashes;
            return GetBlockService.ToJson(blocksList.Blocks.Select(x => x.Block).ToList(), content, false);
        }
    }
}
