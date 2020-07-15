namespace CS.Service.RestApiNode.Models
{
    public class SmartContractDeployModel
    {
        public string SourceCode { get; set; }
        public int TokenStandard { get; set; }
        
        public string HashState { get; set; }
    }
}
