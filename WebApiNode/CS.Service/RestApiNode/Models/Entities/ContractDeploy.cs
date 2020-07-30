using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class ContractDeploy
    {
        public ContractDeploy()
        {
            ByteCodeObjects = new List<BCObject>();
        }
        public string SourceCode { get; set; }

        public ICollection<BCObject> ByteCodeObjects { get; set; }
    }
}
