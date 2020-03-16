using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class ContractInfo
    {
        public int Index { get; set; }
        public string Address { get; set; }
        public string SourceCode { get; set; }
        public string HashState { get; set; }
        public string Methods { get; set; }
        public string Variables { get; set; }
        public int ByteCodeLen { get; set; }
        public bool Found { get; set; }
        public string Deployer { get; set; }
        public string TokenStandard { get; set; }
        public bool IsToken { get; set; }
        public string Token { get; set; }
        public DateTime CreateTime { get; set; }
        public int TxCount { get; set; }
        public object MethodDescriptions { get; set; }
    }
}
