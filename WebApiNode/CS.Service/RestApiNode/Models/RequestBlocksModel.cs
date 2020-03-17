using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class RequestBlocksModel: AbstractRequestApiModel
    {
        public UInt64 BeginSequence { get; set; }

        public UInt64 EndSequence { get; set; }

        public bool ConsensusInfo { get; set; }

        public bool Transactions { get; set; }

        public bool ContractsApproval { get; set; }

        public bool Signatures { get; set; }

        public bool Hashes { get; set; }
    }

}
