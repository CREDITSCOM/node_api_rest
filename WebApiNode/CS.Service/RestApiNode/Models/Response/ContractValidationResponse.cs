namespace CS.Service.RestApiNode.Models
{
    //std::string sourceCode;
    //std::vector< ::general::ByteCodeObject> byteCodeObjects;
    //std::string hashState;
    //int32_t tokenStandard;

    public class ContractValidationResponse : AbstractResponseApiModel
    {
        public ContractDeploy Deploy { get; set; }

        public int TokenStandard { get; set; }
    }
}
