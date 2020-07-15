using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class ModelMethodArgument
    {
        public ModelMethodArgument()
        {
            Annotations = new List<ModelAnnotation>();
        }
        public string Name { get; set; }
        public string Type { get; set; }

        public ICollection<ModelAnnotation> Annotations { get; set; }
    }
}
