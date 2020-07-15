using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class SingleQueryModel
    {
        public string Address { get; set; }
        
        public string FromId { get; set; }

        public IEnumerable<SingleTokenQueryModel> TokenQueries { get; set; }
    }
}
