using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class SmartContractExecuteModel
    {
        public SmartContractExecuteModel()
        {
            Params = new List<string>();
        }
        public string Method { get; set; }

        public List<string> Params { get; set; }
    }
}
