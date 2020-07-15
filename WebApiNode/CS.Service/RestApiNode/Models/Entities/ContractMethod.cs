using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class ContractMethod
    {
        public ContractMethod()
        {
            Arguments = new List<ModelMethodArgument> ();
            Annotations = new List<ModelAnnotation>();
        }
        public string Name { get; set; }
        public string ReturnType { get; set; }

        public ICollection<ModelMethodArgument> Arguments { get; set; }

        public ICollection<ModelAnnotation> Annotations { get; set; }
    }
}
