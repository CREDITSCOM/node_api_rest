using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class SmartContractMethodsModel : AbstractResponseApiModel
    {
        public SmartContractMethodsModel()
        {
            Methods = new List<ContractMethod>();
        }
        public ICollection<ContractMethod> Methods { get; set; }
    }
}
