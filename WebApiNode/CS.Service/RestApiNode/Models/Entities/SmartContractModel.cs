namespace CS.Service.RestApiNode.Models
{
    public class SmartContractModel
    {
        public bool ForgetNewState { get; set; }

        public SmartContractExecuteModel Execute { get; set; }

        public SmartContractDeployModel Deploy { get; set; }

    }
}
