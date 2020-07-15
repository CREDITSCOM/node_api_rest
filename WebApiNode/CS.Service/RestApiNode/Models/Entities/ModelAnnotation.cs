using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class ModelAnnotation
    {
        public ModelAnnotation()
        {
            Arguments = new Dictionary<string, string>();
        }
        public string Name { get; set; }

        public IDictionary<string, string> Arguments { get; set; }

    }
}
